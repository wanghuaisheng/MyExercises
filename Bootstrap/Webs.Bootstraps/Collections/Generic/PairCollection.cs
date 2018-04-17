using System.Collections.Generic;
using Webs.Bootstraps.Collections.Generic.PowerCollections;

namespace Webs.Bootstraps.Collections.Generic
{
    internal class PairCollection<TFirst, TSecond> : List<Pair<TFirst, TSecond>>
    {
        public void Add(TFirst first, TSecond second)
        {
            this.Add(new Pair<TFirst, TSecond>
            {
                First = first,
                Second = second
            });
        }
    }
}