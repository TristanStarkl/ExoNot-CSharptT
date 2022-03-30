using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace BankManagement
{
    internal class Bank
    {
        public List<Operation> Operations { get; set; }
        public FileHandling Files { get; set; }
        public Dictionnaire Listes;

        public Bank(List<Operation> operations, List<Gestionnaire> gestionnaires, FileHandling files)
        {
            Operations = operations;

            Listes = new Dictionnaire(gestionnaires);
            Files = files;
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
                while (!readerAcct.EndOfStream)
                {
                    listColumns = readerAcct.ReadLine().Split(';');
                    if (listColumns.Length == 5)
                    {
                        try
                        {
                            temp = new OperationCompte(listColumns);
                            resultat.Add(temp);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }

            using (StreamReader readerTransaction = new StreamReader(pathTransactionFile))
            {
                while (!readerTransaction.EndOfStream)
                {
                    listColumns = readerTransaction.ReadLine().Split(';');
                    if (listColumns.Length == 5)
                    {
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
            }

            resultat.Sort();

            return (resultat);
        }

        public void Compute()
        {
            using (StreamWriter account = new StreamWriter(Files.SortieAccount))
            {
                using (StreamWriter transaction = new StreamWriter(Files.SortieTransactions))
                {
                    using (StreamWriter metrologie = new StreamWriter(Files.SortiesStats))
                    {
                        Files.AddStreamWriters(account, transaction, metrologie);
                        Listes.FH = Files;
                        foreach (Operation operation in Operations)
                            operation.Execute(Listes);
                        Listes.Compute();
                    }
                }
            }
        }
    }
}