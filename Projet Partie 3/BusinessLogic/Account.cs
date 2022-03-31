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
        protected int _temporalTransactionAmount;
        protected const int _temporalTransactionDays = 7;
        private AccountType _typeOfAccount;
        protected DateTime LastTimeWeCalculatedInterest;
        public bool CanMakeExteriorVirement;
        public bool CanWithdrawMoney;
        public DateTime DateOuv;

        public double InterestRate;
        public double Interest;

        public Account(string identifiant, DateTime dateOuv, double solde = 0, AccountType TypeCompte = AccountType.COMPTE)
        {
            Identifiant = identifiant;
            _solde = solde;
            _lastTransactions = new List<Transaction>();
            _typeOfAccount = TypeCompte;
            InterestRate = 2;
            CanMakeExteriorVirement = true;
            CanWithdrawMoney = true;
            Interest = 0d;
            _temporalTransactionAmount = 1000;
            DateOuv = dateOuv;
        }

        internal AccountType GetAccountType()
        {
            return _typeOfAccount;
        }
        internal double GetSolde()
        {
            return _solde;
        }

        internal virtual bool CanWeCloseTheAccount(DateTime date)
        {
            return true;
        }

        #region Interest
        internal virtual void CalculateInterest(DateTime date)
        {
            // On ne fais rien ici
        }

        #endregion

        #region TransactionLimit
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
            DateTime minDate = T.DateTransaction - TimeSpan.FromDays(_temporalTransactionDays);
            double amount = 0d;

            for (int i = _lastTransactions.Count - 1; i >= 0 && _lastTransactions[i].DateTransaction >= minDate; i--)
            {
                amount += _lastTransactions[i].Amount;
            }

            amount += T.Amount;
            return amount > _temporalTransactionAmount;
        }

        /// <summary>
        /// Vérifie si les transactions sont faisables ou pas
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        internal virtual bool CheckIfLimitIsReached(Transaction T)
        {
            return CheckTemporalLimitTransaction(T) || CheckLimitAmountTransaction(T.Amount);
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
        #endregion

        #region Fees
        internal virtual bool DoesFeesApply(Account account2)
        {
            if (account2.Manager != null && Manager != null)
                return account2.Manager.Name != Manager.Name;

            return false;
        }

        internal virtual double CalculateFees(Transaction t)
        {
            if (!DoesFeesApply(t.To))
                return 0d;
            return Manager.GetAmountFees(t);
        }
        #endregion


        #region Operations

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
            if (Manager != null)
                Manager.AddNewTransaction(t);
        }
        #endregion

        public override string ToString()
        {
            return $"Compte numéro {Identifiant} {_typeOfAccount}: {_solde} euros";
        }
    }

    public class AccountJeune : Account
    {
        public AccountJeune(string identifiant, DateTime date, int age, double solde = 0) : base(identifiant,date, solde, AccountType.JEUNE)
        {
            if (age < 10 || age > 17)
                throw new ArgumentOutOfRangeException($"L'age de la personne est invalide: {age}");
            Deposit(10 * age);
            _temporalTransactionAmount *= (int)(age / 18);
            InterestRate = 0;
        }
    }
    public class AccountLivret : Account
    {
        public AccountLivret(string identifiant, DateTime date, double solde = 0) : base(identifiant, date, solde, AccountType.LIVRET)
        {
            InterestRate = 2;
            CanWithdrawMoney = false;
        }

        internal override void CalculateInterest(DateTime date)
        {
            TimeSpan nbDays = date - LastTimeWeCalculatedInterest;
            double InterestThisTime = (GetSolde() * (int)(nbDays.TotalDays / 365d));
            Deposit(InterestThisTime);
            Interest += InterestThisTime;
            LastTimeWeCalculatedInterest = date;
        }
    }

    public class AccountTerme : Account
    {
        public AccountTerme(string identifiant, DateTime date, double solde = 0) : base(identifiant, date, solde, AccountType.TERME)
        {
            if (solde < 200)
                throw new ArgumentException("Erreur, le solde minimum pour un compte à terme est de 200 euros");
            InterestRate = 5;
            CanMakeExteriorVirement = false;
            CanWithdrawMoney = false;
        }

        /// <summary>
        /// TODO: L'intérêt spécial chaque année de 10%
        /// </summary>
        /// <param name="date"></param>
        internal override void CalculateInterest(DateTime date)
        {
            TimeSpan nbDays = date - LastTimeWeCalculatedInterest;
            double InterestThisTime = (GetSolde() * (int)(nbDays.TotalDays / 365d));
            Deposit(InterestThisTime);
            Interest += InterestThisTime;
            LastTimeWeCalculatedInterest = date;
        }

        internal override bool CanWeCloseTheAccount(DateTime date)
        {
            TimeSpan timediff = date - DateOuv;
            return timediff.TotalDays > (5 * 365);
        }

    }

}
