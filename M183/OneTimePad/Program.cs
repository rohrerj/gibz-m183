using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneTimePad
{
    class Program
    {
        static void Main(string[] args)
        {
            string msg = Console.ReadLine();
            string key = Console.ReadLine();
            string encrypted = Encrypt(msg, key);
            Console.WriteLine(encrypted);
            string decrypted = Decrypt(encrypted, key);
            Console.WriteLine(decrypted);
            Console.ReadKey();
        }
        static string Encrypt(string msg, string key)
        {
            string newText = "";
            if (msg.Length == 0 || key.Length == 0)
            {
                return null;
            }
            msg = msg.ToUpper();
            key = key.ToUpper();
            while (key.Length < msg.Length)
            {
                key += key;
            }
            for (int i = 0; i < msg.Length; i++)
            {
                int newChar = (AlphabeticPosition(msg[i]) + AlphabeticPosition(key[i])) % 26;
                newText += (char)(newChar == 0 ? 90 : newChar + 64);
            }
            return newText;
        }
        static string Decrypt(string msg, string key)
        {
            string newText = "";
            if (msg.Length == 0 || key.Length == 0)
            {
                return null;
            }
            msg = msg.ToUpper();
            key = key.ToUpper();
            while (key.Length < msg.Length)
            {
                key += key;
            }
            for (int i = 0; i < msg.Length; i++)
            {
                int newChar = (AlphabeticPosition(msg[i]) - AlphabeticPosition(key[i]));
                if (newChar < 0)
                {
                    newChar += 26;
                }
                newText += (char)(newChar + 64);
            }
            return newText;
        }
        static int AlphabeticPosition(char c)
        {
            return (c - 64);
        }
    }
}
