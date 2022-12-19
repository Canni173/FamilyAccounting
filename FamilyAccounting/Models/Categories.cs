using FamilyAccounting.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAccounting.Models
{
    public class Categori : IDataModel
    {
        public int id { get; set; }
        public string Type { get; set; }
        public string TypeText
        {
            get
            {
               var categoryType = string.Empty;
                if (Type == "S") categoryType = "Доход";
                if (Type == "U") categoryType = "Расход";
                return categoryType;
            } 
        }

        public string Name { get; set; }

    }
}
