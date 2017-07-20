using System.ComponentModel.DataAnnotations.Schema;


namespace BTX.ReportViewer.DataLayer
{
    public class MarketReportBE
    {
        public int xRank { get; set; }
        public string AgentName { get; set; }
        public int AgentId { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string UnitSizeML { get; set; }
        public string UnitsPerCase { get; set; }
        public decimal AvgPrice { get; set; }
        public string Type { get; set; }
        public int Dist { get; set; }
        public int Dist_Var { get; set; }
        public int Nb_General { get; set; }
        public int Nb_VintageEssential { get; set; }
        public int Nb_Vintages { get; set; }
        public decimal TY_9LCases { get; set; }
        public decimal LY_9LCases { get; set; }
        public decimal TY_9LCasesPct { get; set; }
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
        public decimal TY1y_9LCases { get; set; }
        public decimal LY1y_9LCases { get; set; }
        public decimal TY1y_9LCasesPct { get; set; }
        public decimal TY1y_TotalSalesAmount { get; set; }
        public decimal LY1y_TotalSalesAmount { get; set; }
        public decimal TY1y_TotalSalesAmountPct { get; set; }
        public int TY1y_Units { get; set; }
        public int LY1y_Units { get; set; }
        public decimal TY1y_UnitsPct { get; set; }

        public int OnPrem13PerTY { get; set; }
        public int OnPrem13PerLY { get; set; }
        public decimal TY_OnPrem13PerPct { get; set; }
        public int Agencies13PerTY { get; set; }
        public int Agencies13PerLY { get; set; }
        public decimal TY_Agencies13PerPct { get; set; }
        public string Promo { get; set; }

    }
}
