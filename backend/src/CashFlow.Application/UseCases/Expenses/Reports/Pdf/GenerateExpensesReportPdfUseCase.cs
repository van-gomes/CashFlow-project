using CashFlow.Application.UseCases.Expenses.Register.Reports.PDF.Fonts;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Register.Reports.PDF;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "€";
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
        {
            return [];
        }

        var document = CreateDocument(month);
        var page = CreatePage(document);

        var table = page.AddTable();
        table.Borders.Visible = false;

        table.AddColumn(Unit.FromCentimeter(1.8));
        table.AddColumn(Unit.FromCentimeter(8));

        var row = table.AddRow();
        row.VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;

        row.Cells[0].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
        row.Cells[0].Format.Alignment = ParagraphAlignment.Center;

        var image = row.Cells[0].AddImage("/home/van-gomes/Downloads/foto_GDG.jpeg");

        image.Width = Unit.FromCentimeter(1.5);
        image.Height = Unit.FromCentimeter(1.5);
        image.LockAspectRatio = true;

        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
        row.Cells[1].Format.LeftIndent = Unit.FromCentimeter(0.2);

        var nameParagraph = row.Cells[1].AddParagraph("Hey, Vânia Gomes");

        nameParagraph.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 12
        };

        nameParagraph.Format.SpaceBefore = 0;
        nameParagraph.Format.SpaceAfter = 0;

        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        var title = string.Format(
            ResourceReportGenerationMessages.totalSpentIn,
            month.ToString("Y"));

        paragraph.AddFormattedText(
            title,
            new Font
            {
                Name = FontHelper.RALEWAY_REGULAR,
                Size = 15
            });

        paragraph.AddLineBreak();

        var totalExpenses = expenses.Sum(expense => expense.Amount);

        paragraph.AddFormattedText(
            $"{totalExpenses} {CURRENCY_SYMBOL}",
            new Font
            {
                Name = FontHelper.WORKSANS_BLACK,
                Size = 50
            });

        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();

        document.Info.Title = $"{ResourceReportGenerationMessages.expensesFor} {month:Y}";
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
    
    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}