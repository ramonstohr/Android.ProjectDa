using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
           string text =  Console.ReadLine();
            bool pali = checkPalindrome(text);
            Console.WriteLine(text + " is pali: " + pali); ;
            Console.ReadLine();
        }


        public static bool checkPalindrome(string inputString)
        {

            char[] chars = inputString.ToCharArray();
            for (int i = 0; i < chars.Length / 2; i++)
            {
                if (chars[i] != chars[chars.Length - 1 - i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
