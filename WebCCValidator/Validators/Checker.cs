using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCCValidator.Interfaces;
using WebCCValidator.Models;

namespace WebCCValidator.Validators
{
    public class Checker : IChecker
    {

        private readonly IConfiguration _config;
        private List<Issuer> _issures;
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
            List<char> datalist = new List<char>();
            datalist.AddRange(key);
            bool result = false;
            //Issuer/Lenght relation check
            Issuer current = _issures.Where(s => s.IIN.Contains(datalist[0]) || s.IIN.Contains($"{datalist[0]}{datalist[1]}")).FirstOrDefault();
            result &= (current.Lenght == datalist.Count);
            //Luhn check
             List<int> cc = datalist.Select(s => Convert.ToInt32(s)).ToList();
             result &= CcNumberValidators.CheckLuhn(cc);

            return result;
        }
    }
}
