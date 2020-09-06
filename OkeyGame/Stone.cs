using System;
using System.Collections.Generic;
using System.Text;

namespace OkeyGame
{
    internal class Stone //internal tanımlama sebebim sadece OkeyGame namespacesinden erişilebilmesi için.
    {
        public int Number { get; set; }
        public bool Used { get; set; } = false;
        public bool IsIndicator { get; set; } = false;
        public bool IsOkey { get; set; } = false;
        public bool IsDistributed { get; set; } = false;
    }
}
