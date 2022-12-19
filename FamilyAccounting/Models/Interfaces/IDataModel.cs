using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAccounting.Models.Interfaces
{
    public interface IDataModel
    {
        int id { get; }
    }
    public interface ICategory
    {
        int Category { get; set; }
    }
}
