using System.Collections.Generic;

namespace BTX.ReportViewer.DataLayer
{
    public class SalesPersonalReportBE
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public string UnitSizeML { get; set; }
        public string Category { get; set; }
        public decimal RetailPrice { get; set; }
        public int IsTotal { get; set; }

        //Promo Turn
        public decimal TY_PromoTurn_9LCases { get; set; }
        public decimal LY_PromoTurn_9LCases { get; set; }
        public decimal PromoTurn_9LCases_Pct { get; set; }
        public decimal TY_PromoTurn_SalesAmount { get; set; }
        public decimal LY_PromoTurn_SalesAmount { get; set; }
        public decimal PromoTurn_SalesAmount_Pct { get; set; }
        public decimal TY_PromoTurn_Units { get; set; }
        public decimal LY_PromoTurn_Units { get; set; }
        public decimal PromoTurn_Units_Pct { get; set; }

        //Store 6P
        public decimal Store_LY_6P_9LCases { get; set; }
        public decimal Store_TY_6P_9LCases { get; set; }
        public decimal Store_6P_9LCasesPct { get; set; }
        public decimal Store_LY_6P_SalesAmount { get; set; }
        public decimal Store_TY_6P_SalesAmount { get; set; }
        public decimal Store_6P_SalesAmountPct { get; set; }
        public int Store_TY_6P_Units { get; set; }
        public int Store_LY_6P_Units { get; set; }
        public decimal Store_6P_UnitsPct { get; set; }

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

        //Lic 6P
        public decimal Lic_LY_6P_9LCases { get; set; }
        public decimal Lic_TY_6P_9LCases { get; set; }
        public decimal Lic_6P_9LCasesPct { get; set; }
        public decimal Lic_LY_6P_SalesAmount { get; set; }
        public decimal Lic_TY_6P_SalesAmount { get; set; }
        public decimal Lic_6P_SalesAmountPct { get; set; }
        public int Lic_TY_6P_Units { get; set; }
        public int Lic_LY_6P_Units { get; set; }
        public decimal Lic_6P_UnitsPct { get; set; }

        //Lic 13P
        public decimal Lic_LY_13P_9LCases { get; set; }
        public decimal Lic_TY_13P_9LCases { get; set; }
        public decimal Lic_13P_9LCasesPct { get; set; }
        public decimal Lic_LY_13P_SalesAmount { get; set; }
        public decimal Lic_TY_13P_SalesAmount { get; set; }
        public decimal Lic_13P_SalesAmountPct { get; set; }
        public int Lic_TY_13P_Units { get; set; }
        public int Lic_LY_13P_Units { get; set; }
        public decimal Lic_13P_UnitsPct { get; set; }

        //Not defined
        public string IsListed { get; set; }
        public int StoreInv { get; set; }
        public string Promo { get; set; }

        //Cur Per
        public decimal CurPer_9LCases { get; set; }
        public decimal CurPer_SalesAmount { get; set; }
        public decimal CurPer_Units { get; set; }

        //Category
        public int SetCode { get; set; }
        public string SetName { get; set; }
        public decimal SalesAmount_PromoTurn_TY { get; set; }
        public decimal SalesAmount_PromoTurn_LY { get; set; }
        public decimal SalesAmount_PromoTurn_Pct { get; set; }
        public decimal SalesAmount_6P_TY { get; set; }
        public decimal SalesAmount_6P_LY { get; set; }
        public decimal SalesAmount_6P_Pct { get; set; }
        public decimal SalesAmount_13P_TY { get; set; }
        public decimal SalesAmount_13P_LY { get; set; }
        public decimal SalesAmount_13P_Pct { get; set; }
        public decimal SalesAmount_TY { get; set; }
        public decimal SalesAmount_LY { get; set; }        
        public decimal Avg_Retail_PricePct { get; set; }
        public int xRank { get; set; }

        //ProductsNotInStore
        public string Period { get; set; }
        public string ProductNumber { get; set; }
        public decimal Toronto { get; set; }
        public decimal Ottawa { get; set; }
        public decimal Whitby { get; set; }
        public decimal London { get; set; }
        public decimal ThunderBay { get; set; }
        public decimal Mississauga { get; set; }
        public decimal TorontoToronto { get; set; }

    }
}
