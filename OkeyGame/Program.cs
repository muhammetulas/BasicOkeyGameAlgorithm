using System;
using System.Collections.Generic;
using System.Linq;

namespace OkeyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.GenerateGameStones();
            Game.MixStones();
            Game.GenerateOkey();
            Game.DistributeStones();
            Player p = Game.BestHandPlayer();

            Console.WriteLine("En iyi ele sahip oyuncu: {0}, Puanı: {1}", p.UserName, p.GamePoint);

            Console.ReadKey();
        }
    }
}
