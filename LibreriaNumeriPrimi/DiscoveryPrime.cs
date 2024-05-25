using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaNumeriPrimi
{
    public class DiscoveryPrime : IPrimeNumber
    {
        public bool VerifyPrimeNumber(int number)
        {
            int sqrtNumber =(int)Math.Floor(Math.Sqrt(number))+1;

            if(number==2) { return true; }

            if(number < 2) return false;

            for(int i =2; i< sqrtNumber; i++) {
            
                if(number % i ==0) return false;
            }
            return true;
        }
        public void PrimeNumberInRange(int min, int max)
        {
            if(min>max)
            {
                int temp = max;
                max = min;
                min = temp;
            }

            for (int i = min; i <= max; i++)
            {
                if (VerifyPrimeNumber(i))
                {
                    Console.WriteLine($"{i} è un numero primo");
                } else
                {
                    Console.WriteLine($"{i} non è un numero primo");
                }
            }

        }
    }
}
