using System.ComponentModel.DataAnnotations.Schema;


namespace BTX.ReportViewer.DataLayer
{
    public class SalesSummarySegmentBE
    {
        // public int xRank { get; set; }
        public string Color { get; set; }

        public string Country { get; set; }

        public string Product { get; set; }

        public string Varietal { get; set; }

        public string MyCategory { get; set; }

        public string PriceBand { get; set; }

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

        public decimal TY13Amount { get; set; }
        public decimal TY13AmountPct { get; set; }

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

        public decimal TY_TD_9LCases
        {
            get
            {
                return TY_9LCases / 1000;
            }
        }
        public decimal LY_TD_9LCases
        {
            get
            {
                return LY_9LCases / 1000;
            }
        }
        public decimal TY_TD_9LCasesPct
        {
            get
            {
                return TY_9LCasesPct;
            }
        }
        public decimal TY_TD_TotalSalesAmount
        {
            get
            {
                return TY_TotalSalesAmount / 1000;
            }
        }
        public decimal LY_TD_TotalSalesAmount
        {
            get
            {
                return LY_TotalSalesAmount / 1000;
            }
        }
        public decimal TY_TD_TotalSalesAmountPct
        {
            get
            {
                return TY_TotalSalesAmountPct;
            }
        }
        public int TY_TD_Units
        {
            get
            {
                return (int)(TY_Units / 1000);
            }
        }
        public int LY_TD_Units
        {
            get
            {
                return (int)(LY_Units / 1000);
            }
        }
        public decimal TY_TD_UnitsPct
        {
            get
            {
                return TY_UnitsPct;
            }
        }

    }
}

