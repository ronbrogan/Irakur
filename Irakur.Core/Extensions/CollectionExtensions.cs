using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Core.Extensions
{
    public static class CollectionExtensions
    {

        public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            if(items != null)
            {
                foreach (var item in items)
                    stack.Push(item);
            }
        }

    }
}
