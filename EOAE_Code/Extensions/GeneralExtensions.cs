using System;
using System.Collections.Generic;

namespace EOAE_Code.Extensions
{
    public static class GeneralExtensions
    {
        // Regular LinQ MaxBy method is not available in used C# version, and Taleword one will cause ambigious reference issue
        public static TSource MaxByCustom<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return TaleWorlds.Core.Extensions.MaxBy(source, selector);
        }
    }
}
