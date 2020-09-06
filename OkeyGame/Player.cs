using System;
using System.Collections.Generic;
using System.Text;

namespace OkeyGame
{
    internal class Player
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public List<Stone> PlayerStones { get; set; }
        public int GamePoint { get; set; } = 0;

        public Player()
        {
            this.PlayerStones = new List<Stone>();
        }
    }
}
