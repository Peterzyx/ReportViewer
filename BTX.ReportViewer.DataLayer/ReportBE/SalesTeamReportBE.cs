
namespace BTX.ReportViewer.DataLayer
{
    public class SalesTeamReportBE
    {
        public int TerrNo { get; set; }
        public string SalesRepName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Rank_SalesAmount { get; set; }
        public int Rank_Units { get; set; }
        public int Rank_9LEquivalentCases { get; set; }
        public int IsTotal { get; set; }

        //All Accounts 13P
        public decimal All_LY_13P_9LCases { get; set; }
        public decimal All_TY_13P_9LCases { get; set; }
        public decimal All_13P_9LCasesPct { get; set; }
        public decimal All_LY_13P_SalesAmount { get; set; }
        public decimal All_TY_13P_SalesAmount { get; set; }
        public decimal All_13P_SalesAmountPct { get; set; }
        public int All_TY_13P_Units { get; set; }
        public int All_LY_13P_Units { get; set; }
        public decimal All_13P_UnitsPct { get; set; }

        //all 3P
        public decimal All_LY_3P_9LCases { get; set; }
        public decimal All_TY_3P_9LCases { get; set; }
        public decimal All_3P_9LCasesPct { get; set; }
        public decimal All_LY_3P_SalesAmount { get; set; }
        public decimal All_TY_3P_SalesAmount { get; set; }
        public decimal All_3P_SalesAmountPct { get; set; }
        public int All_TY_3P_Units { get; set; }
        public int All_LY_3P_Units { get; set; }
        public decimal All_3P_UnitsPct { get; set; }


        //Store 13P
        public decimal Store_LY_13P_9LCases { get; set; }
        public decimal Store_TY_13P_9LCases { get; set; }
        public decimal Store_13P_9LCasesPct { get; set; }
        public decimal Store_LY_13P_SalesAmount { get; set; }
        public decimal Store_TY_13P_SalesAmount { get; set; }
        public decimal Store_13P_SalesAmountPct { get; set; }
        public int Store_TY_13P_Units { get; set; }
        public int Store_LY_13P_Units { get; set; }
        public decimal Store_13P_UnitsPct { get; set; }

        //Store 3P
        public decimal Store_LY_3P_9LCases { get; set; }
        public decimal Store_TY_3P_9LCases { get; set; }
        public decimal Store_3P_9LCasesPct { get; set; }
        public decimal Store_LY_3P_SalesAmount { get; set; }
        public decimal Store_TY_3P_SalesAmount { get; set; }
        public decimal Store_3P_SalesAmountPct { get; set; }
        public int Store_TY_3P_Units { get; set; }
        public int Store_LY_3P_Units { get; set; }
        public decimal Store_3P_UnitsPct { get; set; }

        //Licensee
        public int Tot_Rank_9LEquivalentCases { get; set; }
        public int Tot_Rank_SalesAmount { get; set; }
        public int Tot_Rank_Units { get; set; }
        public decimal Tot_LY_13P_9LCases { get; set; }
        public decimal Tot_TY_13P_9LCases { get; set; }
        public decimal Tot_13P_9LCasesPct { get; set; }

        public decimal Tot_LY_13P_SalesAmount { get; set; }
        public decimal Tot_TY_13P_SalesAmount { get; set; }
        public decimal Tot_13P_SalesAmountPct { get; set; }

        public decimal Tot_LY_13P_Units { get; set; }
        public decimal Tot_TY_13P_Units { get; set; }
        public decimal Tot_13P_UnitsPct { get; set; }

        public decimal Tot_LY_3P_9LCases { get; set; }
        public decimal Tot_TY_3P_9LCases { get; set; }
        public decimal Tot_3P_9LCasesPct { get; set; }

        public decimal Tot_LY_3P_SalesAmount { get; set; }
        public decimal Tot_TY_3P_SalesAmount { get; set; }
        public decimal Tot_3P_SalesAmountPct { get; set; }

        public decimal Tot_LY_3P_Units { get; set; }
        public decimal Tot_TY_3P_Units { get; set; }
        public decimal Tot_3P_UnitsPct { get; set; }
        
        public decimal Lic_LY_13P_9LCases { get; set; }
        public decimal Lic_TY_13P_9LCases { get; set; }
        public decimal Lic_13P_9LCasesPct { get; set; }

        public decimal Lic_LY_13P_SalesAmount { get; set; }
        public decimal Lic_TY_13P_SalesAmount { get; set; }
        public decimal Lic_13P_SalesAmountPct { get; set; }

        public int Lic_TY_13P_Units { get; set; }
        public int Lic_LY_13P_Units { get; set; }
        public decimal Lic_13P_UnitsPct { get; set; }

        public decimal Lic_LY_3P_9LCases { get; set; }
        public decimal Lic_TY_3P_9LCases { get; set; }
        public decimal Lic_3P_9LCasesPct { get; set; }

        public decimal Lic_LY_3P_SalesAmount { get; set; }
        public decimal Lic_TY_3P_SalesAmount { get; set; }
        public decimal Lic_3P_SalesAmountPct { get; set; }

        public int Lic_TY_3P_Units { get; set; }
        public int Lic_LY_3P_Units { get; set; }
        public decimal Lic_3P_UnitsPct { get; set; }
        
        public decimal Dir_LY_13P_9LCases { get; set; }
        public decimal Dir_TY_13P_9LCases { get; set; }
        public decimal Dir_13P_9LCasesPct { get; set; }

        public decimal Dir_LY_13P_SalesAmount { get; set; }
        public decimal Dir_TY_13P_SalesAmount { get; set; }
        public decimal Dir_13P_SalesAmountPct { get; set; }

        public int Dir_TY_13P_Units { get; set; }
        public int Dir_LY_13P_Units { get; set; }
        public decimal Dir_13P_UnitsPct { get; set; }

        public decimal Dir_LY_3P_9LCases { get; set; }
        public decimal Dir_TY_3P_9LCases { get; set; }
        public decimal Dir_3P_9LCasesPct { get; set; }

        public decimal Dir_LY_3P_SalesAmount { get; set; }
        public decimal Dir_TY_3P_SalesAmount { get; set; }
        public decimal Dir_3P_SalesAmountPct { get; set; }

        public int Dir_TY_3P_Units { get; set; }
        public int Dir_LY_3P_Units { get; set; }
        public decimal Dir_3P_UnitsPct { get; set; }
    }
}
