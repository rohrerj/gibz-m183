using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace SelfMadeSessions
{
    public class _Session : Dictionary<string,object>
    {
        private static Dictionary<string, _Session> instances = new Dictionary<string, _Session>();//cleanup of sessions not implemented
        private _Session()
        {

        }
        public static string CreateSession()
        {
            _Session session = new _Session();
            string key = GetRandomKey();
            instances.Add(key, session);
            return key;
        }
        public static _Session GetSession(string key)
        {
            if(Exists(key))
            {
                return instances[key];
            }
            else
            {
                return null;
            }
        }
        public static void DeleteSession(string key)
        {
            if(Exists(key))
            {
                instances.Remove(key);
            }
        }
        public static bool Exists(string key)
        {
            return instances.ContainsKey(key);
        }
        private static string GetRandomKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 64).Select(s => s[NextInt(s.Length)]).ToArray());
        }
        private static int NextInt(int max)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[4];

            rng.GetBytes(buffer);
            int result = BitConverter.ToInt32(buffer, 0);

            return new Random(result).Next(0, max);
        }
        public static HttpCookie GetCoookie(string key)
        {
            HttpCookie cookie = new HttpCookie("M183-Session-Cookie")
            {
                HttpOnly = true,
                Value = key,
                Expires = DateTime.MinValue
            };
            return cookie;
        }
    }
}