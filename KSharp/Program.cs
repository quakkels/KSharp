using KSharp.Parser;
using System.Collections.Generic;

namespace KSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parsr(new Dictionary<char, int> {
                { '<', 10 },
                { '+', 20 },
                { '-', 30 },
                { '*', 40 }
            });

            var driver = new Driver(parser);

            driver.MainLoop();
        }
    }
}
