using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace BankManagement
{
    internal class Bank
    {
        public List<Operation> ListOperations { get; set; }
        public FileHandling FH { get; set; }
        public Dictionnaire Listes;

        public Bank(List<Operation> operations, List<Gestionnaire> gestionnaires, FileHandling files)
        {           
            ListOperations = operations;

            Listes = new Dictionnaire(gestionnaires);
            FH = files;
        }

        /// <summary>
        /// Surcharge pour virtual
        /// </summary>
        public Bank()
        {
        }

        public static List<Gestionnaire> ReadGestionnaireFile(string path)
        {
            List<Gestionnaire> resultat = new List<Gestionnaire>();
            string[] listColumns;
            int nbTransactions;


            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        listColumns = reader.ReadLine().Split(';');
                        if (listColumns.Length == 3)
                        {
                            // On check que le nom est bien unique
                            foreach (Gestionnaire ges in resultat)
                            {
                                if (ges.Name == listColumns[0])
                                    throw new Exception($"Deux gestionnaires au nom identiques {ges.Name}");
                            }
                            listColumns[2] = listColumns[2].Replace(".", ",");
                            nbTransactions = 0;
                            int.TryParse(listColumns[2], out nbTransactions);
                            if (listColumns[1] == TypeGestionnaire.PARTICULIER && nbTransactions >= 0)
                                resultat.Add(new Particulier(listColumns[0], nbTransactions));
                            else if (listColumns[1] == TypeGestionnaire.ENTREPRISE && nbTransactions >= 0)
                                resultat.Add(new Enterprise(listColumns[0], nbTransactions));
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            return resultat;
        }

        public static List<Operation> GetAllOperations(string pathAcctFile, string pathTransactionFile)
        {
            List<Operation> resultat = new List<Operation>();
            string[] listColumns;
            Operation temp;

            using (StreamReader readerAcct = new StreamReader(pathAcctFile))
            {
                while (!readerAcct.EndOfStream)
                {
                    listColumns = readerAcct.ReadLine().Split(';');
                    try
                    {
                        temp = new OperationCompte(listColumns);
                        resultat.Add(temp);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            using (StreamReader readerTransaction = new StreamReader(pathTransactionFile))
            {
                while (!readerTransaction.EndOfStream)
                {
                    listColumns = readerTransaction.ReadLine().Split(';');
                    try
                    {
                        temp = new OperationTransaction(listColumns);
                        resultat.Add(temp);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            resultat.Sort();

            return (resultat);
        }

        public void Compute()
        {
            using (StreamWriter account = new StreamWriter(FH.SortieAccount))
            {
                using (StreamWriter transaction = new StreamWriter(FH.SortieTransactions))
                {
                    using (StreamWriter metrologie = new StreamWriter(FH.SortiesStats))
                    {
                        FH.AddStreamWriters(account, transaction, metrologie);
                        Listes.FH = FH;
                        foreach (Operation operation in ListOperations)
                            operation.Execute(Listes);
                        Listes.Compute();
                    }
                }
            }
        }
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
                FH.SortiesStatsF.WriteLine($"{g.Name} : {g.TotalFees} euros");
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


}