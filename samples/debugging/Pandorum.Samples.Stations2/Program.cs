using Pandorum.Samples.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Samples.Stations2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Logging in...");
            
            PandoraHelpers.Login(async client =>
            {
                Console.WriteLine("Finished logging in.");
            });
        }
    }
}
