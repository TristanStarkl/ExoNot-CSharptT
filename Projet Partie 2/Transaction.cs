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
        /// Effectue une transaction, renvoie les frais
        /// </summary>
        /// <returns></returns>
        public void Make()
        {
            if (From == null || To == null)
                throw new ArgumentNullException();
            if (From.Identifiant == To.Identifiant)
                throw new ArgumentOutOfRangeException("Virement dans le même compte");
            if (Amount <= 0)
                throw new ArgumentOutOfRangeException();
            if (From.DoesTheAmountIsSuperiorToTheSolde(Amount))
                throw new Exception();
            if (From.CheckIfLimitIsReached(this))
                throw new Exception();

            // Si on peut le faire, alors
            From.Withdraw(Amount);
            double fees = From.CalculateFees(this);
            To.Deposit(Amount - fees);
            From.Manager.TotalFees += fees;
            From.AddNewTransaction(this);
            To.AddNewTransaction(this);
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