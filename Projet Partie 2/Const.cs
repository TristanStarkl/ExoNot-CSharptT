using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BankManagement
{
    public class Status
    {
        public const string OK = "OK";
        public const string KO = "KO";
    }

    internal class Dictionnaire
    {
        public List<Account> Accounts;
        public List<Gestionnaire> Gestionnaires;
        public FileHandling FH;
        public uint NbComptes = 0;
        public uint NbTransactions = 0;
        public uint NbReussites = 0;
        public uint NbEchecs = 0;
        public double MontantReussite = 0d;


        public Dictionnaire(List<Gestionnaire> gestionnaires)
        {
            Accounts = new List<Account>();
            Environnement e = new Environnement("0");
            Accounts.Add(e);
            Gestionnaires = gestionnaires;
        }

        public void Compute()
        {
            FH.SortiesStatsF.WriteLine("Statistiques");
            FH.SortiesStatsF.WriteLine($"Nombre de comptes : {NbComptes}");
            FH.SortiesStatsF.WriteLine($"Nombre de transactions : {NbTransactions}");
            FH.SortiesStatsF.WriteLine($"Nombre de réussites : {NbReussites}");
            FH.SortiesStatsF.WriteLine($"Nombre d'échecs : {NbEchecs}");
            FH.SortiesStatsF.WriteLine($"Montant total des réussites : {MontantReussite}");
            FH.SortiesStatsF.WriteLine(" ");
            FH.SortiesStatsF.WriteLine("Frais de gestions :");
            foreach (Gestionnaire g in Gestionnaires)
            {
                FH.SortiesStatsF.WriteLine($"{g.Name} : {g.MontantFrais} euros");
            }

        }
    }

    public class FileHandling
    {
        public string SortieAccount;
        public string SortieTransactions;
        public string SortiesStats;
        public StreamWriter SortieAccountF;
        public StreamWriter SortieTransactionsF;
        public StreamWriter SortiesStatsF;

        public FileHandling(string sortieAccount, string sortieTransactions, string sortiesStats)
        {
            SortieAccount = sortieAccount;
            SortieTransactions = sortieTransactions;
            SortiesStats = sortiesStats;
        }

        public void AddStreamWriters(StreamWriter a, StreamWriter t, StreamWriter s)
        {
            SortieAccountF = a;
            SortiesStatsF = s;
            SortieTransactionsF = t;
        }

    }

    public enum TypeFrais
    {
        PERCENTAGE,
        FIXE
    }

    public class TypeGestionnaire
    {
        public const string PARTICULIER = "Particulier";
        public const string ENTREPRISE  = "Entreprise";
    }

    enum TypeOperation
    {
        TRANSACTION,
        COMPTE
    }
}
