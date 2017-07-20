using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTX.ReportViewer.DataLayer
{
    public interface IDropDownSetsRepository
    {
        List<DropDownSetBE> GetAllSetNames();

        List<DropDownSetBE> GetAllUnitSizeMLs(string periodkey, string setnames);

        List<DropDownSetBE> GetAllPeriodKeys();

        List<DropDownSetBE> GetMyCategories();
    }
}
