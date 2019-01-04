using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vigenere
{
    class Program
    {
        static void Main(string[] args)
        {
            string text;
            string key;
            string result = "";
            string decode = "";

            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            char[][] x = new char[26][];
            for(int i = 0; i < 26; i++)
            {
                x[i] = new char[26];
                for(int y = 0; y < 26; y++)
                {
                    int num = (AlphabeticPosition(chars[y]) + i) % 26;
                    if (num == 0)
                    {
                        num = 26;
                    }
                    num += 64;
                    x[i][y] = (char)num;
                }
            }


            for (int i = 0; i < 26; i++)
            {
                for (int y = 0; y < 26; y++)
                {
                    Console.Write(x[i][y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Enter text [A-Z]");
            text = Console.ReadLine().ToUpper();
            Console.WriteLine("Enter key [A-Z]");
            key = Console.ReadLine().ToUpper();

            while (key.Length < text.Length)
            {
                key += key;
            }

            for (int i = 0; i < text.Length; i++)
            {
                var newChar = x[AlphabeticPosition(key[i])-1][AlphabeticPosition(text[i])-1];
                result += newChar;
            }

            Console.WriteLine("Result:");
            Console.WriteLine(result);
            
            for(int i = 0; i < result.Length; i++)
            {
                var c = x[AlphabeticPosition(key[i])-1];
                int index = Array.IndexOf(c, result[i]);
                decode += chars[index];
            }
            Console.WriteLine("Decode with given key:");
            Console.WriteLine(decode);
            
            Console.ReadKey();
        }
        static int AlphabeticPosition(char c)
        {
            return (c - 64);
        }
    }
}
