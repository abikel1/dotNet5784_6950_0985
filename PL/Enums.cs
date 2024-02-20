

using System.Collections.Generic;
using System;
using System.Collections;

namespace PL;

internal class RankCollection : IEnumerable
{
    static readonly IEnumerable<BO.Rank> s_enums = (Enum.GetValues(typeof(BO.Rank)) as IEnumerable<BO.Rank>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class StatusCollection : IEnumerable
{
    static readonly IEnumerable<BO.Status> s_enums = (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
