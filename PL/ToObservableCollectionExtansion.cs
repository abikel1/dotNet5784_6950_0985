using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PL;
internal static class ToObservableCollectionExtansion
{
    internal static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items) => new ObservableCollection<T>(items);
}
