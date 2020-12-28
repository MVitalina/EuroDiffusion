using EuroDiffusion.Entities;
using EuroDiffusion.MainLogic;
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

        List<Country> m_countries = new List<Country>(); 

        Town[,] Towns = new Town[Constants.Dimension, Constants.Dimension]; //towns are stored here
        int NumberOfCountries = 0;

        public bool Run(int numberOfCase)
        {
            Towns = new Town[Constants.Dimension, Constants.Dimension];
            NumberOfCountries = 0;
            m_countries = new List<Country>();

            NumberOfCountries = IOProcessor.ReadNumberOfCountries();
            if (NumberOfCountries == 0)
            {
                return false;
            }
            else if (NumberOfCountries < 0)
            {
                Errors.WriteFatalError("ReadNumberOfCountries()");
                return false;
            }

            for (int i = 0; i < NumberOfCountries; i++)
            {
                Country country = IOProcessor.ReadCountry(i);

                if (country.IsEmpty())
                {
                    Errors.WriteFatalError($"ReadCountry({i})");
                    return false;
                }

                //setting all country`s towns on map
                for (int x = country.FirstCoord.X; x <= country.SecondCoord.X; x++)
                {
                    for (int y = country.FirstCoord.Y; y <= country.SecondCoord.Y; y++)
                    {
                        if (Towns[x, y] != null)
                        {
                            Errors.WriteFatalError("Logical error: can`t set Town again.");
                            return false;
                        }

                        Towns[x, y] = new Town(new Point(x, y), country, NumberOfCountries);
                    }
                }
            }

            int day = 1;
            do
            {
                for (int x = 1; x < Constants.Dimension; x++)
                {
                    for (int y = 1; y < Constants.Dimension; y++)
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
            } while (!EachTownHasAllMotifs() && day < Constants.MaxNumberOfIterationsForCounting);

            if (day == Constants.MaxNumberOfIterationsForCounting)
            {
                Errors.WriteFatalError("while loop");
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
            for (int x = 1; x < Constants.Dimension; x++)
            {
                for (int y = 1; y < Constants.Dimension; y++)
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
            for (int x = 1; x < Constants.Dimension; x++)
            {
                for (int y = 1; y < Constants.Dimension; y++)
                {
                    if (Towns[x, y] != null && !Towns[x, y].HasAllMotifs())
                    {
                        return false;
                    }
                }
            }

            return true;
        }   
    }
}
