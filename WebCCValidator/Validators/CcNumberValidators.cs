using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCCValidator.Validators
{
    public class CcNumberValidators
    {
        public static bool CheckLuhn(List<int> ccNumber)
        {
            int numDigits = ccNumber.Count;
            int sum = ccNumber[numDigits - 1];
            int parity = numDigits % 2;
            for(int i=0; i< numDigits - 2; i++)
            {
                int digit = ccNumber[i];
                if ((i % 2) == parity)
                {
                    digit = digit*2;
                }
                if (digit > 9) {
                    digit = digit - 9;
                }
                sum += digit;
            }
            return (sum % 10) == 0;
        } 
    }
}
