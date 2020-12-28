using EuroDiffusion.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroDiffusion.MainLogic
{
    class IOProcessor
    {
        static public int ReadNumberOfCountries()
        {
            int attempts = 0;
            while (attempts < Constants.MaxNumberOfAttemptsInWhile)
            {
                attempts++;

                Console.WriteLine("Enter number of countries (1 <= number <= 20) or 0 to Exit:");
                string numberOfCountriesStr = Console.ReadLine().Trim();

                if (!int.TryParse(numberOfCountriesStr, out int numberOfCountries))
                {
                    Errors.WriteError("Wrong number of countries");
                    continue;
                }

                if (numberOfCountries == 0)
                {
                    return numberOfCountries;
                }

                if (numberOfCountries < 1 || numberOfCountries > Constants.MaxCountOfCountries)
                {
                    Errors.WriteError("Out of range");
                    continue;
                }

                return numberOfCountries;
            }

            return -1;
        }

        static public Country ReadCountry(int iterator)
        {
            int attempts = 0;
            while (attempts < Constants.MaxNumberOfAttemptsInWhile)
            {
                attempts++;

                Console.WriteLine(iterator + ". Enter name of country with two extreme points(1 <= x1 <= x2 <= 10; 1 <= y1 <= y2 <= 10) in format: 'Country_Name x1 y1 x2 y2'");
                string line = Console.ReadLine().Trim();
                string[] parts = line.Split(' ');

                if (parts.Length != Constants.NumberOfCoords + 1) // +1 for country name
                {
                    Errors.WriteError("Wrong format");
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

        private static bool ValidatePoints(string[] parts)
        {
            for (int i = 1; i < Constants.NumberOfCoords; i++)
            {
                if (!int.TryParse(parts[i], out int num))
                {
                    Errors.WriteError("Points need to be entered as numbers");
                    return false;
                }

                if (num < 1 || num >= Constants.Dimension)
                {
                    Errors.WriteError("Out of range");
                    return false;
                }
            }

            return true;
        }
    }
}
