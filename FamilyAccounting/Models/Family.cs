using FamilyAccounting.Models.Interfaces;
using System;

namespace FamilyAccounting.Models
{
    public class Family : IDataModel
    {
        public int id { get; set; }
        public DateTime Birthdate { get; set; }
        public string Name { get; set; }

    }
}
