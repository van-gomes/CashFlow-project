using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Register.Reports.Excel;
public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    public async Task<byte[]> Execute(DateOnly month)
    {
        var workbook = new XLWorkbook();

        workbook.Author = "Welisson Arley";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";

        var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

        InsertHeader(worksheet);
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.title;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.date;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.paymentType;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.amount;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.description;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;

        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }
}