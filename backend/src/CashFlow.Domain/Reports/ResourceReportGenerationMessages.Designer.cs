namespace CashFlow.Domain.Reports
{
    using System.Globalization;
    using System.Resources;

    public static class ResourceReportGenerationMessages
    {
        private static readonly ResourceManager ResourceManager =
            new(
                "CashFlow.Domain.Reports.ResourceReportGenerationMessages",
                typeof(ResourceReportGenerationMessages).Assembly);

        public static string title =>
            ResourceManager.GetString(nameof(title), CultureInfo.CurrentCulture)!;

        public static string date =>
            ResourceManager.GetString(nameof(date), CultureInfo.CurrentCulture)!;

        public static string paymentType =>
            ResourceManager.GetString(nameof(paymentType), CultureInfo.CurrentCulture)!;

        public static string amount =>
            ResourceManager.GetString(nameof(amount), CultureInfo.CurrentCulture)!;

        public static string description =>
            ResourceManager.GetString(nameof(description), CultureInfo.CurrentCulture)!;

        public static string cach =>
            ResourceManager.GetString(nameof(cach), CultureInfo.CurrentCulture)!;

        public static string creditCard =>
            ResourceManager.GetString(nameof(creditCard), CultureInfo.CurrentCulture)!;

        public static string debitCard =>
            ResourceManager.GetString(nameof(debitCard), CultureInfo.CurrentCulture)!;

        public static string eletronicTransfer =>
            ResourceManager.GetString(nameof(eletronicTransfer), CultureInfo.CurrentCulture)!;

        public static string expensesFor =>
            ResourceManager.GetString(nameof(expensesFor), CultureInfo.CurrentCulture)!;

        public static string totalSpentIn =>
            ResourceManager.GetString(nameof(totalSpentIn), CultureInfo.CurrentCulture)!;
    }
}