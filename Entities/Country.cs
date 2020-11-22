using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EuroDiffusion.Entities
{
    class Country
    {
        public Country(string name, Point firstCoord, Point secondCoord, int countryNumber)
        {
            Name = name;
            CountryNumber = countryNumber;
            FillTowns(firstCoord, secondCoord);
        }

        public string Name { get; private set; }
        public int CountryNumber { get; private set; }
        public List<Town> Towns { get; set; }

        private void FillTowns(Point firstCoord, Point secondCoord)
        {
            //TODO validate

            Towns = new List<Town>();

            for (int x = firstCoord.X; x <= secondCoord.X; x++)
            {
                for (int y = firstCoord.Y; y <= secondCoord.Y; y++)
                {
                    int[] coins = new int[Constants.CountOfCountries];
                    for (int i = 0; i < coins.Count(); i++)
                    {
                        coins[i] = 0;
                    }
                    coins[CountryNumber] = 1000000;

                    Towns.Add(new Town(new Point(x, y), coins));
                }
            }

        }

    }
}
