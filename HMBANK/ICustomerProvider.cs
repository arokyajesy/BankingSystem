using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBANK
{
    internal interface ICustomerProvider
    {
        float GetAccountBalance(long accountNumber);
        float Deposit(long accountNumber, float ammount);
        float Withdraw(long accountNumber, float amount);
        void Transfer(long from, long to, float amount);
        string GetAccountDetails(long accountNumber);
        List<Transactions> GetTransactions(DateTime from, DateTime to);
    }
}
