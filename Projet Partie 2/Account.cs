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
        // Todo: bouger la limite dans le gestionnaire
        internal Gestionnaire Manager;
        private double _temporalTransactionAmount = 2000d;
        private int _temporalTransactionDays = 7; 

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

        // Todo
        private bool CheckLimitAmountTransaction(double amount)
        {
            return Manager.CheckLimitAmountTransaction(amount);
        }

        /// <summary>
        /// ToDO Implémenter la méthode qui vérifie la temporalité de chacune des transactions
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private bool CheckTemporalLimitTransaction(Transaction T)
        {
            return false;
        }

        /// <summary>
        /// Vérifie si les transactions sont faisables ou pas
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        internal bool CheckIfLimitIsReached(Transaction T)
        {
            return CheckTemporalLimitTransaction(T) && CheckLimitAmountTransaction(T.Amount);
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
