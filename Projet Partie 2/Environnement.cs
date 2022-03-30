using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement
{
    public class Environnement : Account
    {

        private List<Transaction> _lastTransactions;
        private double _solde;

        public Environnement(string identifiant, double solde = 0) : base(identifiant)
        {
            Identifiant = identifiant;
            _solde = double.MaxValue;
            _lastTransactions = new List<Transaction>();
        }

        internal new double GetSolde()
        {
            return double.MaxValue;
        }

        internal override bool DoesTheAmountIsSuperiorToTheSolde(double amount)
        {
            return false;
        }

        internal new bool CheckIfLimitIsReached(Transaction T)
        {
            return false;
        }

        internal new void Withdraw(double amount)
        {
        }

        internal new void Deposit(double amount)
        {
        }


    }
}
