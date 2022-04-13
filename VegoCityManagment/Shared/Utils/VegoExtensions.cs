using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoCityManagment.Shared.Utils
{
    public static class VegoExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> self)
            => new(self);
    }
}
