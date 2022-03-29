using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement
{
    public class Account
    {
        public string Identifiant { get; set; }

        private List<Transaction> _lastTransactions;
        private double _solde;
        private double _limitTransactions = 1000d;

        public Account(string identifiant, double solde = 0)
        {
            Identifiant = identifiant;
            _solde = solde;
            _lastTransactions = new List<Transaction>();
        }

        internal double GetSolde()
        {
            return _solde;
        }

        internal bool CheckIfLimitIsReached(double amount)
        {
            double sumLastTransactions = 0;
            if (_lastTransactions.Count >= 1)
            {
                for (int i = _lastTransactions.Count - 1; i > 0 && i > _lastTransactions.Count - 10; i--)
                    sumLastTransactions += _lastTransactions[i].Amount;
            }

            sumLastTransactions += amount;
            return sumLastTransactions > _limitTransactions;
        }

        /// <summary>
        /// Vérifie si une transaction est faisable ou pas
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        internal virtual bool DoesTheAmountIsSuperiorToTheSolde(double amount)
        {
            return amount > _solde;
        }

        /// <summary>
        /// Retire le montant de _solde
        /// </summary>
        /// <param name="amount"></param>
        internal void Withdraw(double amount)
        {
            _solde -= amount;
        }


        internal void Deposit(double amount)
        {
            _solde += amount;
        }

        internal void AddNewTransaction(Transaction t)
        {
            _lastTransactions.Add(t);
        }

        public override string ToString()
        {
            return $"Compte numéro {Identifiant}: {_solde} euros";
        }
    }
}
