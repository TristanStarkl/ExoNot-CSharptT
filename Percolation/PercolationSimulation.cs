using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    public struct PclData
    {
        /// <summary>
        /// Moyenne 
        /// </summary>
        public double Mean { get; set; }
        /// <summary>
        /// Ecart-type
        /// </summary>
        public double StandardDeviation { get; set; }

        public double RelativeStd { get; set; }
    }

    public class PercolationSimulation
    {
        public PclData MeanPercolationValue(int size, int t)
        {
            if (t <= 0)
                throw new ArgumentException("le nombre d'essai ne peut être inférieur à 0");
            if (size <= 0)
                throw new ArgumentException("La taille ne peut être inférieure à 0");

            PclData resultat = new PclData();
            List<double> listRes = new List<double>();
            double totalVariance = 0d;

            for (int i = 0; i < t; i++)
                listRes.Add(this.PercolationValue(size));

            resultat.Mean = listRes.Average();
            foreach (int time in listRes)
                totalVariance += (time - resultat.Mean) * (time - resultat.Mean);

            resultat.StandardDeviation = Math.Sqrt(totalVariance);
            // Mean ne peut pas être égal à 0 parce qu'on a forcément plus de 0 cases ouvertes
            resultat.RelativeStd = resultat.StandardDeviation / resultat.Mean;
            return resultat;
        }

        public double PercolationValue(int size)
        {
            Percolation p = new Percolation(size);
            Random rand = new Random();
            double nbOpenCases = 0;
            int i, j;

            while (!p.Percolate())
            {
                do
                {
                    i = rand.Next(0, size);
                    j = rand.Next(0, size);
                }
                while (p.IsOpen(i, j));
                p.Open(i, j);
                nbOpenCases++;
            }

            return nbOpenCases / (size * size);
        }
    }
}
