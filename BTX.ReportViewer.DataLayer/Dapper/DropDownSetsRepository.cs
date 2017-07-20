using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTX.ReportViewer.MdxClient;
using BTX.ReportViewer.Logging;

namespace BTX.ReportViewer.DataLayer.Dapper
{
    public class DropDownSetsRepository : IDropDownSetsRepository
    {
        private IDbConnection db_pelham = new SqlConnection(ConfigurationManager.ConnectionStrings["SWIG_DW"].ConnectionString);
        private int _client = int.Parse(ConfigurationManager.AppSettings["CompanyClientId"]);
        public static string GetSSASConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SWIG_DW_SSAS"].ConnectionString;
        }
        public static MdxConnection GetSSASConnection()
        {
            LoggingHelper.LogInformation(String.Format("Connecting to Analysis Server: {0}", GetSSASConnectionString()));
            return new MdxConnection(GetSSASConnectionString());
        }

        public List<DropDownSetBE> GetAllSetNames()
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.SetNames;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Obejct Row Mapping
                parms.Add("~0", "id", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Category Code]", "label", DbType.String);

                //Execute Query
                List<DropDownSetBE> output = connection.Query<DropDownSetBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }

        public List<DropDownSetBE> GetAllUnitSizeMLs(string periodkey, string setnames)
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.UnitSizes;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Add Parameters
                string[] stringoperator = new string[] { "-P" };
                string[] result = periodkey.Split(stringoperator, StringSplitOptions.None);

                parms.Add("@Period", "[Date].[Promotional Period].&[" + result[0] + "]&[" + result[1] + "]");
                parms.Add("@SetNames", DapperHelper.DapperHelper.StringToMdxMember(setnames, "[Product].[Category]"));

                //Obejct Row Mapping
                parms.Add("~0", "label", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[Unit Code]", "id", DbType.String);

                //Execute Query
                List<DropDownSetBE> output = connection.Query<DropDownSetBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }

        public List<DropDownSetBE> GetAllPeriodKeys()
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.PeriodKeys;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();
                
                //Obejct Row Mapping
                parms.Add("~0", "id", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[ParameterCaption]", "label", DbType.String);

                //Execute Query
                List<DropDownSetBE> output = connection.Query<DropDownSetBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }

        public List<DropDownSetBE> GetMyCategories()
        {
            var connection = GetSSASConnection();
            using (connection)
            {
                connection.Open();
                string query = DapperHelper.MdxQueryHelper.MyCategory;

                //Create Dynamic Parameters
                DynamicParameters parms = new DynamicParameters();

                //Obejct Row Mapping
                parms.Add("~0", "id", DbType.String);

                //Object Column Mapping
                parms.Add("~[Measures].[ParameterCaption]", "label", DbType.String);

                //Execute Query
                List<DropDownSetBE> output = connection.Query<DropDownSetBE>(query, parms).ToList();

                if (output.Count == 0)
                {
                    LoggingHelper.LogInformation("No result returned from mdx query.");
                }

                LoggingHelper.LogInformation("Returning result from mdx query...");
                return output;
            }
        }
    }
}
