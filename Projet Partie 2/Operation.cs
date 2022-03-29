using System;

namespace BankManagement
{
    internal class Operation
    {
        public string Identifiant;
        public DateTime Date;
        public double Amount;
        public Object Entree;
        public Object Sortie;
        public TypeOperation TypeOfOperation;

        public Operation(string identifiant, DateTime date, double amount, object entree, object sortie, TypeOperation typeOfOperation)
        {
            Identifiant = identifiant;
            Date = date;
            Amount = amount;
            Entree = entree;
            Sortie = sortie;
            TypeOfOperation = typeOfOperation;
        }
    }
}