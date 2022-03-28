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
            string sttsPath = "Statut_1.txt";

            //TODO: Votre implémentation
            List<Account> acc = Bank.ReadAccountFile(acctPath);
            List<Transaction> listT = Bank.ReadTransactionFile(trxnPath, acc);

            foreach (Account ac in acc)
                Console.WriteLine(ac);
            foreach (Transaction t in listT)
                Console.WriteLine(t);
            Bank.HandleTransactions(listT, sttsPath);
            foreach (Account ac in acc)
                Console.WriteLine(ac);

            // Keep the console window open
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
