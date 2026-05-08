namespace CashFlow.Domain.Reports
{
    using System;

    public class ResourceReportGenerationMessages
    {
        private static System.Resources.ResourceManager resourceMan;
        private static System.Globalization.CultureInfo resourceCulture;

        internal ResourceReportGenerationMessages()
        {
        }

        public static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.Equals(null, resourceMan))
                {
                    var temp = new System.Resources.ResourceManager(
                        "CashFlow.Domain.Reports.ResourceReportGenerationMessages",
                        typeof(ResourceReportGenerationMessages).Assembly);

                    resourceMan = temp;
                }

                return resourceMan;
            }
        }

        public static System.Globalization.CultureInfo Culture
        {
            get => resourceCulture;
            set => resourceCulture = value;
        }

        public static string title => ResourceManager.GetString("title", resourceCulture);
        public static string date => ResourceManager.GetString("date", resourceCulture);
        public static string paymentType => ResourceManager.GetString("paymentType", resourceCulture);
        public static string amount => ResourceManager.GetString("amount", resourceCulture);
        public static string description => ResourceManager.GetString("description", resourceCulture);
        public static string cach => ResourceManager.GetString("cach", resourceCulture);
        public static string creditCard => ResourceManager.GetString("creditCard", resourceCulture);
        public static string debitCard => ResourceManager.GetString("debitCard", resourceCulture);
        public static string eletronicTransfer => ResourceManager.GetString("eletronicTransfer", resourceCulture);
        public static string expensesFor => ResourceManager.GetString("expensesFor", resourceCulture);
    }
}