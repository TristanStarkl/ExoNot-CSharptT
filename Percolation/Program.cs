using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    class Program
    {
        static void Main(string[] args)
        {
            Percolation p = new Percolation(6);

            p.Display();
            Console.WriteLine(p.Percolate());
            string res = Console.ReadLine();

        }
    }
}
