using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MebelMag
{
      internal class Check
    {
        public static bool CheckPassword(string password)
        {
            if (password == null)
                return false;

            const int MIN_LENGTH = 6;
            const int MAX_LENGTH = 15;


            bool meetsLengthRequirements = password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH;
            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasDecimalDigit = false;
            bool hasNotRussian = false;
            if (meetsLengthRequirements)
            {
                foreach (char c in password)
                {
                    if (char.IsUpper(c)) hasUpperCaseLetter = true;
                    else if (char.IsLower(c)) hasLowerCaseLetter = true;
                    else if (char.IsDigit(c)) hasDecimalDigit = true;
                }
                hasNotRussian = !Regex.IsMatch(password, "[А-Яа-яЁё]");
            }

            bool isValid = meetsLengthRequirements
                        && hasUpperCaseLetter
                        && hasLowerCaseLetter
                        && hasDecimalDigit
                   && hasNotRussian;
            return isValid;

        }
        public static bool CheckEmail(string email)
        {
            if (email != null)
            {
                string expresion;
                expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (Regex.IsMatch(email, expresion) && Regex.IsMatch(email, "[А-Яа-яЁё]") == false)
                {
                    //  if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                    //{
                    return true;
                    //}
                    //else
                    //{
                    //  return false;
                    //}
                }
                else
                {
                    return false;
                }
            }
            else
                return false;

        }
        public static bool CheckPhone(string phone)
        {
            if (phone != null)
            {
                string expresion;
                expresion = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
                if (Regex.IsMatch(phone, expresion))
                {
                    // if (Regex.Replace(phone, expresion, string.Empty).Length == 0)
                    // {
                    return true;
                    // }
                    //else
                    //{
                    //    return false;
                    //}
                }
                else
                {
                    return false;
                }
            }
            else return false;

        }
    }
}
