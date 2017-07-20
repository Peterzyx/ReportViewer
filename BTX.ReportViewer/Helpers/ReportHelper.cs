using BTX.ReportViewer.EntityModel;
using BTX.ReportViewer.ReportService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;



namespace BTX.ReportViewer.Helpers
{

    public class ReportHelper
    {
        #region CONSTANTS
        private const string FOLDER = "Folder";
        private const string REPORT = "Report";
        private const string REPORTS = "Reports";
        private const string MAINFOLDER = "Reports";
        #endregion

        private string _reportImage = ConfigurationManager.AppSettings["DefaultReportImage"];
        private string _reportImageFolder = ConfigurationManager.AppSettings["DefaultReportImageFolder"];


        public Dictionary<string, List<ReportBE>> GroupReportsByDirectory(List<CatalogItem> ciList)
        {

            Dictionary<string, List<ReportBE>> data = new Dictionary<string, List<ReportBE>>();

            string reportGroupName = string.Empty;
            string folderPath = string.Empty;
            string currentFolder = string.Empty;

            List<CatalogItem> reports = new List<CatalogItem>();

            #region Group Reports by Directory Name
            foreach (CatalogItem ci in ciList)
            {
                if (ci.TypeName.ToString().Equals(FOLDER))
                {
                    reportGroupName = ci.Name;
                }

                if (!String.IsNullOrEmpty(reportGroupName) && data.ContainsKey(reportGroupName))
                {
                    if (ci.TypeName.ToString().Equals(REPORT))
                    {
                        ReportBE report = new ReportBE();
                        report.ReportName = ci.Name;
                        report.ReportDescription = ci.Description;
                        report.ReportGroup = reportGroupName;
                        report.ReportImage = "";
                        report.ReportImageURL = GetReportImage(ci.Path);
                        report.ReportPath = ci.Path;
                        report.ReportType = ci.TypeName;

                        data[reportGroupName].Add(report);
                    }
                }
                else
                {
                    List<ReportBE> rpts = new List<ReportBE>();
                    if (ci.TypeName.ToString().Equals(REPORT))
                    {
                        ReportBE report = new ReportBE();
                        report.ReportName = ci.Name;
                        report.ReportDescription = ci.Description;
                        report.ReportGroup = reportGroupName;
                        report.ReportImage = "";
                        report.ReportImageURL = GetReportImage(ci.Path);
                        report.ReportPath = ci.Path;

                        rpts.Add(report);
                        if (string.IsNullOrEmpty(reportGroupName))
                            reportGroupName = REPORTS;
                        data.Add(reportGroupName, rpts);
                    }
                }
            }
            #endregion


            return data;

        }

        private string GetReportImage(string reportPath)
        {
            string reportImagePath = Path.Combine(_reportImageFolder, _reportImage).Replace("\\", "/");
            if (IsDefaultReportImage(reportPath))
            {
                reportImagePath = Path.Combine(_reportImageFolder, reportPath.TrimStart('/'), _reportImage).Replace("\\", "/");
            }

            return reportImagePath;
        }

        private bool IsDefaultReportImage(string folderPath)
        {
            bool isDefaultReportImage = false;
            string reportImage = string.Empty;
            string reportImageFullpath = string.Empty;
            string dir = string.Empty;
            dir = _reportImageFolder + folderPath;
            dir = HttpContext.Current.Server.MapPath(dir);

            reportImageFullpath = Path.Combine(dir, _reportImage);

            // Check for directory path. Create if doesn't exist (new Report)
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            // Check for custom report Image.
            if (File.Exists(reportImageFullpath))
            {
                isDefaultReportImage = true;
            }

            return isDefaultReportImage;

        }

        #region Report Hierarchy Tree
        /// <summary>
        /// Adds a child node based on the catalog item's path
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public Node AddChild(Node parent, CatalogItem catalog, HttpContextBase httpContext)
        {
            //the first string of the path is the Home Folder (Henry Pelham) - changed to Home
            var paths = catalog.Path.Trim('/').Split('/').ToArray();
            paths[0] = MAINFOLDER;

            Node currentNode = parent;
            var totalPathCount = catalog.TypeName.Equals(FOLDER, StringComparison.InvariantCultureIgnoreCase) ? paths.Count() : paths.Count() - 1;
            for (int pathCount = 0; pathCount < totalPathCount; pathCount++)
            {
                var prevNode = currentNode;
                //find any nodes that matches the 
                currentNode = Node.FindNode(currentNode, paths[pathCount]);

                //if the node is null, create a new folder
                if (currentNode == null)
                {
                    currentNode = Node.CreateFolder(prevNode, paths[pathCount], string.Join("_", paths).Replace(" ", "_"));
                }
            }

            //if the catalog is not a report type, return the folder
            if (!catalog.TypeName.Equals(REPORT, StringComparison.InvariantCultureIgnoreCase))
            {
                return currentNode;
            }

            //create a report and add it to the parent node
            ReportBE report = new ReportBE();
            report.ReportName = catalog.Name;
            report.ReportDescription = catalog.Description;
            report.ReportGroup = catalog.Name;
            report.ReportImage = "";
            report.ReportImageURL = GetReportImage(catalog.Path);
            report.ReportPath = catalog.Path;
            report.ReportType = catalog.TypeName;
            report.ReportParameters = ReportServer.GetItemParameters(httpContext, catalog.Path, null, true, null, null);

            currentNode.Reports.Add(report);

            return currentNode;
        }
        /// <summary>
        /// Function that iterates through the catalog items and format then in a node type object
        /// </summary>
        /// <param name="ciList">catalog item</param>
        /// <returns></returns>
        public Node CreateReportDirectoryHierarchy(List<CatalogItem> ciList, HttpContextBase httpContext)
        {
            //create a parent Node
            Node parent = new Node()
            {
                Name = MAINFOLDER,
                CssClass = MAINFOLDER
            };

            //iterate the catalog items and add them in the node
            Node node = parent;
            foreach (var ci in ciList)
            {
                node = AddChild(parent, ci, httpContext);
            }

            //Hide any folder that does not contain any report files
            HideNonReportDirectory(parent);
            return parent;
        }

        /// <summary>
        /// Hides folder that does not contain and report file
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool HideNonReportDirectory(Node node)
        {
            var ret = false;
            if (node.Children != null && node.Children.Any())
            {
                foreach (var child in node.Children)
                {
                    ret = HideNonReportDirectory(child);
                }
            }

            if (!ret && node.Reports.Any(x => x.ReportType.Equals(REPORT, StringComparison.InvariantCultureIgnoreCase)))
            {
                ret = true;
            }

            if (ret == false)
            {
                node.CssClass = "hidden";
            }

            return ret;
        }
        #endregion  
    }
}