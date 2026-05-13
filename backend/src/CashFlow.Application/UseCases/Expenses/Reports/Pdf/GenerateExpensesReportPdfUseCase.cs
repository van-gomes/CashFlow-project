using System.Globalization;
using System.Reflection;
using CashFlow.Application.UseCases.Expenses.Register.Reports.Colors;
using CashFlow.Application.UseCases.Expenses.Register.Reports.PDF.Fonts;
using CashFlow.Domain;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Register.Reports.PDF;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "€";
    private const int HEIGHT_ROW_EXPENSE_TABLE = 25;
    private const int HEIGHT_ROW_WHITE_SPACE = 30;

    private readonly IExpensesReadOnlyRepository _repository;

    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
    {
        _repository = repository;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.FilterByMonth(month);

        if (expenses.Count == 0)
            return [];

        var document = CreateDocument(month);
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(page);

        var totalExpenses = expenses.Sum(expense => expense.Amount);
        CreateTotalSpentSection(page, month, totalExpenses);

        foreach (var expense in expenses)
        {
            CreateExpenseSection(page, expense);
        }

        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();

        document.Info.Title =
            $"{ResourceReportGenerationMessages.expensesFor} {month.ToString("Y", CultureInfo.InvariantCulture)}";

        document.Info.Author = "Vânia Gomes";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();

        table.AddColumn(Unit.FromPoint(45));
        table.AddColumn(Unit.FromPoint(300));

        var row = table.AddRow();
        row.Height = Unit.FromPoint(40);

        var pathFile = GetProfilePhotoPath();

        if (!string.IsNullOrWhiteSpace(pathFile) && File.Exists(pathFile))
        {
            var image = row.Cells[0].AddImage(pathFile);
            image.LockAspectRatio = true;
            image.Width = Unit.FromPoint(32);
        }

        row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

        row.Cells[1].AddParagraph("Hey, Vânia Gomes");
        row.Cells[1].Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 16,
            Color = ColorsHelper.BLACK
        };

        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private static string? GetProfilePhotoPath()
    {
        const string fileName = "ProfilePhoto.png";

        var currentDirectory = Directory.GetCurrentDirectory();

        while (!string.IsNullOrWhiteSpace(currentDirectory))
        {
            var files = Directory.GetFiles(
                currentDirectory,
                fileName,
                SearchOption.AllDirectories);

            var file = files.FirstOrDefault();

            if (file is not null)
                return file;

            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }

        return null;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = 40;
        paragraph.Format.SpaceAfter = 40;

        var title = string.Format(
            ResourceReportGenerationMessages.totalSpentIn,
            month.ToString("Y", CultureInfo.InvariantCulture));

        paragraph.AddFormattedText(title, new Font
        {
            Name = FontHelper.RALEWAY_REGULAR,
            Size = 15,
            Color = ColorsHelper.BLACK
        });

        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{totalExpenses:F2} {CURRENCY_SYMBOL}", new Font
        {
            Name = FontHelper.WORKSANS_BLACK,
            Size = 50,
            Color = ColorsHelper.BLACK
        });
    }

    private void CreateExpenseSection(Section page, Expense expense)
    {
        var hasDescription = string.IsNullOrWhiteSpace(expense.Description) == false;

        var table = CreateExpenseTable(page);

        var titleRow = table.AddRow();
        titleRow.Height = HEIGHT_ROW_EXPENSE_TABLE;
        titleRow.KeepWith = hasDescription ? 2 : 1;

        AddExpenseTitle(titleRow.Cells[0], expense.Title);
        AddHeaderForAmount(titleRow.Cells[3]);

        var informationRow = table.AddRow();
        informationRow.Height = HEIGHT_ROW_EXPENSE_TABLE;

        informationRow.Cells[0].AddParagraph(expense.Date.ToString("D", CultureInfo.InvariantCulture));
        SetStyleBaseForExpenseInformation(informationRow.Cells[0]);
        informationRow.Cells[0].Format.LeftIndent = 20;

        informationRow.Cells[1].AddParagraph(expense.Date.ToString("t", CultureInfo.InvariantCulture));
        SetStyleBaseForExpenseInformation(informationRow.Cells[1]);

        informationRow.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
        SetStyleBaseForExpenseInformation(informationRow.Cells[2]);

        AddAmountForExpense(informationRow.Cells[3], expense.Amount);

        if (hasDescription)
        {
            var descriptionRow = table.AddRow();
            descriptionRow.Height = HEIGHT_ROW_EXPENSE_TABLE;

            descriptionRow.Cells[0].AddParagraph(expense.Description!);
            descriptionRow.Cells[0].Format.Font = new Font
            {
                Name = FontHelper.WORKSANS_REGULAR,
                Size = 10,
                Color = ColorsHelper.BLACK
            };

            descriptionRow.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
            descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            descriptionRow.Cells[0].MergeRight = 2;
            descriptionRow.Cells[0].Format.LeftIndent = 20;

            informationRow.Cells[3].MergeDown = 1;
        }

        AddWhiteSpace(table);
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();

        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private void AddExpenseTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 14,
            Color = ColorsHelper.BLACK
        };

        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.amount);
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 14,
            Color = ColorsHelper.WHITE
        };

        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font
        {
            Name = FontHelper.WORKSANS_REGULAR,
            Size = 12,
            Color = ColorsHelper.BLACK
        };

        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"-{amount:F2} {CURRENCY_SYMBOL}");
        cell.Format.Font = new Font
        {
            Name = FontHelper.WORKSANS_REGULAR,
            Size = 14,
            Color = ColorsHelper.BLACK
        };

        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private static void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = HEIGHT_ROW_WHITE_SPACE;
        row.Borders.Visible = false;
    }

    private static byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}