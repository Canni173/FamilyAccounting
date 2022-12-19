using FamilyAccounting.Models.Interfaces;

namespace FamilyAccounting.Models
{
    public class AccountingInCategory : IDataModel
    {
        public int id { get; set; }
        public string User { get; set; }
        public string Category { get; set; }
        public string CategoryType { get; set; }
        public double Suum { get; set; } 
        public int Events { get; set; }

    }

}
