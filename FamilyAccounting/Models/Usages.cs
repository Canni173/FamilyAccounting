using FamilyAccounting.Models.Interfaces;
using System;

namespace FamilyAccounting.Models
{
    public class Usages : IDataModel, ICategory
    {
        public int id { get; set; }
        public int User { get; set; }
        public int Category { get; set; }
        public int Usage { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

    }
}
