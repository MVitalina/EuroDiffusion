using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroDiffusion.Entities
{
    class Town
    {
        public Town(Point coord, Country country, int countOfMotifs) 
        {
            Coord = coord;
            Country = country;

            Coins = new int[countOfMotifs];
            AccumulatedCoins = new int[countOfMotifs];
            for (int i = 0; i < Coins.Length; i++)
            {
                Coins[i] = 0;
                AccumulatedCoins[i] = 0;
            }
            Coins[country.Motif] = 1000000;

            DiffusionCompleted = false;
        }

        public int[] Coins { get; private set; }
        public int[] AccumulatedCoins { get; private set; }
        public Point Coord { get; private set; }
        public Country Country { get; private set; }
        public bool DiffusionCompleted { get; set; }


        public bool HasAllMotifs()
        {
            return !Coins.Where(x => x <= 0).Any();
        }

        public void AccumulateCoins()
        {
            for (int i = 0; i < Coins.Length; i++)
            {
                Coins[i] += AccumulatedCoins[i];
                AccumulatedCoins[i] = 0;
            }
        }

        public int[] GetRepresentativeCoins()
        {
            int[] representativeCoins = new int[Coins.Length];
            for (int i = 0; i < Coins.Length; i++)
            {
                representativeCoins[i] = Coins[i] / 1000;
                Coins[i] -= representativeCoins[i];
            }
             
            return representativeCoins;
        }

        public void SetRepresentativeCoins(int[] representativeCoins)
        {
            if (representativeCoins.Length != Coins.Length)
            {
                return;
            }

            for (int i = 0; i < Coins.Length; i++)
            {
                AccumulatedCoins[i] += representativeCoins[i];
            }
        }
    }
}
