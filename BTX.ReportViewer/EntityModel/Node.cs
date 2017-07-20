using BTX.ReportViewer.EntityModel;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BTX.ReportViewer.EntityModel
{
    public class Node
    {
        public List<ReportBE> Reports;
        public string Name;
        public List<Node> Children;
        public string CssClass;

        public Node()
        {
            Children = new List<Node>();
            Reports = new List<ReportBE>();
        }

        public static Node FindNode(Node startingNode, string nodeName)
        {

            if (startingNode.Name.Equals(nodeName, StringComparison.InvariantCultureIgnoreCase))
            {
                return startingNode;
            }

            if (startingNode.Children.Count() > 0)
            {
                foreach (var node in startingNode.Children)
                {
                    var ret = FindNode(node, nodeName);
                    if (ret != null)
                        return ret;
                }
            }

            return null;
        }

        public static Node CreateFolder(Node parentNode, string folderName, string cssClass = null)
        {
            var node = new Node();
            node.Name = folderName;
            node.CssClass = cssClass;
            parentNode.Children.Add(node);
            return node;
        }

    }
}