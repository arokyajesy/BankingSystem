using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBANK
{
    internal interface IBankRepository
    {
        void CreateAccount(Customer customer, long accNo, string accType, float balance);
        List<Account> ListAccounts();
        float GetAccountBalance(long accountNumber);
        float Deposit(long accountNumber, float amount);
        float Withdraw(long accountNumber, float amount);
        void Transfer(long from, long to, float amount);
        void CalculateInterest();
        Account GetAccountDetails(long accountNumber);
        List<Transactions> GetTransactions(DateTime from, DateTime to);
    }
}
