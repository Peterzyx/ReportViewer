using BTX.ReportViewer.MdxClient;
using BTX.ReportViewer.Logging;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BTX.ReportViewer.DataLayer.Dapper
{
    public class GridsRepository : IGridsRepository
    {
        private IDbConnection db_pelham = new SqlConnection(ConfigurationManager.ConnectionStrings["SWIG_DW"].ConnectionString);
        public static string GetSSASConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SWIG_DW_SSAS"].ConnectionString;
        }
        public static MdxConnection GetSSASConnection()
        {
            LoggingHelper.LogInformation(String.Format("Connecting to Analysis Server: {0}", GetSSASConnectionString()));
            return new MdxConnection(GetSSASConnectionString());
        }

        private int _client = int.Parse(ConfigurationManager.AppSettings["CompanyClientId"]);
        
        #region Top 50 Sales
        public List<MarketReportBE> GetTop50SalesCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.Top50Sales(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "AgentName", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases MAT]", "TY1y_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "LY1y_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "TY1y_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "TY1y_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "LY1y_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "TY1y_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "TY1y_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "LY1y_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "TY1y_UnitsPct", DbType.Double);

                //parms.Add("~[Measures].[Rol Year Per TY$]", "TY1y_TotalSalesAmount", DbType.Double);
                //parms.Add("~[Measures].[Rol Year $ Var %]", "TY1y_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "OnPrem13PerTY", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per LY]", "OnPrem13PerLY", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY_OnPrem13PerPct", DbType.Double);

                //Execute Query
                List<MarketReportBE> output = connection.Query<MarketReportBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }
        #endregion

        #region Sales by Brands
        public List<MarketReportBE> GetSalesByAgentsCube(string periodkey, string agentname, string setnames, string unitsizes, decimal pricefrom, decimal priceto)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesByAgents(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Agent", "[Product].[Agent].&[" + agentname + "]");

                //Obejct Row Mapping
                parms.Add("~0", "BrandName", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Nb_General]", "Nb_General", DbType.Double);
                parms.Add("~[Measures].[Nb_Vintages]", "Nb_Vintages", DbType.Double);
                parms.Add("~[Measures].[Nb_VintageEssential]", "Nb_VintageEssential", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases MAT]", "TY1y_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "LY1y_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "TY1y_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "TY1y_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "LY1y_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "TY1y_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "TY1y_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "LY1y_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "TY1y_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per TY]", "OnPrem13PerTY", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per LY]", "OnPrem13PerLY", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY_OnPrem13PerPct", DbType.Double);
                //parms.Add("~[Measures].[Rol Year Per TY$]", "TY1y_TotalSalesAmount", DbType.Double);
                //parms.Add("~[Measures].[Rol Year $ Var %]", "TY1y_TotalSalesAmountPct", DbType.Double);

                //Execute Query
                List<MarketReportBE> output = connection.Query<MarketReportBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }
        #endregion

        #region Sales by Products
        public List<MarketReportBE> GetSalesByProductsCube(string periodkey, string agentname, string brandname, string setnames, string unitsizes, decimal pricefrom, decimal priceto)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesByProducts(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                //parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Agent", "[Product].[Agent].&[" + agentname + "]");
                parms.Add("@Brand", "[Product].[Brand].&[" + brandname + "]");

                //Obejct Row Mapping
                parms.Add("~0", "ProductId", DbType.String);
                parms.Add("~1", "ProductName", DbType.String);
                parms.Add("~2", "UnitSizeML", DbType.String);
                parms.Add("~3", "UnitsPerCase", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Nb_General]", "Nb_General", DbType.Double);
                parms.Add("~[Measures].[Nb_Vintages]", "Nb_Vintages", DbType.Double);
                parms.Add("~[Measures].[Nb_VintageEssential]", "Nb_VintageEssential", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases MAT]", "TY1y_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "LY1y_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "TY1y_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "TY1y_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "LY1y_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "TY1y_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "TY1y_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "LY1y_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "TY1y_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per TY]", "OnPrem13PerTY", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per LY]", "OnPrem13PerLY", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY_OnPrem13PerPct", DbType.Double);
                //parms.Add("~[Measures].[Rol Year Per TY$]", "TY1y_TotalSalesAmount", DbType.Double);
                //parms.Add("~[Measures].[Rol Year $ Var %]", "TY1y_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Promo]", "Promo", DbType.String);

                //Execute Query
                List<MarketReportBE> output = connection.Query<MarketReportBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }
        #endregion

        #region Sales Summary for Store
        
        public List<SalesTeamReportBE> GetSalesTeamSummaryAllCube(string periodkey)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesTeamSummaryAll;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");

                //Obejct Row Mapping
                parms.Add("~0", "SalesRepName", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Terr No]", "TerrNo", DbType.Int16);
                parms.Add("~[Measures].[9L Eq Cases MAT]", "All_TY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "All_LY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "All_13P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "All_TY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "All_LY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "All_13P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "All_TY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "All_LY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "All_13P_UnitsPct", DbType.Double);

                //Execute Query
                List<SalesTeamReportBE> output = connection.Query<SalesTeamReportBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }
        #endregion

        #region Sales Summary for Store
        public List<SalesTeamReportBE> GetSalesTeamSummaryStoreCube(string periodkey)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesTeamSummaryStore;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                
                //Obejct Row Mapping
                parms.Add("~0", "SalesRepName", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Terr No]", "TerrNo", DbType.Int16);
                parms.Add("~[Measures].[9L Eq Cases MAT]", "Store_TY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "Store_LY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "Store_13P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "Store_TY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "Store_LY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "Store_13P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "Store_TY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "Store_LY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "Store_13P_UnitsPct", DbType.Double);

                //Execute Query
                List<SalesTeamReportBE> output = connection.Query<SalesTeamReportBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }
        #endregion

        #region Sales Summary for Licensee
        public List<SalesTeamReportBE> GetSalesTeamSummaryLicenseeCube(string periodkey)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesTeamSummaryLicensee;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                
                //Obejct Row Mapping
                parms.Add("~0", "SalesRepName", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Terr No]", "TerrNo", DbType.Int16);
                parms.Add("~[Measures].[9L Eq Cases MAT]", "Lic_TY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "Lic_LY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "Lic_13P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "Lic_TY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "Lic_LY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "Lic_13P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "Lic_TY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "Lic_LY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "Lic_13P_UnitsPct", DbType.Double);

                //Execute Query
                List<SalesTeamReportBE> output = connection.Query<SalesTeamReportBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }
        #endregion

        #region Sales Summary for Store by Territory
        public List<SalesTeamReportBE> GetSalesSummaryStoreByTerritoryCube(int userid, string periodkey, int sortgroup)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryStoreByTerritory;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UserId", "[Sales Rep].[Sales Rep].&[" + userid + "]");

                //Obejct Row Mapping
                parms.Add("~0", "AccountNumber", DbType.String);
                parms.Add("~1", "AccountName", DbType.String);
                parms.Add("~2", "City", DbType.String);
                parms.Add("~3", "Address", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Rank Sales Amount]", "Rank_SalesAmount", DbType.Int16);
                parms.Add("~[Measures].[Rank Units]", "Rank_Units", DbType.Int16);
                parms.Add("~[Measures].[Rank 9L Eq Cases MAT]", "Rank_9LEquivalentCases", DbType.Int16);
                
                parms.Add("~[Measures].[9L Eq Cases MAT]", "Store_TY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "Store_LY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "Store_13P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "Store_TY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "Store_LY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "Store_13P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "Store_TY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "Store_LY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "Store_13P_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases P3P]", "Store_TY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P3P]", "Store_LY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P3P Var %]", "Store_3P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P3P]", "Store_TY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P3P]", "Store_LY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P3P Var %]", "Store_3P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P3P]", "Store_TY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P3P]", "Store_LY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P3P Var %]", "Store_3P_UnitsPct", DbType.Double);

                //Execute Query
                List<SalesTeamReportBE> sales = connection.Query<SalesTeamReportBE>(query, parms).ToList();

                string queryTotal = DapperHelper.MdxQueryHelper.SalesSummaryStoreByTerritoryTotal;
                List<SalesTeamReportBE> salesTotal = connection.Query<SalesTeamReportBE>(queryTotal, parms).ToList();

                sales = sales.Concat(salesTotal).OrderBy(x => x.Rank_9LEquivalentCases).ToList();

                if (sales.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                List<SalesTeamReportBE> sortedsales = null; ;
                if (sortgroup == 0)
                {
                    sortedsales = sales.OrderBy(x => x.Rank_9LEquivalentCases).ToList();
                }
                else if (sortgroup == 1)
                {
                    sortedsales = sales.OrderBy(x => x.Rank_SalesAmount).ToList();
                }
                else
                {
                    sortedsales = sales.OrderBy(x => x.Rank_Units).ToList();
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return sortedsales;
            }
        }
        #endregion

        #region Sales Summary for Licensee by Territory
        public List<SalesTeamReportBE> GetSalesSummaryLicenseeByTerritoryCube(int userid, string periodkey, int sortgroup)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryLicenseeByTerritory;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UserId", "[Sales Rep].[Sales Rep].&[" + userid + "]");

                //Obejct Row Mapping
                parms.Add("~0", "AccountNumber", DbType.String);
                parms.Add("~1", "AccountName", DbType.String);
                parms.Add("~2", "City", DbType.String);
                parms.Add("~3", "Address", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Rank Sales Amount]", "Tot_Rank_SalesAmount", DbType.Int16);
                parms.Add("~[Measures].[Rank Units]", "Tot_Rank_Units", DbType.Int16);
                parms.Add("~[Measures].[Rank 9L Eq Cases MAT]", "Tot_Rank_9LEquivalentCases", DbType.Int16);

                parms.Add("~[Measures].[9L Eq Cases MAT]", "Tot_TY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "Tot_LY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "Tot_13P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "Tot_TY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "Tot_LY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "Tot_13P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "Tot_TY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "Tot_LY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "Tot_13P_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases P3P]", "Tot_TY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P3P]", "Tot_LY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P3P Var %]", "Tot_3P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P3P]", "Tot_TY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P3P]", "Tot_LY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P3P Var %]", "Tot_3P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P3P]", "Tot_TY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P3P]", "Tot_LY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P3P Var %]", "Tot_3P_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L 13P Direct Sales]", "Dir_TY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L 13P Direct Sales Prior]", "Dir_LY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L 13P Direct Sales Prior Var]", "Dir_13P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Direct Sales]", "Dir_TY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Direct Sales Prior]", "Dir_LY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Direct Sales Prior Var]", "Dir_13P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units 13P Direct Sales]", "Dir_TY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units 13P Direct Sales Prior]", "Dir_LY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units 13P Direct Sales Prior Var]", "Dir_13P_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L 3P Direct Sales]", "Dir_TY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L 3P Direct Sales Prior]", "Dir_LY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L 3P Direct Sales Prior Var]", "Dir_3P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Direct Sales]", "Dir_TY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Direct Sales Prior]", "Dir_LY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Direct Sales Prior Var]", "Dir_3P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units 3P Direct Sales]", "Dir_TY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units 3P Direct Sales Prior]", "Dir_LY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units 3P Direct Sales Prior Var]", "Dir_3P_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L 13P Licensee]", "Lic_TY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L 13P Licensee Prior]", "Lic_LY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L 13P Licensee Prior Var]", "Lic_13P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Licensee]", "Lic_TY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Licensee Prior]", "Lic_LY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Licensee Prior Var]", "Lic_13P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units 13P Licensee]", "Lic_TY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units 13P Licensee Prior]", "Lic_LY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units 13P Licensee Prior Var]", "Lic_13P_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L 3P Licensee]", "Lic_TY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L 3P Licensee Prior]", "Lic_LY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L 3P Licensee Prior Var]", "Lic_3P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Licensee]", "Lic_TY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Licensee Prior]", "Lic_LY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Licensee Prior Var]", "Lic_3P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units 3P Licensee]", "Lic_TY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units 3P Licensee Prior]", "Lic_LY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units 3P Licensee Prior Var]", "Lic_3P_UnitsPct", DbType.Double);

                //Execute Query
                List<SalesTeamReportBE> sales = connection.Query<SalesTeamReportBE>(query, parms).ToList();

                string queryTotal = DapperHelper.MdxQueryHelper.SalesSummaryLicenseeByTerritoryTotal;
                List<SalesTeamReportBE> salesTotal = connection.Query<SalesTeamReportBE>(queryTotal, parms).ToList();

                sales = sales.Concat(salesTotal).OrderBy(x => x.Tot_Rank_9LEquivalentCases).ToList();

                if (sales.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                List<SalesTeamReportBE> sortedsales = null; ;
                if (sortgroup == 0)
                {
                    sortedsales = sales.OrderBy(x => x.Tot_Rank_9LEquivalentCases).ToList();
                }
                else if (sortgroup == 1)
                {
                    sortedsales = sales.OrderBy(x => x.Tot_Rank_SalesAmount).ToList();
                }
                else
                {
                    sortedsales = sales.OrderBy(x => x.Tot_Rank_Units).ToList();
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return sortedsales;
            }
        }
        #endregion

        #region Sales Summary for All Accounts by Territory
        public List<SalesTeamReportBE> GetSalesSummaryAllByTerritoryCube(int userid, string periodkey, int sortgroup)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryAllByTerritory;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UserId", "[Sales Rep].[Sales Rep].&[" + userid + "]");

                //Obejct Row Mapping
                parms.Add("~0", "AccountNumber", DbType.String);
                parms.Add("~1", "AccountName", DbType.String);
                parms.Add("~2", "City", DbType.String);
                parms.Add("~3", "Address", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Rank Sales Amount]", "Rank_SalesAmount", DbType.Int16);
                parms.Add("~[Measures].[Rank Units]", "Rank_Units", DbType.Int16);
                parms.Add("~[Measures].[Rank 9L Eq Cases MAT]", "Rank_9LEquivalentCases", DbType.Int16);

                parms.Add("~[Measures].[9L Eq Cases MAT]", "All_TY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "All_LY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "All_13P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "All_TY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "All_LY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "All_13P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "All_TY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "All_LY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "All_13P_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases P3P]", "All_TY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P3P]", "All_LY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P3P Var %]", "All_3P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P3P]", "All_TY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P3P]", "All_LY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P3P Var %]", "All_3P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P3P]", "All_TY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P3P]", "All_LY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P3P Var %]", "All_3P_UnitsPct", DbType.Double);

                //Execute Query
                List<SalesTeamReportBE> sales = connection.Query<SalesTeamReportBE>(query, parms).ToList();

                string queryTotal = DapperHelper.MdxQueryHelper.SalesSummaryAllByTerritoryTotal;
                List<SalesTeamReportBE> salesTotal = connection.Query<SalesTeamReportBE>(queryTotal, parms).ToList();

                sales = sales.Concat(salesTotal).OrderBy(x => x.Rank_9LEquivalentCases).ToList();

                if (sales.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                List<SalesTeamReportBE> sortedsales = null; ;
                if (sortgroup == 0)
                {
                    sortedsales = sales.OrderBy(x => x.Rank_9LEquivalentCases).ToList();
                }
                else if (sortgroup == 1)
                {
                    sortedsales = sales.OrderBy(x => x.Rank_SalesAmount).ToList();
                }
                else
                {
                    sortedsales = sales.OrderBy(x => x.Rank_Units).ToList();
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return sortedsales;
            }
        }
        #endregion

        #region Product Sales for Store
        public List<SalesPersonalReportBE> GetSalesTeamStoreProductCube(int userid, string periodkey, string accountnumber, int clientonly)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesTeamStoreProduct;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UserId", "[Sales Rep].[Sales Rep].&[" + userid + "]");
                parms.Add("@AccountNumber", "[Account].[Account Number].[Account Number].&[" + accountnumber + "]");
                if (clientonly == 1)
                {
                    parms.Add("@ClientOnly", "[Product].[Is Own].&[1]");
                }
                else
                {
                    parms.Add("@ClientOnly", "[Product].[Is Own].&[0], [Product].[Is Market].&[1]");
                }

                //Obejct Row Mapping
                parms.Add("~0", "Code", DbType.String);
                parms.Add("~1", "ProductName", DbType.String);
                parms.Add("~2", "UnitSizeML", DbType.String);
                parms.Add("~3", "Category", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Listed/Delisted]", "IsListed", DbType.String);

                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_PromoTurn_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_PromoTurn_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "PromoTurn_9LCases_Pct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_PromoTurn_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_PromoTurn_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "PromoTurn_SalesAmount_Pct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_PromoTurn_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_PromoTurn_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "PromoTurn_Units_Pct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases P6P]", "Store_TY_6P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "Store_LY_6P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "Store_6P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "Store_TY_6P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "Store_LY_6P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "Store_6P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "Store_TY_6P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "Store_LY_6P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "Store_6P_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases MAT]", "Store_TY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "Store_LY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "Store_13P_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "Store_TY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "Store_LY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "Store_13P_SalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "Store_TY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "Store_LY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "Store_13P_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[Promo List]", "Promo", DbType.String);
                parms.Add("~[Measures].[Inventory]", "StoreInv", DbType.Int16);
                
                //Execute Query
                List<SalesPersonalReportBE> sales = connection.Query<SalesPersonalReportBE>(query, parms).ToList();

                if(sales != null)
                {
                    for (int i = 0; i < sales.Count; i++)
                        sales[i].Promo = string.Empty;
                }

                string queryTotal = DapperHelper.MdxQueryHelper.SalesTeamStoreProductTotal;
                List<SalesPersonalReportBE> salesTotal = connection.Query<SalesPersonalReportBE>(queryTotal, parms).ToList();


                sales = salesTotal.Concat(sales).ToList();

                if (sales.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                //get promotion code 

                query = DapperHelper.MdxQueryHelper.StoreSalesPromotionCode;

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@AccountNumber", "[Account].[Account Number].[Account Number].&[" + accountnumber + "]");
                //Obejct Row Mapping
                parms.Add("~0", "ProductNumber", DbType.String);
                parms.Add("~1", "PromotionCode", DbType.String);

                //Execute Query
                List<ProductPromotionCodeBE> promotionCodeData = connection.Query<ProductPromotionCodeBE>(query, parms).ToList();
                if (sales != null && promotionCodeData != null)
                {
                    for (var i = 0; i < sales.Count; i++)
                    {
                        for (var j = 0; j < promotionCodeData.Count; j++)
                        {
                            if (sales[i].Code == promotionCodeData[j].ProductNumber)
                            {
                                if (sales[i].Promo.Trim() == string.Empty || sales[i].Promo.Trim() == "0")
                                    sales[i].Promo = promotionCodeData[j].PromotionCode;
                                else
                                    sales[i].Promo = sales[i].Promo + "," + promotionCodeData[j].PromotionCode;
                            }
                        }
                    }
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");




                return sales;
            }
        }
        #endregion

        #region Category Sales for Store
        public List<SalesPersonalReportBE> GetSalesTeamStoreCategoryCube (int userid, string periodkey, string accountnumber, int clientonly)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesCategoryStore;
                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@AccountNumber", "[Account].[Account Number].&[" + accountnumber + "]");
                if (clientonly == 1)
                {
                    parms.Add("@ClientOnly", "[Product].[Is Own].&[1]");
                } else
                {
                    parms.Add("@ClientOnly", "[Product].[Is Own].&[0], [Product].[Is Market].&[1]");
                }
                

                //Obejct Row Mapping
                parms.Add("~0", "SetName", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Rank]", "xRank", DbType.Int16);

                parms.Add("~[Measures].[SalesAmount_PromoTurn_TY]", "SalesAmount_PromoTurn_TY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_PromoTurn_LY]", "SalesAmount_PromoTurn_LY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_PromoTurn_Pct]", "SalesAmount_PromoTurn_Pct", DbType.Double);
                
                parms.Add("~[Measures].[SalesAmount_6P_TY]", "SalesAmount_6P_TY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_6P_LY]", "SalesAmount_6P_LY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_6P_Pct]", "SalesAmount_6P_Pct", DbType.Double);

                parms.Add("~[Measures].[SalesAmount_13P_TY]", "SalesAmount_13P_TY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_13P_LY]", "SalesAmount_13P_LY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_13P_Pct]", "SalesAmount_13P_Pct", DbType.Double);

                parms.Add("~[Measures].[Promo List]", "Promo", DbType.String);
                parms.Add("~[Measures].[Inventory]", "StoreInv", DbType.Int16);
                
                //Execute Query
                List<SalesPersonalReportBE> category = connection.Query<SalesPersonalReportBE>(query, parms).ToList();
                
                if (category.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
              

                return category;
            }
        }
        #endregion

        #region Product Sales Not In Store
        public List<SalesPersonalReportBE> GetSalesTeamProductsNotInStoreCube(int userid, string periodkey, string accountnumber)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.ProductsNotInStoreLicensee;
                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@AccountNumber", "[Account].[Account Number].&[" + accountnumber + "]");
                
                //Obejct Row Mapping
                parms.Add("~1", "ProductName", DbType.String);
                parms.Add("~2", "ProductNumber", DbType.String);

                //Object Column Mapping
                parms.Add("~3", "London", DbType.Double);
                parms.Add("~4", "Mississauga", DbType.Double);
                parms.Add("~5", "Ottawa", DbType.Double);
                parms.Add("~6", "ThunderBay", DbType.Double);
                parms.Add("~7", "Toronto", DbType.Double);
                parms.Add("~8", "TorontoToronto", DbType.Double);
                parms.Add("~9", "Whitby", DbType.Double);

                //Execute Query
                List<SalesPersonalReportBE> category = connection.Query<SalesPersonalReportBE>(query, parms).ToList();

                if (category.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return category;
            }
        }
        #endregion

        #region Sales Category Details for Store

        public List<SalesPersonalReportBE> GetSalesCategoryDetails(string periodkey, string accountnumber, string category, int isclientonly)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesCategoryDetailStore;
                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@Category", "[Product].[Category].&[" + category + "]");

                if (isclientonly == 1)
                {
                    parms.Add("@ClientOnly", "[Product].[Is Own].&[1]");
                }
                else
                {
                    parms.Add("@ClientOnly", "[Product].[Is Own].&[0], [Product].[Is Market].&[1]");
                }
                parms.Add("@AccountNumber", "[Account].[Account Number].[Account Number].&[" + accountnumber + "]");

                //Obejct Row Mapping
                parms.Add("~0", "Code", DbType.String);
                parms.Add("~1", "ProductName", DbType.String);
                parms.Add("~2", "UnitSizeML", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[SalesAmount_PromoTurn_TY]", "SalesAmount_PromoTurn_TY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_PromoTurn_LY]", "SalesAmount_PromoTurn_LY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_PromoTurn_Pct]", "SalesAmount_PromoTurn_Pct", DbType.Double);

                parms.Add("~[Measures].[SalesAmount_6P_TY]", "SalesAmount_6P_TY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_6P_LY]", "SalesAmount_6P_LY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_6P_Pct]", "SalesAmount_6P_Pct", DbType.Double);

                parms.Add("~[Measures].[SalesAmount_13P_TY]", "SalesAmount_13P_TY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_13P_LY]", "SalesAmount_13P_LY", DbType.Double);
                parms.Add("~[Measures].[SalesAmount_13P_Pct]", "SalesAmount_13P_Pct", DbType.Double);

                parms.Add("~[Measures].[Promo List]", "Promo", DbType.String);
                parms.Add("~[Measures].[Inventory]", "StoreInv", DbType.Int32);

                //Execute Query
                List<SalesPersonalReportBE> output = connection.Query<SalesPersonalReportBE>(query, parms).ToList();
                if (output != null)
                {
                    for (int i = 0; i < output.Count; i++)
                        output[i].Promo = string.Empty;
                }

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                //get promotion code 

                query = DapperHelper.MdxQueryHelper.StoreSalesPromotionCode;

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@AccountNumber", "[Account].[Account Number].[Account Number].&[" + accountnumber + "]");
                //Obejct Row Mapping
                parms.Add("~0", "ProductNumber", DbType.String);
                parms.Add("~1", "PromotionCode", DbType.String);

                //Execute Query
                List<ProductPromotionCodeBE> promotionCodeData = connection.Query<ProductPromotionCodeBE>(query, parms).ToList();
                if (output != null && promotionCodeData != null)
                {
                    for (var i = 0; i < output.Count; i++)
                    {
                        for (var j = 0; j < promotionCodeData.Count; j++)
                        {
                            if (output[i].Code == promotionCodeData[j].ProductNumber)
                            {
                                if (output[i].Promo.Trim() == string.Empty || output[i].Promo.Trim() == "0")
                                    output[i].Promo = promotionCodeData[j].PromotionCode;
                                else
                                    output[i].Promo = output[i].Promo + "," + promotionCodeData[j].PromotionCode;
                            }
                        }
                    }
                }
                return output;
            }
        }
        #endregion

        #region Licensee Details

        public List<LicenseeDetailsBE> GetLicenseeDetails(string periodkey, string accountname)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.LicenseeDetails;
                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@AccountName", "[Account].[Account Name].&[" + accountname + "]");

                //Obejct Row Mapping
                parms.Add("~0", "MyCategory", DbType.String);
                parms.Add("~1", "CSPC", DbType.String);
                parms.Add("~2", "ProductName", DbType.String);
                parms.Add("~3", "UnitSizeML", DbType.String);
                parms.Add("~4", "Price", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[LCBO Sales CP TY]", "LCBO_Sales_CP", DbType.Double);
                parms.Add("~[Measures].[Direct Sales CP TY]", "Direct_Sales_CP", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 1P TY]", "LCBO_Sales_1P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 1P TY]", "Direct_Sales_1P", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 2P TY]", "LCBO_Sales_2P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 2P TY]", "Direct_Sales_2P", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 3P TY]", "LCBO_Sales_3P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 3P TY]", "Direct_Sales_3P", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 4P TY]", "LCBO_Sales_4P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 4P TY]", "Direct_Sales_4P", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 5P TY]", "LCBO_Sales_5P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 5P TY]", "Direct_Sales_5P", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 6P TY]", "LCBO_Sales_6P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 6P TY]", "Direct_Sales_6P", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 7P TY]", "LCBO_Sales_7P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 7P TY]", "Direct_Sales_7P", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 8P TY]", "LCBO_Sales_8P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 8P TY]", "Direct_Sales_8P", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 9P TY]", "LCBO_Sales_9P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 9P TY]", "Direct_Sales_9P", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 10P TY]", "LCBO_Sales_10P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 10P TY]", "Direct_Sales_10P", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 11P TY]", "LCBO_Sales_11P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 11P TY]", "Direct_Sales_11P", DbType.Double);
                parms.Add("~[Measures].[LCBO Sales 12P TY]", "LCBO_Sales_12P", DbType.Double);
                parms.Add("~[Measures].[Direct Sales 12P TY]", "Direct_Sales_12P", DbType.Double);

                //Execute Query
                List<LicenseeDetailsBE> licenseedetails = connection.Query<LicenseeDetailsBE>(query, parms).ToList();

                if (licenseedetails.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return licenseedetails;
            }
        }
        #endregion

        #region Search

        public List<SearchBE> GetSearch()
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.Search;

                DynamicParameters parms = new DynamicParameters();

                //Object Column Mapping
                //parms.Add("~[Measures].[Terr No]", "TerrNo", DbType.Int16);
                //parms.Add("~[Sales Rep].[Sales Rep].[Sales Rep]", "SalesRep", DbType.String);
                parms.Add("~[Account].[Account Name].[Account Name]", "StoreName", DbType.String);
                parms.Add("~[Account].[City].[City]", "City", DbType.String);
                parms.Add("~[Account].[Address].[Address]", "Address", DbType.String);
                parms.Add("~[Account].[Postal Code].[Postal Code]", "PostalCode", DbType.String);
                parms.Add("~[Account].[Region].[Region]", "Region", DbType.String);
                parms.Add("~[Account].[Account Number].[Account Number]", "StoreNumber", DbType.String);

                //Execute Query
                List<SearchBE> output = connection.Query<SearchBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }
        #endregion

        #region LicSearch

        public List<LicSearchBE> GetLicSearch()
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.LicSearch;

                DynamicParameters parms = new DynamicParameters();

                //Object Column Mapping
                //parms.Add("~[Measures].[Terr No]", "TerrNo", DbType.Int16);
                //parms.Add("~[Sales Rep].[Sales Rep].[Sales Rep]", "SalesRep", DbType.String);
                parms.Add("~[Account].[Account Name].[Account Name]", "StoreName", DbType.String);
                parms.Add("~[Account].[City].[City]", "City", DbType.String);
                parms.Add("~[Account].[Address].[Address]", "Address", DbType.String);
                parms.Add("~[Account].[Postal Code].[Postal Code]", "PostalCode", DbType.String);
                parms.Add("~[Account].[Region].[Region]", "Region", DbType.String);
                parms.Add("~[Account].[Account Number].[Account Number]", "StoreNumber", DbType.String);

                //Execute Query
                List<LicSearchBE> output = connection.Query<LicSearchBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }
        #endregion

        #region CSPCSearch
        public List<CSPCSearchBE> GetCSPCSearch()
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.CSPCSearch;

                DynamicParameters parms = new DynamicParameters();

                //Object Column Mapping
                parms.Add("~[Product].[CSPC].[CSPC]", "CSPC", DbType.String);
                parms.Add("~[Product].[Product Name].[Product Name]", "ProductName", DbType.String);
                parms.Add("~[Product].[Category].[Category]", "Category", DbType.String);
                parms.Add("~[Product].[Subcategory].[Subcategory]", "Subcategory", DbType.String);
                parms.Add("~[Product].[Brand].[Brand]", "Brand", DbType.String);
                //Execute Query
                List<CSPCSearchBE> output = connection.Query<CSPCSearchBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }
        #endregion

        #region CSPC Store From CSPCSearch

        public List<CSPCStoreLicenseeBE> GetCSPCStoreCube(string periodkey, string cspc, int groupid)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.CSPCStore;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@CSPC", "[Product].[CSPC].[CSPC].&[" + cspc + "]");

                //Obejct Row Mapping
                parms.Add("~0", "Code", DbType.String);
                parms.Add("~1", "AccountName", DbType.String);
                parms.Add("~2", "City", DbType.String);
                parms.Add("~3", "Address", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Rank Sales Amount]", "RankSalesAmount", DbType.Int16);
                parms.Add("~[Measures].[Rank Units]", "RankUnits", DbType.Int16);
                parms.Add("~[Measures].[Rank 9L Eq Cases MAT]", "Rank9LCases", DbType.Int16);

                parms.Add("~[Measures].[9L Eq Cases MAT]", "TY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "LY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "Pct_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "TY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "LY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "Pct_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "TY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "LY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "Pct_13P_Units", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases P3P]", "TY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P3P]", "LY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P3P Var %]", "Pct_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P3P]", "TY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P3P]", "LY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P3P Var %]", "Pct_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Units P3P]", "TY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P3P]", "LY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P3P Var %]", "Pct_3P_Units", DbType.Double);

                parms.Add("~[Measures].[9L 13P Direct Sales]", "TY_13P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[9L 13P Direct Sales Prior]", "LY_13P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[9L 13P Direct Sales Prior Var]", "Pct_13P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Direct Sales]", "TY_13P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Direct Sales Prior]", "LY_13P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Direct Sales Prior Var]", "Pct_13P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 13P Direct Sales]", "TY_13P_Units_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 13P Direct Sales Prior]", "LY_13P_Units_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 13P Direct Sales Prior Var]", "Pct_13P_Units_DirectSales", DbType.Double);

                parms.Add("~[Measures].[9L 3P Direct Sales]", "TY_3P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[9L 3P Direct Sales Prior]", "LY_3P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[9L 3P Direct Sales Prior Var]", "Pct_3P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Direct Sales]", "TY_3P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Direct Sales Prior]", "LY_3P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Direct Sales Prior Var]", "Pct_3P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 3P Direct Sales]", "TY_3P_Units_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 3P Direct Sales Prior]", "LY_3P_Units_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 3P Direct Sales Prior Var]", "Pct_3P_Units_DirectSales", DbType.Double);

                parms.Add("~[Measures].[9L 13P Licensee]", "TY_13P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[9L 13P Licensee Prior]", "LY_13P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[9L 13P Licensee Prior Var]", "Pct_13P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Licensee]", "TY_13P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Licensee Prior]", "LY_13P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Licensee Prior Var]", "Pct_13P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 13P Licensee]", "TY_13P_Units_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 13P Licensee Prior]", "LY_13P_Units_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 13P Licensee Prior Var]", "Pct_13P_Units_Licensee", DbType.Double);

                parms.Add("~[Measures].[9L 3P Licensee]", "TY_3P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[9L 3P Licensee Prior]", "LY_3P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[9L 3P Licensee Prior Var]", "Pct_3P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Licensee]", "TY_3P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Licensee Prior]", "LY_3P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Licensee Prior Var]", "Pct_3P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 3P Licensee]", "TY_3P_Units_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 3P Licensee Prior]", "LY_3P_Units_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 3P Licensee Prior Var]", "Pct_3P_Units_Licensee", DbType.Double);

                //Execute Query
                List<CSPCStoreLicenseeBE> sales = connection.Query<CSPCStoreLicenseeBE>(query, parms).ToList();
                
                if (groupid == 0)
                {
                    sales = sales.OrderBy(x => x.Rank9LCases).ToList();
                } else if (groupid == 1)
                {
                    sales = sales.OrderBy(x => x.RankSalesAmount).ToList();
                } else
                {
                    sales = sales.OrderBy(x => x.RankUnits).ToList();
                }

                if (sales.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return sales;
            }
        }
        #endregion

        #region CSPC Licensee From CSPCSearch

        public List<CSPCStoreLicenseeBE> GetCSPCLicenseeCube(string periodkey, string cspc, int groupid)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.CSPCLicensee;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@CSPC", "[Product].[CSPC].[CSPC].&[" + cspc + "]");

                //Obejct Row Mapping
                parms.Add("~0", "Code", DbType.String);
                parms.Add("~1", "AccountName", DbType.String);
                parms.Add("~2", "City", DbType.String);
                parms.Add("~3", "Address", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Rank Sales Amount]", "RankSalesAmount", DbType.Int16);
                parms.Add("~[Measures].[Rank Units]", "RankUnits", DbType.Int16);
                parms.Add("~[Measures].[Rank 9L Eq Cases MAT]", "Rank9LCases", DbType.Int16);

                parms.Add("~[Measures].[9L Eq Cases MAT]", "TY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT]", "LY_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior MAT Var %]", "Pct_13P_9LCases", DbType.Double);
                parms.Add("~[Measures].[Sales Amount MAT]", "TY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT]", "LY_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior MAT Var %]", "Pct_13P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Units MAT]", "TY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT]", "LY_13P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior MAT Var %]", "Pct_13P_Units", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases P3P]", "TY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P3P]", "LY_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P3P Var %]", "Pct_3P_9LCases", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P3P]", "TY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P3P]", "LY_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P3P Var %]", "Pct_3P_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Units P3P]", "TY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P3P]", "LY_3P_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P3P Var %]", "Pct_3P_Units", DbType.Double);

                parms.Add("~[Measures].[9L 13P Direct Sales]", "TY_13P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[9L 13P Direct Sales Prior]", "LY_13P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[9L 13P Direct Sales Prior Var]", "Pct_13P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Direct Sales]", "TY_13P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Direct Sales Prior]", "LY_13P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Direct Sales Prior Var]", "Pct_13P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 13P Direct Sales]", "TY_13P_Units_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 13P Direct Sales Prior]", "LY_13P_Units_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 13P Direct Sales Prior Var]", "Pct_13P_Units_DirectSales", DbType.Double);

                parms.Add("~[Measures].[9L 3P Direct Sales]", "TY_3P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[9L 3P Direct Sales Prior]", "LY_3P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[9L 3P Direct Sales Prior Var]", "Pct_3P_9LCases_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Direct Sales]", "TY_3P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Direct Sales Prior]", "LY_3P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Direct Sales Prior Var]", "Pct_3P_SalesAmount_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 3P Direct Sales]", "TY_3P_Units_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 3P Direct Sales Prior]", "LY_3P_Units_DirectSales", DbType.Double);
                parms.Add("~[Measures].[Units 3P Direct Sales Prior Var]", "Pct_3P_Units_DirectSales", DbType.Double);

                parms.Add("~[Measures].[9L 13P Licensee]", "TY_13P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[9L 13P Licensee Prior]", "LY_13P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[9L 13P Licensee Prior Var]", "Pct_13P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Licensee]", "TY_13P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Licensee Prior]", "LY_13P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 13P Licensee Prior Var]", "Pct_13P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 13P Licensee]", "TY_13P_Units_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 13P Licensee Prior]", "LY_13P_Units_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 13P Licensee Prior Var]", "Pct_13P_Units_Licensee", DbType.Double);

                parms.Add("~[Measures].[9L 3P Licensee]", "TY_3P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[9L 3P Licensee Prior]", "LY_3P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[9L 3P Licensee Prior Var]", "Pct_3P_9LCases_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Licensee]", "TY_3P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Licensee Prior]", "LY_3P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Sales 3P Licensee Prior Var]", "Pct_3P_SalesAmount_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 3P Licensee]", "TY_3P_Units_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 3P Licensee Prior]", "LY_3P_Units_Licensee", DbType.Double);
                parms.Add("~[Measures].[Units 3P Licensee Prior Var]", "Pct_3P_Units_Licensee", DbType.Double);

                //Execute Query
                List<CSPCStoreLicenseeBE> sales = connection.Query<CSPCStoreLicenseeBE>(query, parms).ToList();
                
                if (groupid == 0)
                {
                    sales = sales.OrderBy(x => x.Rank9LCases).ToList();
                }
                else if (groupid == 1)
                {
                    sales = sales.OrderBy(x => x.RankSalesAmount).ToList();
                }
                else
                {
                    sales = sales.OrderBy(x => x.RankUnits).ToList();
                }

                if (sales.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return sales;
            }
        }
        #endregion

        #region Sales Summary by Color
        public List<SalesSummarySegmentBE> GetSalesSummaryByColorCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryByColor(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "Color", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);              
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryByWeek(1, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Segment", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekBE> weekData = connection.Query<WeekBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++) {
                    for (var j = 0; j < weekData.Count; ++j)
                    if (output[i].Color == weekData[j].Segment)
                    {
                        output[i].Week1 = weekData[j].Week1;
                        output[i].Week2 = weekData[j].Week2;
                        output[i].Week3 = weekData[j].Week3;
                        output[i].Week4 = weekData[j].Week4;
                    }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by Country
        public List<SalesSummarySegmentBE> GetSalesSummaryByCountryCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryByCountry(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "Country", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryByWeek(2, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Segment", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekBE> weekData = connection.Query<WeekBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].Country == weekData[j].Segment)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by Varietal
        public List<SalesSummarySegmentBE> GetSalesSummaryByVarietalCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryByVarietal(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "Varietal", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryByWeek(3, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Segment", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekBE> weekData = connection.Query<WeekBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].Varietal == weekData[j].Segment)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by My Category
        public List<SalesSummarySegmentBE> GetSalesSummaryByMyCategoryCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryByMyCategory(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "MyCategory", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryByWeek(4, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Segment", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekBE> weekData = connection.Query<WeekBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].MyCategory == weekData[j].Segment)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by Price Band
        public List<SalesSummarySegmentBE> GetSalesSummaryByPriceBandCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryByPriceBand(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "PriceBand", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryByWeek(5, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Segment", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekBE> weekData = connection.Query<WeekBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].PriceBand == weekData[j].Segment)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by Color Country
        public List<SalesSummarySegmentBE> GetSalesSummaryByColorCountryDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string colorname, string category)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryColorCountry(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@ColorName", " [Product].[Master Color].&[" + colorname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "Country", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryColorCountryByWeek(colorname, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Color", " [Product].[Master Color].&[" + colorname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Country", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekSalesSummaryBE> weekData = connection.Query<WeekSalesSummaryBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].Country == weekData[j].Country)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by Color Country Product
        public List<SalesSummarySegmentBE> GetSalesSummaryByColorCountryProductDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string colorname, string countryname, string category)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryColorCountryProduct(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@ColorName", " [Product].[Master Color].&[" + colorname + "]");
                parms.Add("@CountryName", " [Product].[Master Country].&[" + countryname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "Product", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryColorCountryProductByWeek(colorname, countryname, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Color", " [Product].[Master Color].&[" + colorname + "]");
                parms.Add("@Country", " [Product].[Master Country].&[" + countryname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Product", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekSalesSummaryBE> weekData = connection.Query<WeekSalesSummaryBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].Product == weekData[j].Product)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by Color Country
        public List<SalesSummarySegmentBE> GetSalesSummaryByCountryColorDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string countryname, string category)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryCountryColor(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@CountryName", " [Product].[Master Country].&[" + countryname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "Color", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryCountryColorByWeek(countryname, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Country", " [Product].[Master Country].&[" + countryname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Color", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekSalesSummaryBE> weekData = connection.Query<WeekSalesSummaryBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].Color == weekData[j].Color)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by Color Country Product
        public List<SalesSummarySegmentBE> GetSalesSummaryByCountryColorProductDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string countryname, string colorname, string category)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryColorCountryProduct(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@ColorName", " [Product].[Master Color].&[" + colorname + "]");
                parms.Add("@CountryName", " [Product].[Master Country].&[" + countryname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "Product", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryCountryColorProductByWeek(countryname, colorname, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Color", " [Product].[Master Color].&[" + colorname + "]");
                parms.Add("@Country", " [Product].[Master Country].&[" + countryname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Product", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekSalesSummaryBE> weekData = connection.Query<WeekSalesSummaryBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].Product == weekData[j].Product)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by Varietal Product
        public List<SalesSummarySegmentBE> GetSalesSummaryByVarietalProductDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string varietal, string category)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryVarietalProduct(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@Varietal", " [Product].[Master Varietal].&[" + varietal + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "Product", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryVarietalProductByWeek(varietal, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@Varietal", " [Product].[Master Varietal].&[" + varietal + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Product", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekSalesSummaryBE> weekData = connection.Query<WeekSalesSummaryBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].Product == weekData[j].Product)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by My Category Country
        public List<SalesSummarySegmentBE> GetSalesSummaryByMyCategoryCountryDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string mycategoryname, string category)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryMyCategoryCountry(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@MyCategoryName", " [Product].[My Category].&[" + mycategoryname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "Country", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryMyCategoryCountryByWeek(mycategoryname, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@MyCategory", " [Product].[My Category].&[" + mycategoryname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Country", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekSalesSummaryBE> weekData = connection.Query<WeekSalesSummaryBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].Country == weekData[j].Country)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by Color Country Product
        public List<SalesSummarySegmentBE> GetSalesSummaryByMyCategoryCountryProductDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string mycategoryname, string countryname, string category)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryMyCategoryCountryProduct(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@MyCategoryName", " [Product].[My Category].&[" + mycategoryname + "]");
                parms.Add("@CountryName", " [Product].[Master Country].&[" + countryname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "Product", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryMyCategoryCountryProductByWeek(mycategoryname, countryname, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                parms.Add("@MyCategory", " [Product].[My Category].&[" + mycategoryname + "]");
                parms.Add("@Country", " [Product].[Master Country].&[" + countryname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Product", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekSalesSummaryBE> weekData = connection.Query<WeekSalesSummaryBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].Product == weekData[j].Product)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by Price Band Category
        public List<SalesSummarySegmentBE> GetSalesSummaryByPriceBandCategoryDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string pricebandname, string category)
        {
            string pricebandascii = PriceBandToAscii(pricebandname);
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryPriceBandCategory(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");

                if (pricebandname != "Unknown")
                    parms.Add("@PriceBandName", " [Product Price Band].[Product Price Band].&[" + pricebandascii + "]");
                else
                    parms.Add("@PriceBandName", " [Product Price Band].[Product Price Band].[All].UNKNOWNMEMBER");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "MyCategory", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryPriceBandCategoryByWeek(pricebandname, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                if (pricebandname != "Unknown")
                    parms.Add("@PriceBand", " [Product Price Band].[Product Price Band].&[" + pricebandascii + "]");
                else
                    parms.Add("@PriceBand", " [Product Price Band].[Product Price Band].[All].UNKNOWNMEMBER");

                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "MyCategory", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekSalesSummaryBE> weekData = connection.Query<WeekSalesSummaryBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].MyCategory == weekData[j].MyCategory)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Sales Summary by PriceBand MyCategory Product
        public List<SalesSummarySegmentBE> GetSalesSummaryByPriceBandMyCategoryProductDetailCube(string periodkey, string setnames, string unitsizes, decimal pricefrom, decimal priceto, int groupid, string pricebandname, string mycategoryname, string category)
        {
            string pricebandascii = PriceBandToAscii(pricebandname);
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SalesSummaryPriceBandMyCategoryProduct(pricefrom, priceto);

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Load Input Parameter(s)
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                if (pricebandname != "Unknown")
                    parms.Add("@PriceBandName", " [Product Price Band].[Product Price Band].&[" + pricebandascii + "]");
                else
                    parms.Add("@PriceBandName", " [Product Price Band].[Product Price Band].[All].UNKNOWNMEMBER");

                parms.Add("@MyCategory", " [Product].[My Category].&[" + mycategoryname + "]");
               
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));

                //Obejct Row Mapping
                parms.Add("~0", "Product", DbType.String);
                parms.Add("~[Measures].[9L Eq Cases CP LY]", "LY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP TY]", "TY_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases CP LY Var %]", "TY_9LCasesPct", DbType.Double);

                //Object Column Mapping
                //parms.Add("~[Measures].[xRank]", "xRank", DbType.Int16);
                parms.Add("~[Measures].[Sales Amount CP TY]", "TY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY]", "LY_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount CP LY Var %]", "TY_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units CP TY]", "TY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY]", "LY_Units", DbType.Double);
                parms.Add("~[Measures].[Units CP LY Var %]", "TY_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases P6P]", "TY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P]", "LY6m_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior P6P Var %]", "TY6m_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount P6P]", "TY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P]", "LY6m_TotalSalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior P6P Var %]", "TY6m_TotalSalesAmountPct", DbType.Double);
                parms.Add("~[Measures].[Units P6P]", "TY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P]", "LY6m_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior P6P Var %]", "TY6m_UnitsPct", DbType.Double);

                parms.Add("~[Measures].[9L Eq Cases Prior YTD]", "LYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases YTD]", "TYYTD_9LCases", DbType.Double);
                parms.Add("~[Measures].[9L Eq Cases Prior YTD Var %]", "TYYTD_9LCasesPct", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD]", "LYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units YTD]", "TYYTD_Units", DbType.Double);
                parms.Add("~[Measures].[Units Prior YTD Var %]", "TYYTD_UnitsPct", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD]", "LYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount YTD]", "TYYTD_SalesAmount", DbType.Double);
                parms.Add("~[Measures].[Sales Amount Prior YTD Var %]", "TYYTD_SalesAmountPct", DbType.Double);

                parms.Add("~[Measures].[On Prem 13 Per LY]", "LY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Per TY]", "TY13_OnPrem", DbType.Double);
                parms.Add("~[Measures].[On Prem 13 Var %]", "TY13_OnPremPct", DbType.Double);

                parms.Add("~[Measures].[Rol Year Per TY$]", "TY13Amount", DbType.Double);
                parms.Add("~[Measures].[Rol Year $ Var %]", "TY13AmountPct", DbType.Double);

                //Execute Query
                List<SalesSummarySegmentBE> output = connection.Query<SalesSummarySegmentBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");

                // Getting Weekly Data
                query = DapperHelper.MdxQueryHelper.SalesSummaryPriceBandMyCategoryProductByWeek(pricebandname, mycategoryname, groupid);

                //Create Dynamic Parameters
                parms = new DynamicParameters();

                //Load Input Parameter(s)
                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@UnitSize", DapperHelper.DapperHelper.StringToMdxMember(unitsizes, "[Product].[Volume Per Unit ML]"));
                if (pricebandname != "Unknown")
                    parms.Add("@PriceBand", " [Product Price Band].[Product Price Band].&[" + pricebandascii + "]");
                else
                    parms.Add("@PriceBand", " [Product Price Band].[Product Price Band].[All].UNKNOWNMEMBER");
                parms.Add("@MyCategory", " [Product].[My Category].&[" + mycategoryname + "]");
                parms.Add("@Category", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "Product", DbType.String);
                parms.Add("~1", "Week1", DbType.Double);
                parms.Add("~2", "Week2", DbType.Double);
                parms.Add("~3", "Week3", DbType.Double);
                parms.Add("~4", "Week4", DbType.Double);

                //Execute Query
                List<WeekSalesSummaryBE> weekData = connection.Query<WeekSalesSummaryBE>(query, parms).ToList();

                for (var i = 0; i < output.Count; i++)
                {
                    for (var j = 0; j < weekData.Count; ++j)
                        if (output[i].Product == weekData[j].Product)
                        {
                            output[i].Week1 = weekData[j].Week1;
                            output[i].Week2 = weekData[j].Week2;
                            output[i].Week3 = weekData[j].Week3;
                            output[i].Week4 = weekData[j].Week4;
                        }
                }

                return output;
            }
        }
        #endregion

        #region Helper Function
        private string PriceBandToAscii(string priceband)
        {
            string pricebandascii = string.Empty;

            switch (priceband)
            {
                case "Unassigned":
                    pricebandascii = "-1";
                    break;

                case "Unknown":
                    pricebandascii = "UNKNOWNMEMBER";
                    break;

                default:
                    byte[] charByte = Encoding.ASCII.GetBytes(priceband);
                    pricebandascii = ((int)charByte[0] - 18).ToString();
                    break;
            }

            return pricebandascii;
        }
        #endregion
    }
}
