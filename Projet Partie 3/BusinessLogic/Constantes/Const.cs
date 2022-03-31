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
        ERREUR
    }
}
