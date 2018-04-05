using Annytab.Stemmer;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            IStemmer stemmer = new TestStemmer();

            string[] test = new string[]
            {
               "adres",
               "aires"
            };

            foreach (var item in test)
            {
                Console.WriteLine(stemmer.GetSteamWord(item));
            }

            Console.ReadLine();
        }
    }
}
