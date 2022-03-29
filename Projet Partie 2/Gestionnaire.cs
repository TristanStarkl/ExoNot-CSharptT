using System.Collections.Generic;


namespace BankManagement
{
    internal class Gestionnaire
    {
        public string Name;
        public TypeGestionnaire Type;
        public int NbTransactions;
        List<Account> ListAccounts;

        public Gestionnaire(string name, TypeGestionnaire type, int nbTransactions)
        {
            Name = name;
            Type = type;
            NbTransactions = nbTransactions;
            ListAccounts = new List<Account>();
        }


    }
}