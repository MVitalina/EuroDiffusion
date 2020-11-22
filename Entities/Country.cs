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
        public Country() : this("", new Point(), new Point(), -1)
        {

        }

        public Country(string name, Point firstCoord, Point secondCoord, int countryNumber)
        {
            Name = name;
            Motif = countryNumber;
            FirstCoord = firstCoord;
            SecondCoord = secondCoord;
            NumberOfDaysToComplete = 0;
            EachTownHasAllMotifs = false;
        }

        public string Name { get; private set; }
        public int Motif { get; private set; }
        public int NumberOfDaysToComplete { get; set; }
        public bool EachTownHasAllMotifs { get; set; }
        public Point FirstCoord { get; set; }
        public Point SecondCoord { get; set; }

        public bool IsEmpty()
        {
            return Name == "" || Motif < 0;
        }

        public void SetNumberOfDaysToComplete(int days)
        {
            if (NumberOfDaysToComplete < days)
            {
                NumberOfDaysToComplete = days;
            }
        }
    }
}
