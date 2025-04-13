using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBANK
{
    internal interface IBankServiceProvider
    {
        void CreateAccount(Customer customer, string accountType, float balance);
        void ListAccounts();
        void CalculateInterest();
    }
}
