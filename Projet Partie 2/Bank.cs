using System.Collections.Generic;
using System.IO;
using System;

namespace BankManagement
{
    internal class Bank
    {


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
                                    throw new Exception($"Deux comptes au nom identiques {ges.Name}");
                            }
                            listColumns[2] = listColumns[2].Replace(".", ",");
                            nbTransactions = 0;
                            int.TryParse(listColumns[2], out nbTransactions);
                            if (listColumns[1] == TypeGestionnaire.PARTICULIER && nbTransactions > 0)
                                resultat.Add(new Particulier(listColumns[0], nbTransactions));
                            else if (listColumns[1] == TypeGestionnaire.ENTREPRISE && nbTransactions > 0)
                                resultat.Add(new Enterprise(listColumns[0], nbTransactions));
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

        public static List<Operation> GetAllOperations(string pathAcctFile, string pathTransactionFile)
        {
            List<Operation> resultat = new List<Operation>();
            string[] listColumns;
            Operation temp;

            using (StreamReader readerAcct = new StreamReader(pathAcctFile))
            {
                // D'abord on lit au moins une fois le AcctFile
                

                using (StreamReader readerTransaction = new StreamReader(pathTransactionFile))
                {

                }

            }

            return resultat;

        }

        /// <summary>
        /// Renvoie une liste de compte du fichier path
        /// </summary>
        /// <param name="path">Le chemin du fichier</param>
        /// <returns></returns>
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
                                if (acc.Identifiant == listColumns[0])
                                    throw new Exception($"Deux comptes au nom identiques {acc.Identifiant}");
                            }
                            listColumns[1] = listColumns[1].Replace(".", ",");
                            initialAmount = 0;
                            double.TryParse(listColumns[1], out initialAmount);
                            if (initialAmount >= 0)
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

        /// <summary>
        /// Vérifie que la transaction identifiant existe dans la liste de transactions
        /// </summary>
        /// <param name="list"></param>
        /// <param name="identifiant"></param>
        /// <returns></returns>
        public static bool CheckIfTransactionExist(List<Transaction> list, string identifiant)
        {
            foreach (Transaction t in list)
            {
                if (t.Name == identifiant)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Renvoie la liste des transactions adaptés
        /// </summary>
        /// <param name="path"></param>
        /// <param name="listAccount"></param>
        /// <returns></returns>
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
                                if (acc.Identifiant == listColumns[2])
                                {
                                    from = acc;
                                }
                                if (acc.Identifiant == listColumns[3])
                                {
                                    to = acc;
                                }
                            }

                            // Vérifie l'unicité de la transaction
                            if (!Bank.CheckIfTransactionExist(resultat, listColumns[0]))
                            {
                                amount = float.Parse(listColumns[1].Replace(".", ","));

                                resultat.Add(new Transaction(listColumns[0], amount, from, to));
                            }
                            else // Si la transaction n'est pas unique, on la déclare comme étant fausse
                            {
                                resultat.Add(new Transaction(listColumns[0], 0, null, null));
                            }
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