using System;
using System.Collections.Generic;


namespace BankManagement
{
    internal abstract class Gestionnaire
    {
        public string Name;
        protected string Type;

        public int NbTransactions;
        private double _limitTransactionsAmount = 1000d;
        private List<Transaction> _ListTransactions;
        public TypeFrais TypeOfFees;
        public double MontantFrais;


        public Gestionnaire(string name, int nbTransactions, TypeFrais typeOfFees, double montantFrais)
        {
            Name = name;
            NbTransactions = nbTransactions;
            TypeOfFees = typeOfFees;
            MontantFrais = montantFrais;
            _ListTransactions = new List<Transaction>();
        }

        public abstract double GetAmountFees(Transaction lastTransaction);

        public void AddNewTransaction(Transaction T)
        {
            _ListTransactions.Add(T);
        }

        public override string ToString()
        {
            return $"Gestionnaire {Name} ({Type}): {NbTransactions}";
        }

        internal bool CheckLimitAmountTransaction(double amount)
        {
            double sumLastTransactions = 0;
            if (_ListTransactions.Count >= 1)
            {
                for (int i = _ListTransactions.Count - 1; i > 0 && i > _ListTransactions.Count - NbTransactions; i--)
                    sumLastTransactions += _ListTransactions[i].Amount;
            }

            sumLastTransactions += amount;
            return sumLastTransactions > _limitTransactionsAmount;
        }
    }

    internal class Enterprise : Gestionnaire
    {
        public Enterprise(string name, int nbTransactions) : base(name, nbTransactions, TypeFrais.FIXE, 10d)
        {
            Type = TypeGestionnaire.ENTREPRISE;
        }

        public override double GetAmountFees(Transaction lastTransaction)
        {
            throw new System.NotImplementedException();
        }
    }
    internal class Particulier : Gestionnaire
    {
        public Particulier(string name, int nbTransactions) : base(name, nbTransactions, TypeFrais.PERCENTAGE, 0.01d)
        {
            Type = TypeGestionnaire.PARTICULIER;
        }

        public override double GetAmountFees(Transaction lastTransaction)
        {
            throw new System.NotImplementedException();
        }
    }
}