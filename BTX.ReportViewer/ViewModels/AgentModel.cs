using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTX.ReportViewer.ViewModels
{
    public class AgentModel
    {
        string period { get; set; }
        int agentid { get; set; }
        string setnames { get; set; }
        string unitsizes { get; set; }
        decimal pricefrom { get; set; }
        decimal priceto { get; set; }

        public AgentModel()
        {
            this.period = string.Empty;
            this.agentid = 0;
            this.setnames = string.Empty;
            this.unitsizes = string.Empty;
            this.pricefrom = 0;
            this.priceto = 0;
        }

        public AgentModel(string period, int agentid, string setnames, string unitsizes, decimal pricefrom, decimal priceto)
        {
            this.period = period;
            this.agentid = agentid;
            this.setnames = setnames;
            this.unitsizes = unitsizes;
            this.pricefrom = pricefrom;
            this.priceto = priceto;
        }
    }
}