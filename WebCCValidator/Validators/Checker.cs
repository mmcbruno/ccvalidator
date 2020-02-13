using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebCCValidator.Interfaces;
using WebCCValidator.Models;

namespace WebCCValidator.Validators
{
    public class Checker : IChecker
    {

        private readonly IConfiguration _config;
        private List<Issuer> _issures;
        private readonly string NonNumericalRegex = "[^.0-9]";
        public Checker(IConfiguration config)
        {
            _config = config;
            _issures = new List<Issuer>();
            IEnumerable<IConfigurationSection> issuers = _config.GetSection("IssuersIdentifiers").GetChildren();

            foreach (IConfigurationSection configSection in issuers)
            {
                _issures.Add(
                    new Issuer
                    {
                        Name = configSection["Name"],
                        IIN = configSection["IIN"],
                        Lenght = Int16.Parse(configSection["Lenght"])
                    }
                 );
            }
        }

        public bool CheckCC(string key)
        {

            if (string.IsNullOrEmpty(key)) { return false; }
            //List<char> datalist = new List<char>();
            //datalist.AddRange(key);
            
            bool isValid = false;
            //only numeric char
            Match m = Regex.Match(key, NonNumericalRegex, RegexOptions.IgnoreCase);
            isValid = !m.Success;
            if (isValid)
            {
                //Issuer/Lenght relation check
                Issuer current = _issures.Where(s => s.IIN.Contains(key[0]) || s.IIN.Contains($"{key[0]}{key[1]}")).FirstOrDefault();
                isValid &= (current != null && current.Lenght == key.Length);
            }
            if (isValid)
            {
                //Luhn check
                //List<int> cc = datalist.Select(s => (int)Char.GetNumericValue(s)).ToList();
                isValid &= CheckLuhn(key);
            }
            return isValid;
        }

        private bool CheckLuhn(string ccNumber)
        {
            if (string.IsNullOrEmpty(ccNumber)) return false;
            int numDigits = ccNumber.Length;
            ccNumber = new String(ccNumber.Reverse().ToArray());
            int sum = int.Parse(ccNumber[0].ToString());// check digit
            for (int i = 1; i <= numDigits - 1; i++)
            {
                int digit = int.Parse(ccNumber[i].ToString()); 
                if ((i % 2) == 1)
                {
                    digit = digit * 2;
                    if (digit > 9)
                    {
                        digit = digit - 9;
                    }
                }
                sum += digit;
            }
            return (sum % 10) == 0;
        }
    }
}
