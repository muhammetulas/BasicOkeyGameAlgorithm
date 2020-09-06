using System;
using System.Collections.Generic;
using System.Text;

namespace OkeyGame
{
    internal class SortStonesByNumber : IComparer<Stone>
    {
        public int Compare(Stone x, Stone y)
        {
            return x.Number.CompareTo(y.Number);
        }
    }
}
