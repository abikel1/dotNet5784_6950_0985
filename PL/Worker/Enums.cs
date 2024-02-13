

using System;
using System.Collections;
using System.Collections.Generic;

namespace PL.Worker;

internal class RankCollection :IEnumerable
{
    static readonly IEnumerable<BO.Rank> s_enums = (Enum.GetValues(typeof(BO.Rank)) as IEnumerable<BO.Rank>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
