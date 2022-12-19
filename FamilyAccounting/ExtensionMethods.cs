using FamilyAccounting.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace FamilyAccounting
{
    public static class ExtensionMethods
    {
        public static T FindById<T>(this IEnumerable<T> source, int id) where T : IDataModel
=> id == -1 ? default(T) : source.FirstOrDefault(p => p.id == id);
    }
}
