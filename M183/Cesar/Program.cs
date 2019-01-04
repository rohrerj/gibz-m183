using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesar
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = null, result = "";
            int positions = 0;
            Console.WriteLine("input text. only A-Z");
            line = Console.ReadLine().ToUpper();
            Console.WriteLine("input positions");
            positions = int.Parse(Console.ReadLine());
            List<char> newList = line.ToCharArray().ToList();

            newList.ForEach(x => result += GetNewChar(x, positions));
            Console.WriteLine(result);
            Console.ReadKey();
        }
        static char GetNewChar(char currChar, int translation)
        {
            if (currChar < 65 || currChar > 90) throw new ArgumentException();
            translation %= 26;
            if (currChar + translation < 65)
            {
                var restwert = -(currChar + translation - 65) - 1;
                return (char)(90 - restwert);
            }
            else if (currChar + translation > 90)
            {
                var restwert = (currChar + translation - 90 - 1);
                return (char)(65 + restwert);
            }
            return (char)(currChar + translation);
        }
    }
}
