using System.ComponentModel.DataAnnotations.Schema;


namespace BTX.ReportViewer.DataLayer
{
    public class SalesSummaryByColorDetailBE
    {
        // public int xRank { get; set; }
        public string SalesRepId { get; set; }
        public string SalesRep { get; set; }
        public string Category { get; set; }

        public decimal Week1 { get; set; }

        public decimal Week2 { get; set; }
        public decimal Week3 { get; set; }
        public decimal Week4 { get; set; }

        public decimal TY_9LCases { get; set; }
        public decimal LY_9LCases { get; set; }
        public decimal TY_9LCasesPct { get; set; }

        public int Nb_General { get; set; }
        public int Nb_VintageEssential { get; set; }
        public int Nb_Vintages { get; set; }
        //public int AgentId { get; set; }
        //public int BrandId { get; set; }
        //public string BrandName { get; set; }
        //public string ProductId { get; set; }
        //public string ProductName { get; set; }
        //public string UnitSizeML { get; set; }
        //public string UnitsPerCase { get; set; }
        //public decimal AvgPrice { get; set; }
        //public string Type { get; set; }
        //public int Dist { get; set; }
        //public int Dist_Var { get; set; }



        public decimal TY_TotalSalesAmount { get; set; }
        public decimal LY_TotalSalesAmount { get; set; }
        public decimal TY_TotalSalesAmountPct { get; set; }
        public int TY_Units { get; set; }
        public int LY_Units { get; set; }
        public decimal TY_UnitsPct { get; set; }
        public decimal TY6m_9LCases { get; set; }
        public decimal LY6m_9LCases { get; set; }
        public decimal TY6m_9LCasesPct { get; set; }
        public decimal TY6m_TotalSalesAmount { get; set; }
        public decimal LY6m_TotalSalesAmount { get; set; }
        public decimal TY6m_TotalSalesAmountPct { get; set; }
        public int TY6m_Units { get; set; }
        public int LY6m_Units { get; set; }
        public decimal TY6m_UnitsPct { get; set; }

        public decimal LYYTD_9LCases { get; set; }
        public decimal TYYTD_9LCases { get; set; }
        public decimal TYYTD_9LCasesPct { get; set; }
        public decimal LYYTD_Units { get; set; }
        public decimal TYYTD_Units { get; set; }
        public decimal TYYTD_UnitsPct { get; set; }
        public decimal LYYTD_SalesAmount { get; set; }
        public decimal TYYTD_SalesAmount { get; set; }
        public decimal TYYTD_SalesAmountPct { get; set; }

        public decimal LY13_OnPrem { get; set; }
        public decimal TY13_OnPrem { get; set; }
        public decimal TY13_OnPremPct { get; set; }

        //public decimal TY1y_9LCases { get; set; }
        //public decimal LY1y_9LCases { get; set; }
        //public decimal TY1y_9LCasesPct { get; set; }
        //public decimal TY1y_TotalSalesAmount { get; set; }
        //public decimal LY1y_TotalSalesAmount { get; set; }
        //public decimal TY1y_TotalSalesAmountPct { get; set; }
        //public int TY1y_Units { get; set; }
        //public int LY1y_Units { get; set; }
        //public decimal TY1y_UnitsPct { get; set; }

        //public int OnPrem13PerTY { get; set; }
        //public int OnPrem13PerLY { get; set; }
        //public decimal TY_OnPrem13PerPct { get; set; }
        //public int Agencies13PerTY { get; set; }
        //public int Agencies13PerLY { get; set; }
        //public decimal TY_Agencies13PerPct { get; set; }
        //public string Promo { get; set; }

    }
}


