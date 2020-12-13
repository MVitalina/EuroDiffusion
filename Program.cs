using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EuroDiffusion
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Diffuser diffuser = new Diffuser();
            int attempts = 0;
            while (attempts < 100)
            {
                attempts++;
                if (!diffuser.Run(attempts))
                {
                    Console.ReadKey();
                    return;
                }
            }
            Console.ReadKey();
            return;
        }
    }
}
