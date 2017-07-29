using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTX.ReportViewer.ViewModels
{
    public class GridsModel
    {
        public int Id { set; get; }

        public string CurrentUser;
        public string CurrentUserFirstName;
        public string CurrentUserSurName;
        public string CurrentUserEmail;
        public string SamAccount;

        //for SubGrid: SalesByProducers
        public string period { get; set; }
        public int agentid { get; set; }
        public string agentname { get; set; }
        public string setnames { get; set; }
        public string unitsizes { get; set; }
        public decimal pricefrom { get; set; }
        public decimal priceto { get; set; }

        public int brandid { get; set; }
        public string brandname { get; set; }
        public int selectedgroupid { get; set; }
        public int userid{get;set;}
        public string salesname { get; set; }
        public string accountnumber { get; set; }
        public string accountname { get; set; }

        public string colorname { get; set; }
        public string countryname { get; set; }
        public string cspc { get; set; }
        public string productname { get; set; }

        public string varietalname { get; set; }

        public string mycategoryname { get; set; }

        public string pricebandname { get; set; }

        public int salesrepid { get; set; }
        public string category { get; set; }
        public int isclientonly { get; set; }

        public GridsModel()
        {
            this.CurrentUser = string.Empty;
            this.CurrentUserFirstName = string.Empty;
            this.CurrentUserSurName = string.Empty;
            this.CurrentUserEmail = string.Empty;
            this.SamAccount = string.Empty;
            this.colorname = string.Empty;
            this.countryname = string.Empty;
            this.pricebandname = string.Empty;
            this.isclientonly = 0;
        }

        public GridsModel(string period, int agentid, string agentname, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid)
        {
            this.period = period;
            this.agentid = agentid;
            this.agentname = agentname;
            this.setnames = setnames;
            this.unitsizes = unitsizes;
            this.pricefrom = pricefrom;
            this.priceto = priceto;
            this.selectedgroupid = selectedgroupid;
        }

        public GridsModel(string period, int agentid, string agentname, string setnames, int brandid, string brandname, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid)
        {
            this.period = period;
            this.agentid = agentid;
            this.agentname = agentname;
            this.setnames = setnames;
            this.brandid = brandid;
            this.brandname = brandname;
            this.unitsizes = unitsizes;
            this.pricefrom = pricefrom;
            this.priceto = priceto;
            this.selectedgroupid = selectedgroupid;
        }

        public GridsModel(int userid, string salesname, string period, int selectedgroupid)
        {
            this.userid = userid;
            this.salesname = salesname;
            this.period = period;
            this.selectedgroupid = selectedgroupid;
        }

        public GridsModel(int userid, string period, string accountnumber, string salesname, string accountnname, int selectedgroupid)
        {
            this.userid = userid;
            this.period = period;
            this.accountnumber = accountnumber;
            this.salesname = salesname;
            this.accountname = accountnname;
            this.selectedgroupid = selectedgroupid;
        }
        public GridsModel(int userid, string period, int isclientonly,  string accountnumber, string salesname, string accountnname, int selectedgroupid)
        {
            this.userid = userid;
            this.period = period;
            this.accountnumber = accountnumber;
            this.salesname = salesname;
            this.accountname = accountnname;
            this.selectedgroupid = selectedgroupid;
            this.isclientonly = isclientonly;
        }

        public GridsModel(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, int type, string value)
        {
            this.period = period;
            this.agentid = agentid;
            this.agentname = agentname;
            this.setnames = setnames;
            this.unitsizes = unitsizes;
            this.pricefrom = pricefrom;
            this.priceto = priceto;
            this.selectedgroupid = selectedgroupid;
            if (type == 1)
                this.colorname = value;
            else if (type == 2)
                this.countryname = value;
            else if (type == 3)
                this.varietalname = value;
            else if (type == 4)
                this.mycategoryname = value;
            else if (type == 5)
                this.pricebandname = value;
        }

        public GridsModel(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, string colorname, string category)
        {
            this.period = period;
            this.agentid = agentid;
            this.agentname = agentname;
            this.setnames = setnames;
            this.unitsizes = unitsizes;
            this.pricefrom = pricefrom;
            this.priceto = priceto;
            this.selectedgroupid = selectedgroupid;
            this.colorname = colorname;
            this.category = category;
        }

        public GridsModel(string period, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int selectedgroupid, int type, string value1, string value2)
        {
            this.period = period;
            this.agentid = agentid;
            this.agentname = agentname;
            this.setnames = setnames;
            this.unitsizes = unitsizes;
            this.pricefrom = pricefrom;
            this.priceto = priceto;
            this.selectedgroupid = selectedgroupid;
            if (type == 1)  // by color, country
            {
                this.colorname = value1;
                this.countryname = value2;
            }
            else if (type == 2)
            {
                this.colorname = value2;
                this.countryname = value1;
            }
            else if (type == 4)
            {
                this.mycategoryname = value1;
                this.countryname = value2;
            }
            else if (type == 5)
            {
                this.pricebandname = value1;
                this.mycategoryname = value2;
            }
        }

        public GridsModel(string productname, string cspc, string period, int selectedgroupid)
        {
            this.productname = productname;
            this.cspc = cspc;
            this.period = period;
            this.selectedgroupid = selectedgroupid;
        }

        public GridsModel(string category, string period, int isclientonly, string accountnumber, string accountname)
        {
            this.category = category;
            this.period = period;
            this.isclientonly = isclientonly;
            this.accountnumber = accountnumber;
            this.accountname = accountname;
        }
    }
}