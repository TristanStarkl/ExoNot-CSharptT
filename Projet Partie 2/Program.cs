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
            string acctPath = "Comptes_1.txt";
            string trxnPath = "Transactions_1.txt";
            string gestPath = "Gestionnaires_1.txt";

            string sttsAcctPath = "StatutOpe_1.txt";
            string sttsTrxnPath = "StatutTra_1.txt";
            string mtrlPath = "Metrologie_1.txt";

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

            bank = new Bank(listOperations,listGestionnaires, files);
            bank.Compute();




            // Keep the console window open
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
