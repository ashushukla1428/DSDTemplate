using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Wempe.CommonClasses
{
    public class Helper
    {

        public static string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
        {
            // If salt is not specified, generate it.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
            new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash;

            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
            saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        public static bool VerifyHash(string plainText, string hashAlgorithm, string hashValue)
        {

            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits, hashSizeInBytes;

            // Make sure that hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Size of hash is based on the specified algorithm.
            switch (hashAlgorithm.ToUpper())
            {

                case "SHA384":
                    hashSizeInBits = 384;
                    break;

                case "SHA512":
                    hashSizeInBits = 512;
                    break;

                default: // Must be MD5
                    hashSizeInBits = 128;
                    break;
            }

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            // Compute a new hash string.
            string expectedHashString = ComputeHash(plainText, hashAlgorithm, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }

        public static string CreateRandomPassword(int passwordLength)
        {
            passwordLength = 8;

            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public static BColor GetThemeByCompanyId(Int64 Id)
        {
            BColor clr = new BColor();
            var k = new wmpWebsiteTheme();
            using (var dbCtx = new dbWempeEntities())
            {
                 k = dbCtx.wmpWebsiteThemes.Where(s => s.CompanyId == Id).FirstOrDefault();
                if (k == null)
                {
                    var newId = 0;
                    k = dbCtx.wmpWebsiteThemes.Where(s => s.CompanyId == newId).FirstOrDefault();
                }
            }            
            clr.BodyColor = k.BodyColor;
            clr.FormActions = k.FormActions;
            clr.FormControl = k.FormControl;
            clr.ModalPopUp = k.ModalPopUp;
            clr.PageBar = k.PageBar;
            clr.PageContent = k.PageContent;
            clr.PageFooter = k.PageFooter;
            clr.PageHeader = k.PageHeader;
            clr.PageSidebar = k.PageSidebar;
            clr.PortletBody = k.PortletBody;
            clr.PortletTitle = k.PortletTitle;
            clr.SidebarSubMenu = k.SidebarSubMenu;
            clr.TabContent = k.TabContent;
            clr.Button = k.Button;
            clr.ActiveTab = k.ActiveTab;
            clr.ActiveSideBar = k.ActiveSideBar;
            clr.SideBarHover = k.SideBarHover;
            clr.InnerTable = k.InnerTable;
            clr.InnerTableHover = k.InnerTableHover;
            clr.ButtonHover = k.ButtonHover;
            return clr;
           
        }
    }
    public class BColor
    {
        public string BodyColor { get; set; }
        public string PageSidebar { get; set; }
        public string PageHeader { get; set; }
        public string PageFooter { get; set; }
        public string PageContent { get; set; }
        public string PageBar { get; set; }
        public string TabContent { get; set; }
        public string PortletTitle { get; set; }
        public string PortletBody { get; set; }
        public string FormActions { get; set; }
        public string FormControl { get; set; }
        public string ModalPopUp { get; set; }
        public string SidebarSubMenu { get; set; }
        public string Button { get; set; }
        public string ActiveTab { get; set; }
        public string ActiveSideBar { get; set; }
        public string SideBarHover { get; set; }
        public string InnerTable { get; set; }
        public string InnerTableHover { get; set; }
        public string ButtonHover { get; set; }
    }
}

