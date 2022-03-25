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


            PercolationSimulation ps = new PercolationSimulation();
            Console.WriteLine(ps.PercolationValue(6));
            PclData resultat = ps.MeanPercolationValue(100, 80);

            Console.WriteLine("Moyenne");
            Console.WriteLine(resultat.Mean);
            Console.WriteLine("écart type");
            Console.WriteLine(resultat.RelativeStd);


            string res = Console.ReadLine();

        }
    }
}
