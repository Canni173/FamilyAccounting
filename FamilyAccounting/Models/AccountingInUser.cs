using FamilyAccounting.Models.Interfaces;

namespace FamilyAccounting.Models
{
    public class AccountingInUser : IDataModel
    {
        public int id { get; set; }
        public string User { get; set; }
        /// <summary>
        /// Потрачено
        /// </summary>
        public double Spent { get; set; }
        /// <summary>
        /// Заработано
        /// </summary>
        public double Win { get; set; }  
 

    }

}
