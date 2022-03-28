using System.Collections.Generic;
using System.IO;
using System;

namespace BankManagement
{
    internal class Bank
    {

        public Bank()
        {
        }

        public static List<Account> ReadAccountFile(string path)
        {
            List<Account> resultat = new List<Account>();
            Environnement exterior = new Environnement("0");
            string[] listColumns;
            double initialAmount;

            resultat.Add(exterior);
            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        listColumns = reader.ReadLine().Split(';');
                        if (listColumns.Length == 2)
                        {
                            // On check que le nom est bien unique
                            foreach (Account acc in resultat)
                            {
                                if (acc.identifiant == listColumns[0])
                                    throw new Exception($"Deux comptes au nom identiques {acc.identifiant}");
                            }
                            listColumns[1] = listColumns[1].Replace(",", ".");
                            initialAmount = 0;
                            double.TryParse(listColumns[1], out initialAmount);
                            resultat.Add(new Account(listColumns[0], initialAmount));
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            return resultat;
        }

        public static List<Transaction> ReadTransactionFile(string path, List<Account> listAccount)
        {
            List<Transaction> resultat = new List<Transaction>();
            string[] listColumns;
            float amount;
            Account from;
            Account to;

            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    listColumns = reader.ReadLine().Split(';');
                    if (listColumns.Length == 4)
                    {
                        try
                        {
                            from = null;
                            to = null;
                            // On recherche maintenant si la matière existe déjà
                            foreach (Account acc in listAccount)
                            {
                                if (acc.identifiant == listColumns[2])
                                {
                                    from = acc;
                                }
                                if (acc.identifiant == listColumns[3])
                                {
                                    to = acc;
                                }
                            }
                            amount = float.Parse(listColumns[1].Replace(".", ","));

                            resultat.Add(new Transaction(listColumns[0], amount, from, to));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Erreur parsing {path} {e}");
                        }
                    }
                }
            }
            return resultat;
        }

        public static void HandleTransactions(List<Transaction> listT, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (Transaction t in listT)
                {
                    writer.WriteLine(t.Name + ";" + t.Make());
                }
            }
        }
    }
}