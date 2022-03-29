using BankManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement
{
    public class Transaction
    {
        public double Amount;

        public Account From;

        public Account To;

        public string Name;

        public Transaction(string name, double amount, Account from, Account to)
        {
            Name = name;
            Amount = amount;
            From = from;
            To = to;
        }

        /// <summary>
        /// Effectue une transaction, renvoie l'état
        /// </summary>
        /// <returns></returns>
        public string Make()
        {
            if (From == null || To == null)
                return Status.KO;
            if (From.identifiant == To.identifiant)
                return Status.KO;
            if (Amount <= 0)
                return Status.KO;
            if (From.DoesTheAmountIsSuperiorToTheSolde(Amount))
                return Status.KO;
            if (From.CheckIfLimitIsReached(Amount))
                return Status.KO;
            // Si on peut le faire, alors
            From.Withdraw(Amount);
            To.Deposit(Amount);
            From.AddNewTransaction(this);
            To.AddNewTransaction(this);
            return Status.OK;
        }

        /// <summary>
        /// Override de l'écriture de la transaction
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name}: transfert de {Amount} de {From.identifiant} à {To.identifiant}";
        }
    }
}