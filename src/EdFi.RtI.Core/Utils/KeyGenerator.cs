using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdFi.RtI.Core.Utils
{
    public static class KeyGenerator
    {
        private static readonly string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random random = new Random();

        public static string New(int length)
        {
            return new string(Enumerable
                .Repeat(characters, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}
