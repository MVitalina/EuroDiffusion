using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroDiffusion.MainLogic
{
    class Errors
    {
        public static void WriteError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--X-X-- Error: " + error + ", try again --X-X--");
            Console.ResetColor();
        }

        public static void WriteFatalError(string funcName)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("--X-X-- FATAL Error: " + funcName + ". Exiting --X-X--");
            Console.ResetColor();
        }
    }
}
