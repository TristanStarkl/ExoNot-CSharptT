using System;
using System.Collections.Generic;

namespace BankManagement
{
    internal class Operation : IComparable<Operation>
    {
        public string Identifiant;
        public DateTime Date;
        public double Amount;
        public string IdentifiantEntree;
        public string IdentifiantSortie;
        public TypeOperation Type;

        public int c;
        public AccountType TypeAccount;


        public Operation(string identifiant, DateTime date, double amount, string entree, string sortie, TypeOperation typeOfOperation)
        {
            Identifiant = identifiant;
            Date = date;
            Amount = amount;
            IdentifiantEntree = entree;
            IdentifiantSortie = sortie;
            Type = typeOfOperation;
        }

        protected Operation()
        {
        }

        public Operation(string[] listColumns, TypeOperation t)
        {
            if (listColumns.Length != 5)
                throw new ArgumentException("Mauvais formatage, 5 colonnes nécessaires");

            double amount;
            string tmp = listColumns[2].Replace('.', ',');
            DateTime date;
            Identifiant = listColumns[0];
            this.IdentifiantEntree = listColumns[3];
            IdentifiantSortie = listColumns[4];
            Type = t;
            if (String.IsNullOrEmpty(tmp) && t == TypeOperation.COMPTE)
                tmp = "0";

            if (string.IsNullOrEmpty(this.IdentifiantEntree))
                this.IdentifiantEntree = null;
                
            if (String.IsNullOrEmpty(IdentifiantSortie))
                IdentifiantSortie = null;

            if (!double.TryParse(tmp, out amount))
                throw new ArgumentException($"Le montant n'est pas valide pour l'opération {Identifiant} {Type} (montant reçu : {tmp})");

            Amount = amount;

            if (!DateTime.TryParse(listColumns[1], out date))
                throw new ArgumentException("La date n'est pas valide");
            Date = date;
        }

        public virtual void Execute(Dictionnaire bank)
        {
        }

        #region Compare

        public int CompareTo(Operation other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;

            // The temperature comparison depends on the comparison of
            // the underlying Double values.
            return Date.CompareTo(other.Date);
        }

        // Define the is greater than operator.
        public static bool operator >(Operation operand1, Operation operand2)
        {
            return operand1.CompareTo(operand2) > 0;
        }

        // Define the is less than operator.
        public static bool operator <(Operation operand1, Operation operand2)
        {
            return operand1.CompareTo(operand2) < 0;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(Operation operand1, Operation operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(Operation operand1, Operation operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }

        #endregion

        public override string ToString()
        {
            if (Type == TypeOperation.COMPTE)
                return $"OPERATION: COMPTE {Type}-{Identifiant}- DATE {Date} : {Amount} : GESTIONNAIRE {IdentifiantEntree} VERS {IdentifiantSortie}";
            else
                return $"OPERATION: TRANSACTION -{Identifiant}- DATE  {Date} : {Amount} EUROS DEPUIS  {IdentifiantEntree} VERS {IdentifiantSortie}";
        }

        protected Account DoesTheAccountExist(Dictionnaire bank, string name)
        {
            Account account = null;

            // On récupère le compte
            foreach (Account acc in bank.Accounts)
            {
                if (acc.Identifiant == name && acc.DateClo == null)
                {
                    account = acc;
                    break;
                }
            }
            if (account == null)
                throw new ArgumentException($"COMPTE {Identifiant} : Le compte n'existe pas :'{Identifiant}'");
            return account;
        }

        protected Gestionnaire DoesTheGestionnaireExist(Dictionnaire bank, string name)
        {
            Gestionnaire m = null;
            foreach (Gestionnaire g in bank.Gestionnaires)
            {
                if (g.Name == name)
                    m = g;
            }
            if (m == null)
                throw new ArgumentException($"COMPTE {Identifiant} : Le gestionnaire n'existe pas :'{name}'");
            return m;
        }
    }

    internal class OperationCompte : Operation
    {
        public OperationCompte(string[] listColumns)
        {
            if (listColumns.Length != 7)
                throw new ArgumentException("Mauvais formatage, 7 colonnes nécessaires");

            double amount;
            string type = listColumns[AccountFileDefinition.TYPE];
            int.TryParse(listColumns[AccountFileDefinition.AGE], out c);

            switch (type)
            {
                case "J":
                    TypeAccount = AccountType.JEUNE;
                    break;
                case "C":
                    TypeAccount = AccountType.COMPTE;
                    break;
                case "L":
                    TypeAccount = AccountType.LIVRET;
                    break;
                case "T":
                    TypeAccount = AccountType.TERME;
                    break;
                default:
                    TypeAccount = AccountType.DEFAUT;
                    break;
            }


            string tmp = listColumns[AccountFileDefinition.SOLDE_INITIAL].Replace('.', ',');
            DateTime date;
            Identifiant = listColumns[AccountFileDefinition.IDENTIFIANT];
            IdentifiantEntree = listColumns[AccountFileDefinition.IDENTIFIANT_ENTREE];
            IdentifiantSortie = listColumns[AccountFileDefinition.IDENTIFIANT_SORTIE];
            Type = TypeOperation.COMPTE;

            if (String.IsNullOrEmpty(tmp))
                tmp = "0";

            if (String.IsNullOrEmpty(IdentifiantEntree))
                IdentifiantEntree = null;

            if (String.IsNullOrEmpty(IdentifiantSortie))
                IdentifiantSortie = null;

            if (!double.TryParse(tmp, out amount))
                throw new ArgumentException($"Le montant n'est pas valide pour l'opération {Identifiant} {Type} (montant reçu : {tmp})");

            Amount = amount;

            if (!DateTime.TryParse(listColumns[AccountFileDefinition.DATE], out date))
                throw new ArgumentException("La date n'est pas valide");
            Date = date;
        }

        /// <summary>
        /// Ajout d'un nouveau compte
        /// TODO: EXTRAIRE LE SWITCH
        /// </summary>
        /// <param name="bank"></param>
        private void _AddNewAccount(Dictionnaire bank)
        {
            Account account;
            Gestionnaire g = DoesTheGestionnaireExist(bank, IdentifiantEntree);
            
            // On vérifie que le compte n'existe pas déjà
            foreach (Account acc in bank.Accounts)
            {
                if (acc.Identifiant == Identifiant)
                    throw new ArgumentException($"Le compte existe déjà {Identifiant}");
            }

            switch (TypeAccount)
            {
                case AccountType.LIVRET:
                    account = new AccountLivret(Identifiant, Date, Amount)
                    {
                        Manager = g
                    };
                    bank.Accounts.Add(account);
                    break;
                case AccountType.TERME:
                    account = new AccountTerme(Identifiant, Date, Amount)
                    {
                        Manager = g
                    };
                    bank.Accounts.Add(account);
                    break;
                case AccountType.COMPTE:
                    account = new Account(Identifiant, Date, Amount)
                    {
                        Manager = g
                    }; 
                    bank.Accounts.Add(account);
                    break;
                case AccountType.JEUNE:
                    account = new AccountJeune(Identifiant, Date, c, Amount)
                    {
                        Manager = g
                    };
                    bank.Accounts.Add(account);
                    break;
                default:
                    break;
            }
            bank.NbComptes++;
        }

        private void _ClotureAccount(Dictionnaire bank)
        {
            Account account = DoesTheAccountExist(bank, Identifiant);
            Gestionnaire m = DoesTheGestionnaireExist(bank, IdentifiantSortie);
 
            // On vérifie que le manager correspond bien
            if (account.Manager.Name != m.Name)
                throw new ArgumentException($"Le compte {Identifiant} n'est pas géré par le manager {IdentifiantSortie}");

            if (!account.CanWeCloseTheAccount(Date))
                throw new ArgumentException($"Impossible de clôturer le compte {Identifiant}, cela fait moins de 5ans");

            account.DateClo = Date;
            bank.NbComptes--;
        }

        private void _CessionAccount(Dictionnaire bank)
        {
            Account account = DoesTheAccountExist(bank, Identifiant);
            Gestionnaire ges1 = DoesTheGestionnaireExist(bank, IdentifiantEntree);
            Gestionnaire ges2 = DoesTheGestionnaireExist(bank, IdentifiantSortie);

            if (account.Manager.Name != ges1.Name)
                throw new ArgumentException($"Le compte {Identifiant} n'est pas géré par le manager {IdentifiantEntree}");

            if (ges1.Name == ges2.Name)
                throw new ArgumentException($"Impossible de céder le compte au même gestionnaire {Identifiant}");
            account.Manager = ges2;
        }

        private string _Execute(Dictionnaire bank)
        {
            try
            {
                if (IdentifiantEntree == null && IdentifiantSortie == null)
                    return Status.KO;
                if (IdentifiantSortie == null)
                    _AddNewAccount(bank);
                else if (IdentifiantEntree == null)
                    _ClotureAccount(bank);
                else // Cession d'un compte
                    _CessionAccount(bank);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Status.KO;
            }

            return Status.OK;
        }

        public override void Execute(Dictionnaire bank)
        {
            bank.FH.SortieAccountF.WriteLine($"{Identifiant};{_Execute(bank)}");
        }

    }

    internal class OperationTransaction : Operation
    {
        public OperationTransaction(string[] listColumns) : base(listColumns, TypeOperation.TRANSACTION)
        {
        }

        private void _MakeVirement(Dictionnaire bank)
        {
            Account emetteur = DoesTheAccountExist(bank, IdentifiantEntree);
            Account destinataire = DoesTheAccountExist(bank, IdentifiantSortie);
            Transaction T = new Transaction(Identifiant, Date, Amount, emetteur, destinataire);
            T.Make();
        }

        private string _Execute(Dictionnaire bank)
        {
            bank.NbTransactions++;

            try
            {
                if (IdentifiantEntree == null || IdentifiantSortie == null)
                    return Status.KO;
                _MakeVirement(bank);
                bank.NbReussites++;
                bank.MontantReussite += Amount;
            }
            catch (Exception e)
            {
                bank.NbEchecs++;
                Console.WriteLine(e.Message);
                return Status.KO;
            }

            return Status.OK;
        }


        public override void Execute(Dictionnaire bank)
        {
            bank.FH.SortieTransactionsF.WriteLine($"{Identifiant};{_Execute(bank)}");
        }

    }

}