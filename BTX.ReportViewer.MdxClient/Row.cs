using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTX.ReportViewer.MdxClient
{
    internal class Row
    {
        public List<Cell> Cells { get; set; }

        public Row()
        {
            Cells = new List<Cell>();
        }
    }
}
