using FamilyAccounting.Models.Interfaces;
using System;

namespace FamilyAccounting.Models
{
    public class Source : IDataModel, ICategory
    {
        public int id { get; set; }
        public int User { get; set; }
        public int Category { get; set; }
        public double Income { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

    }

}
