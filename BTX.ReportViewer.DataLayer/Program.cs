using BTX.ReportViewer.DataLayer;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Globalization;

namespace BTX.ReportViewer.DataLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SWIG_DW"].ConnectionString))
            //{

            //    var output =  connection.Query<MarketReportBE>(@"EXEC [lcbo].[usp_OLAPtest]", 50).ToList();

            //    foreach (MarketReportBE sale in output)
            //    {
            //        Console.WriteLine("Name: " + sale.AgentName);
            //        Console.WriteLine("9L Cases: " + sale.LY1y_9LCases);
            //    }
            //}

            IGridsRepository repository = new Dapper.GridsRepository();
            //var output = repository.GetTop50SalesCube("2016P11", "ONTARIO RED VQA,ONTARIO WHITE VQA");
            var output = repository.GetLicenseeDetails("2017-P3", "Bier ()");
            foreach (LicenseeDetailsBE report in output)
            {
                
            }

            Console.ReadLine();
        }
    }
}
