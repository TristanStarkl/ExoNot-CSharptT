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
        public string Name;
        public DateTime DateTransaction;
        public Account From;
        public Account To;

        public Transaction(string name, DateTime dateTransaction, double amount, Account from, Account to)
        {
            Name = name;
            DateTransaction = dateTransaction;
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
            if (From.Identifiant == To.Identifiant)
                return Status.KO;
            if (Amount <= 0)
                return Status.KO;
            if (From.DoesTheAmountIsSuperiorToTheSolde(Amount))
                return Status.KO;
            if (From.CheckIfLimitIsReached(this))
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
            string fIdentifiant = "compte inexistant";
            string tIdentifiant = "compte inexistant";
            if (From != null)
                fIdentifiant = From.Identifiant;
            if (To != null)
                tIdentifiant = To.Identifiant;

            return $"{Name}: transfert de {Amount} de {fIdentifiant} à {tIdentifiant}";
        }
    }
}