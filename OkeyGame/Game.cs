using System;
using System.Collections.Generic;
using System.Linq;

namespace OkeyGame
{
    internal static class Game
    {
        static Random rnd = new Random();
        public static List<Stone> GameStones { get; set; }
        public static List<Player> PlayerList { get; set; }

        static Game()
        {
            GameStones = new List<Stone>();
            PlayerList = new List<Player>();
        }

        /// <summary>
        /// Oyun içinde kullanılacak taşları oluşturur.
        /// </summary>
        public static void GenerateGameStones()
        {
            for (int i = 0; i < 106; i++)
            {
                GameStones.Add(new Stone { Number = i % 53 });
            }
        }

        /// <summary>
        /// Taşları karıştırır
        /// </summary>
        public static void MixStones()
        {
            GameStones = GameStones.OrderBy(item => rnd.Next()).ToList<Stone>();
        }

        /// <summary>
        /// Gösterge ve Okey belirler.
        /// </summary>
        public static void GenerateOkey()
        {
            //Gösterge belirleniyor
            int indicatorNumber = rnd.Next(0, 53);
            //sahte okey geldiyse kontrolü
            indicatorNumber = indicatorNumber != 52 ? indicatorNumber : indicatorNumber - 1;
            GameStones.FindAll(x => x.Number == indicatorNumber).ToList<Stone>().ForEach(item => item.IsIndicator = true);
            //Göstergelerden biri kullanıldı olarak belirleniyor ki oyunculara 2 kere gösterge dağıtılmasın.
            GameStones.FirstOrDefault(c => c.IsIndicator).IsDistributed = true;

            //Okey ayarlanıyor
            GameStones.FindAll(x => x.Number == (indicatorNumber + 1 % 13 == 0 ? indicatorNumber - 12 : indicatorNumber + 1)).
                ToList<Stone>().ForEach(item => item.IsOkey = true);
        }

        /// <summary>
        /// Taşları kullanıcılara dağıtır.
        /// </summary>
        public static void DistributeStones()
        {
            PlayerList.Add(new Player { Id = 1, UserName = "FirstPlayer" });
            PlayerList.Add(new Player { Id = 2, UserName = "SecondPlayer" });
            PlayerList.Add(new Player { Id = 3, UserName = "ThirdPlayer" });
            PlayerList.Add(new Player { Id = 4, UserName = "ForthPlayer" });

            //15 Taşlı kullanıcı belirleniyor.
            int fifteenStonePlayer = rnd.Next(0, 4);

            for (int i = 0; i < 106; i++)
            {
                if (GameStones[i].IsDistributed)
                    continue;

                //ilk önce 15 taş verilmesi gereken kullanıcıya taşlarını veriyoruz.
                if (PlayerList[fifteenStonePlayer].PlayerStones.Count < 15)
                {
                    switch (fifteenStonePlayer)
                    {
                        case 0:
                            PlayerList[0].PlayerStones.Add(GameStones[i]);
                            GameStones[i].IsDistributed = true;
                            break;
                        case 1:
                            PlayerList[1].PlayerStones.Add(GameStones[i]);
                            GameStones[i].IsDistributed = true;
                            break;
                        case 2:
                            PlayerList[2].PlayerStones.Add(GameStones[i]);
                            GameStones[i].IsDistributed = true;
                            break;
                        case 3:
                            PlayerList[3].PlayerStones.Add(GameStones[i]);
                            GameStones[i].IsDistributed = true;
                            break;
                    }
                }
                else
                {
                    if (PlayerList.FirstOrDefault(x => x.PlayerStones.Count < 14) != null &&
                        PlayerList.FirstOrDefault(x => x.PlayerStones.Count < 14).PlayerStones.Count < 14)
                    {
                        if (PlayerList.FirstOrDefault(x => x.PlayerStones.Count > 0 && x.PlayerStones.Count < 14) != null)
                        {
                            switch (PlayerList.FirstOrDefault(x => x.PlayerStones.Count > 0 && x.PlayerStones.Count < 14).Id)
                            {
                                case 1:
                                    PlayerList[0].PlayerStones.Add(GameStones[i]);
                                    GameStones[i].IsDistributed = true;
                                    break;
                                case 2:
                                    PlayerList[1].PlayerStones.Add(GameStones[i]);
                                    GameStones[i].IsDistributed = true;
                                    break;
                                case 3:
                                    PlayerList[2].PlayerStones.Add(GameStones[i]);
                                    GameStones[i].IsDistributed = true;
                                    break;
                                case 4:
                                    PlayerList[3].PlayerStones.Add(GameStones[i]);
                                    GameStones[i].IsDistributed = true;
                                    break;
                            }
                        }
                        else
                        {
                            switch (PlayerList.FirstOrDefault(x => x.PlayerStones.Count == 0).Id)
                            {
                                case 1:
                                    PlayerList[0].PlayerStones.Add(GameStones.FirstOrDefault(x => x.Number == i));
                                    GameStones.FirstOrDefault(x => x.Number == i).IsDistributed = true;
                                    break;
                                case 2:
                                    PlayerList[1].PlayerStones.Add(GameStones.FirstOrDefault(x => x.Number == i));
                                    GameStones.FirstOrDefault(x => x.Number == i).IsDistributed = true;
                                    break;
                                case 3:
                                    PlayerList[2].PlayerStones.Add(GameStones.FirstOrDefault(x => x.Number == i));
                                    GameStones.FirstOrDefault(x => x.Number == i).IsDistributed = true;
                                    break;
                                case 4:
                                    PlayerList[3].PlayerStones.Add(GameStones.FirstOrDefault(x => x.Number == i));
                                    GameStones.FirstOrDefault(x => x.Number == i).IsDistributed = true;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public static Player BestHandPlayer()
        {
            int bestHand = -1;
            for (int i = 0; i < PlayerList.Count; i++)
            {
                PlayerList[i].GamePoint = CalculateHandPoint(PlayerList[i].PlayerStones);
                if (bestHand < PlayerList[i].GamePoint)
                {
                    bestHand = PlayerList[i].GamePoint;
                }
            }
            return PlayerList.FirstOrDefault(x => x.GamePoint == bestHand);
        }

        private static int CalculateHandPoint(List<Stone> userStones)
        {
            int okey = GameStones.FirstOrDefault(x => x.IsOkey).Number;
            int point = 0;
            int jokerCount = userStones.Where(s => s.Number.Equals(okey)).Count();
            point += jokerCount * 3;
            userStones.Where(w => w.Number.Equals(okey)).ToList().ForEach(s => s.Number = 999);
            userStones.Where(w => w.Number.Equals(52)).ToList().ForEach(s => s.Number = okey);
            userStones.Sort(new SortStonesByNumber());
            var groupsYellow = userStones.Where(x => ((int)(x.Number / 13)) == 0).GroupConsecutive();
            var groupsBlue = userStones.Where(x => ((int)(x.Number / 13)) == 1).GroupConsecutive();
            var groupsBlack = userStones.Where(x => ((int)(x.Number / 13)) == 2).GroupConsecutive();
            var groupsRed = userStones.Where(x => ((int)(x.Number / 13)) == 3).GroupConsecutive();

            for (int i = 0; i < groupsYellow.Count(); i++)
            {
                int plusPoint = GetPoint(groupsYellow.ToList()[i].Count());
                if (groupsYellow.ToList()[i].Count() > 2)
                {
                    foreach (var item in groupsYellow.ToList()[i])
                    {
                        userStones.Where(x => x.Number == item).First().Used = true;
                    }
                }
                point += plusPoint;
            }
            for (int i = 0; i < groupsBlue.Count(); i++)
            {
                int plusPoint = GetPoint(groupsBlue.ToList()[i].Count());
                if (groupsBlue.ToList()[i].Count() > 2)
                {
                    foreach (var item in groupsBlue.ToList()[i])
                    {
                        userStones.Where(x => x.Number == item).First().Used = true;
                    }
                }
                point += plusPoint;
            }
            for (int i = 0; i < groupsBlack.Count(); i++)
            {
                int plusPoint = GetPoint(groupsBlack.ToList()[i].Count());
                if (groupsBlack.ToList()[i].Count() > 2)
                {
                    foreach (var item in groupsBlack.ToList()[i])
                    {
                        userStones.Where(x => x.Number == item).First().Used = true;
                    }
                }
                point += plusPoint;
            }
            for (int i = 0; i < groupsRed.Count(); i++)
            {
                int plusPoint = GetPoint(groupsRed.ToList()[i].Count());
                if (groupsRed.ToList()[i].Count() > 2)
                {
                    foreach (var item in groupsRed.ToList()[i])
                    {
                        userStones.Where(x => x.Number == item).First().Used = true;
                    }
                }
                point += plusPoint;

            }
            for (int i = 0; i < userStones.Count; i++)
            {
                if (!userStones[i].Used)
                {
                    var lst = userStones.Where(x => !x.Used && (x.Number == userStones[i].Number + 13
                                    || x.Number == userStones[i].Number + 26
                                    || x.Number == userStones[i].Number + 39)
                     );
                    int count = lst.Count() + 1;
                    if (count == 2)
                    {
                        point += 1;
                    }
                    else if (count == 3)
                    {
                        point += 3;
                    }
                    else if (count == 4)
                    {
                        point += 4;
                    }
                    if (count > 2)
                    {
                        foreach (var item in lst)
                        {
                            userStones.Where(x => x.Number == item.Number).First().Used = true;
                        }
                        userStones.Where(x => x.Number == userStones[i].Number).First().Used = true;

                    }
                }
            }


            int twinPoint = 0;
            for (int i = 0; i < userStones.Count; i++)
            {
                if (userStones[i].Number != 999 && userStones.Where(x => x.Number == userStones[i].Number).Count() == 2)
                {
                    twinPoint += 2;
                }
            }
            twinPoint += (jokerCount * 2);



            for (int i = 0; i < userStones.Count; i++)
            {
                if (userStones[i].Number < 13)
                {
                    Console.WriteLine(userStones[i].Number % 13 + 1 + "\tSARI");
                }
                else if (userStones[i].Number < 26)
                {
                    Console.WriteLine(userStones[i].Number % 13 + 1 + "\tMAVİ");

                }
                else if (userStones[i].Number < 39)
                {
                    Console.WriteLine(userStones[i].Number % 13 + 1 + "\tSİYAH");

                }
                else if (userStones[i].Number < 52)
                {
                    Console.WriteLine(userStones[i].Number % 13 + 1 + "\tKIRMIZI");

                }
            }
            Console.WriteLine(jokerCount + "\tAdet Okey");
            Console.WriteLine(Math.Max(point, twinPoint) + "\tPuan");
            Console.WriteLine("==================================================");

            return Math.Max(point, twinPoint);
        }

        private static int GetPoint(int v)
        {
            int newPoint = 0;
            switch (v)
            {
                case 0:
                case 1:
                    break;
                case 2:
                    newPoint = 1;
                    break;
                case 3:
                case 4:
                    newPoint = v;
                    break;
                case 5:
                    newPoint = 4;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                    newPoint = v;
                    break;
                default:
                    break;
            }

            return newPoint;
        }

        public static IEnumerable<IEnumerable<int>> GroupConsecutive(this IEnumerable<Stone> list)
        {
            var group = new List<int>();
            foreach (var i in list)
            {
                if (group.Count == 0 || i.Number - group[group.Count - 1] <= 1)  /// Bir oncekiyle ayniysa ya da ardisiksa gruba ekle
                {
                    if (!group.Contains(i.Number))                              //Ayni tastan 2 kere varsa gruba ekleme
                        group.Add(i.Number);
                }
                else if (i.Number % 13 == 12 && group.Count > 1 && list.Where(x => x.Number == 0).Count() > 0) /// 12 , 13 varsa 1 de gelebilir
                {
                    group.Add(i.Number);
                }
                else                                                              /// Ardisik olmadigi icin yeni grup olustur
                {
                    yield return group;
                    group = new List<int> { i.Number };
                }
            }
            yield return group;
        }
    }
}
