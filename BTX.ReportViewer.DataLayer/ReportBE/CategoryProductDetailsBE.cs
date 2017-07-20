using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTX.ReportViewer.DataLayer
{
    public class CategoryProductDetailsBE
    {
        public int SetCode { get; set; }
        public string SetName { get; set; }
        public string ProductName { get; set; }
        public string CSPC { get; set; }
        public string UnitSizeML { get; set; }
        public string IsListed { get; set; }
        public string TY_Promo_Turn { get; set; }
        public string LY_Promo_Turn { get; set; }
        public decimal SalesAmount_6P_TY { get; set; }
        public decimal SalesAmount_6P_LY { get; set; }
        public decimal SalesAmount_6P_Pct { get; set; }
        public decimal SalesAmount_13P_TY { get; set; }
        public decimal SalesAmount_13P_LY { get; set; }
        public decimal SalesAmount_13P_Pct { get; set; }
        public int InventoryCount { get; set; }
        public string Promo { get; set; }
        public int xRankSetByProduct { get; set; }
        public int xRank { get; set; }
    }
}
