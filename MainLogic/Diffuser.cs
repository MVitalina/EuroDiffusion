using EuroDiffusion.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EuroDiffusion
{
    class Diffuser
    {
        public Diffuser()
        {

        }

        List<Country> m_countries = new List<Country>(); //TODO exclusive

        Town[,] Towns = new Town[11, 11]; //towns are stored here
        int NumberOfCountries = 0;

        public bool Run(int numberOfCase)
        {
            Towns = new Town[11, 11];
            NumberOfCountries = 0;
            m_countries = new List<Country>();

            //TODO to UI func
            NumberOfCountries = ReadNumberOfCountries();
            if (NumberOfCountries == 0)
            {
                return false;
            }
            else if (NumberOfCountries < 0)
            {
                WriteFatalError("ReadNumberOfCountries()");
                return false;
            }

            //TODO to UI func
            for (int i = 0; i < NumberOfCountries; i++)
            {
                Country country = ReadCountry(i);

                if (country.IsEmpty())
                {
                    WriteFatalError($"ReadCountry({i})");
                    return false;
                }

                //setting all country`s towns on map
                for (int x = country.FirstCoord.X; x <= country.SecondCoord.X; x++)
                {
                    for (int y = country.FirstCoord.Y; y <= country.SecondCoord.Y; y++)
                    {
                        if (Towns[x, y] == null)
                        {
                            Towns[x, y] = new Town(new Point(x, y), country, NumberOfCountries);
                        }
                        else
                        {
                            WriteFatalError("Logical error: can`t set Town again.");
                            return false;
                        }
                    }
                }
            }

            int day = 1;
            do
            {
                for (int x = 1; x <= 10; x++)
                {
                    for (int y = 1; y <= 10; y++)
                    {
                        if (Towns[x, y] == null)
                        {
                            continue;
                        }

                        SendRepresentativeCoins(x, y, x + 1, y); //East 
                        SendRepresentativeCoins(x, y, x, y + 1); //North
                        SendRepresentativeCoins(x, y, x - 1, y); //West
                        SendRepresentativeCoins(x, y, x, y - 1); //South
                    }
                }

                BureaucraticIssuesInTheEvening(day);
                day++;
            } while (!EachTownHasAllMotifs() && day < 10000);

            if (day == 10000)
            {
                WriteFatalError("while loop");
                return false;
            }

            m_countries = m_countries.OrderBy(c => c.NumberOfDaysToComplete).ToList();
            Console.WriteLine("Case Number " + numberOfCase);
            foreach (var country in m_countries)
            {
                Console.WriteLine(country.Name + "\t" + country.NumberOfDaysToComplete);
            }

            return true;
        }

        private void BureaucraticIssuesInTheEvening(int day)
        {
            for (int x = 1; x <= 10; x++)
            {
                for (int y = 1; y <= 10; y++)
                {
                    if (Towns[x, y] == null)
                    {
                        continue;
                    }

                    Towns[x, y].AccumulateCoins();
                    CheckIfDiffusionIsCompleted(x, y, day);
                }
            }
        }

        private void CheckIfDiffusionIsCompleted(int x, int y, int day)
        {
            if (Towns[x, y] == null || Towns[x, y].DiffusionCompleted || !Towns[x, y].HasAllMotifs())
            {
                return;
            }

            if (m_countries.Any(c => c.Name == Towns[x, y].Country.Name))
            {
                foreach (var country in m_countries)
                {
                    if (country.Name == Towns[x, y].Country.Name)
                    {
                        country.SetNumberOfDaysToComplete(day);
                        break;
                    }
                }
            }
            else
            {
                m_countries.Add(Towns[x, y].Country);
                m_countries.Last().SetNumberOfDaysToComplete(day);
            }

            Towns[x, y].DiffusionCompleted = true;
        }

        private bool SendRepresentativeCoins(int xFrom, int yFrom, int xTo, int yTo)
        {
            if (Towns[xFrom, yFrom] != null && Towns[xTo, yTo] != null)
            {
                Towns[xTo, yTo].SetRepresentativeCoins(
                    Towns[xFrom, yFrom].GetRepresentativeCoins());
                return true;
            }

            return false;
        }

        private bool EachTownHasAllMotifs()
        {
            for (int x = 1; x <= 10; x++)
            {
                for (int y = 1; y <= 10; y++)
                {
                    if (Towns[x, y] != null && !Towns[x, y].HasAllMotifs())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private int ReadNumberOfCountries()
        {
            int attempts = 0;
            while (attempts < 100)
            {
                attempts++;

                Console.WriteLine("Enter number of countries (1 <= number <= 20) or 0 to Exit:");
                string numberOfCountriesStr = Console.ReadLine().Trim();

                if (!int.TryParse(numberOfCountriesStr, out int numberOfCountries))
                {
                    WriteError("Wrong number of countries");
                    continue;
                }

                if (numberOfCountries == 0)
                {
                    return numberOfCountries;
                }

                if (numberOfCountries < 1 || numberOfCountries > Constants.MaxCountOfCountries)
                {
                    WriteError("Out of range");
                    continue;
                }

                return numberOfCountries;
            }

            return -1;
        }

        private Country ReadCountry(int iterator)
        {
            int attempts = 0;
            while (attempts < 100)
            {
                attempts++;

                Console.WriteLine(iterator + ". Enter name of country with two extreme points(1 <= x1 <= x2 <= 10; 1 <= y1 <= y2 <= 10) in format: 'Country_Name x1 y1 x2 y2'");
                string line = Console.ReadLine().Trim();
                string[] parts = line.Split(' ');

                if (parts.Length != 5)
                {
                    WriteError("Wrong format");
                    continue;
                }

                if (!ValidatePoints(parts))
                {
                    continue;
                }

                Point first = new Point(int.Parse(parts[1]), int.Parse(parts[2]));
                Point second = new Point(int.Parse(parts[3]), int.Parse(parts[4]));

                if (first.X > second.X || first.Y > second.Y)
                {
                    return new Country();
                }

                return new Country(parts[0], 
                    new Point(int.Parse(parts[1]), int.Parse(parts[2])), 
                    new Point(int.Parse(parts[3]), int.Parse(parts[4])), 
                    iterator);
            }

            return new Country();
        }

        private bool ValidatePoints(string[] parts)
        {
            //TODO first < second coord
            for (int i = 1; i < 4; i++)
            {
                if (!int.TryParse(parts[i], out int num))
                {
                    WriteError("Points need to be entered as numbers");
                    return false;
                }

                if (num < 1 || num > 10)
                {
                    WriteError("Out of range");
                    return false;
                }
            }

            return true;
        }

        private void WriteError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--X-X-- Error: " + error + ", try again --X-X--");
            Console.ResetColor();
        }

        private void WriteFatalError(string funcName)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("--X-X-- FATAL Error: " + funcName + ". Exiting --X-X--");
            Console.ResetColor();
        }
    }
}
