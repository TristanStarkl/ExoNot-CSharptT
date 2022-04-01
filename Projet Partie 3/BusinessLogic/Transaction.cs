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
            // Vérification de l'existence
            if (From == null || To == null)
                throw new ArgumentNullException();

            if (Name == "3")
                Console.WriteLine("Point d'arrêt");
            // Intérêts
            From.CalculateInterest(DateTransaction);
            To.CalculateInterest(DateTransaction);

            // Continuation des vérifications
            if (From.Identifiant == To.Identifiant)
                throw new ArgumentOutOfRangeException($"TRANSACTION {Name}:Virement dans le même compte");
            if (Amount <= 0)
                throw new ArgumentOutOfRangeException($"TRANSACTION {Name}: Montant inférieur à 0");
            if (From.DoesTheAmountIsSuperiorToTheSolde(Amount))
                throw new Exception($"TRANSACTION {Name}: Le montant est supérieur au solde");
            if (From.CheckIfLimitIsReached(this))
                throw new Exception($"TRANSACTION {Name}: Limite atteinte pour le compte débiteur ");
            // Est-ce que le compte peut faire un virement ?
            if (To.Identifiant == "0" && !From.CanMakeWithdrawal)
                throw new Exception($"TRANSACTION {Name}: Impossible de retirer de l'argent");
            if (!From.CanMakeExteriorVirement)
                throw new Exception($"TRANSACTION {Name}: Impossible d'émettre des virements");

            // Si on peut le faire, alors
            From.Withdraw(Amount);
            double fees = From.CalculateFees(this);
            To.Deposit(Amount - fees);
            if (From.Manager != null)
                From.Manager.TotalFees += fees;
            Console.WriteLine($"La transaction numéro {Name} de {From.Identifiant} a {To.Identifiant} a entraîné {fees} euros de frais");
            From.AddNewTransaction(this);
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