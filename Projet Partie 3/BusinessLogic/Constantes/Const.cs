using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BankManagement
{
    public class Status
    {
        public const string OK = "OK";
        public const string KO = "KO";
    }

    public enum TypeFrais
    {
        PERCENTAGE,
        FIXE
    }

    public class TypeGestionnaire
    {
        public const string PARTICULIER = "Particulier";
        public const string ENTREPRISE  = "Entreprise";
    }

    enum TypeOperation
    {
        TRANSACTION,
        COMPTE
    }

    public enum AccountType
    {
        LIVRET,
        TERME,
        COMPTE,
        JEUNE,
        DEFAUT,
        ERREUR
    }

    public class AccountFileDefinition
    {
        public static int IDENTIFIANT = 0;
        public static int TYPE = 1;
        public static int DATE = 2;
        public static int SOLDE_INITIAL = 3;
        public static int AGE = 4;
        public static int IDENTIFIANT_ENTREE = 5;
        public static int IDENTIFIANT_SORTIE = 6;
    }

    public class TransactionFileDefinition
    {
        public static int IDENTIFIANT = 0;
        public static int DATE = 1;
        public static int MONTANT = 2;
        public static int EMETTEUR = 3;
        public static int DESTINATAIRE = 4;
    }
}
