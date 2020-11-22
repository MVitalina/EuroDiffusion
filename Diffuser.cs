using EuroDiffusion.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroDiffusion
{
    class Diffuser
    {
        public Diffuser()
        {

        }

        public bool Run()
        {
            int numberOfCountries = ReadNumberOfCountries();
            if (numberOfCountries == 0)
            {
                return false;
            }

            for (int i = 0; i < numberOfCountries; i++)
            {
                Country country = ReadCountry(i);
                //TODO main logic
            }

            return true;
        }

        private int ReadNumberOfCountries()
        {
            while (true)
            {
                Console.WriteLine("Enter number of countries (1 ≤ number ≤ 20) or 0 to Exit:");
                string numberOfCountriesStr = Console.ReadLine();

                if (!int.TryParse(numberOfCountriesStr, out int numberOfCountries))
                {
                    WriteError("Wrong number of countries");
                    continue;
                }

                if (numberOfCountries < 1 || numberOfCountries > 20)
                {
                    WriteError("Out of range");
                    continue;
                }

                return numberOfCountries;
            }
        }

        private Country ReadCountry(int iterator)
        {
            while (true)
            {
                Console.WriteLine(iterator + ". Enter name of country with two extreme points(1 ≤ x1 ≤ x2 ≤ 10; 1 ≤ y1 ≤ y2 ≤ 10) in format:");
                Console.WriteLine("Country_Name x1 y1 x2 y2");
                string line = Console.ReadLine();
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

                return new Country(parts[0], 
                    new Point(int.Parse(parts[1]), int.Parse(parts[2])), 
                    new Point(int.Parse(parts[3]), int.Parse(parts[4])), 
                    iterator);
            }
        }

        private bool ValidatePoints(string[] parts)
        {
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
            Console.WriteLine("---Error: " + error + ", try again---");
        }
    }
}
