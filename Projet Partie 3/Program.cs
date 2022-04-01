using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i != 2; i++)
            {
                Console.WriteLine("=======================================================================");
                Console.WriteLine($"--------------------- TRAITEMENT DES FICHIERS NUMERO {i} --------------");
                Console.WriteLine("=======================================================================");
                string acctPath = $"Comptes_{i}.txt";
                string trxnPath = $"Transactions_{i}.txt";
                string gestPath = $"Gestionnaires_{i}.txt";

                string sttsAcctPath = $"StatutOpe_{i}.txt";
                string sttsTrxnPath = $"StatutTra_{i}.txt";
                string mtrlPath = $"Metrologie_{i}.txt";

                FileHandling files = new FileHandling(sttsAcctPath, sttsTrxnPath, mtrlPath);
                Bank bank;

                //TODO: Votre implémentation
                List<Gestionnaire> listGestionnaires = Bank.ReadGestionnaireFile(gestPath);
                List<Operation> listOperations = Bank.GetAllOperations(acctPath, trxnPath);

                Console.WriteLine("------------ GESTIONNAIRES ------------");
                foreach (Gestionnaire ges in listGestionnaires)
                    Console.WriteLine(ges);

                Console.WriteLine("------------ OPERATIONS ------------");
                foreach (Operation ope in listOperations)
                    Console.WriteLine(ope);

                Console.WriteLine("------------ COMPUTATION -----------");
                bank = new Bank(listOperations, listGestionnaires, files);
                bank.Compute();

                Console.WriteLine("------------ FRAIS DEXECUTION -----------");
                foreach (Gestionnaire ges in bank.Listes.Gestionnaires)
                    Console.WriteLine($"GESTIONNAIRE: {ges.Name}: {ges.TotalFees} euros");
                Console.WriteLine("------------ SOLDES DES COMPTES -----------");
                foreach (Account acc in bank.Listes.Accounts)
                    Console.WriteLine($"COMPTE: {acc.Identifiant}: {acc.GetSolde()} euros");
                Console.WriteLine("------------ INTERETS GENERES -----------");
                foreach (Account acc in bank.Listes.Accounts)
                    Console.WriteLine($"COMPTE: {acc.Identifiant}: {acc.InterestMade} euros");

            }


            // Keep the console window open
            Console.WriteLine("------------ FIN -----------");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
