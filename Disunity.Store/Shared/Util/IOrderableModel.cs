using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Disunity.Store.Shared.Util
{
    public enum Ordering
    {
        Asc,
        Desc
    }

    public interface IOrderableModel
    {
        string OrderBy { get; }
        Ordering? Order { get; }

        IEnumerable<string> OrderByChoices { get; }

    }
}
