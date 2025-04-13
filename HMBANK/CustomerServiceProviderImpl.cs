using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBANK
{
        internal class CustomerServiceProviderImpl : ICustomerProvider
        {
            protected List<Account> accountList = new List<Account>();
            protected List<Transactions> transactionList = new List<Transactions>();
            public float GetAccountBalance(long accountNumber)
            {
                var account = accountList.FirstOrDefault(a => a.accountNumber == accountNumber);
                if (account == null)
                {
                    Console.WriteLine("Account not found");
                }

                return account.accountBalance;
            }
            public float Deposit(long accountNumber, float Amount)
            {
                var account = accountList.FirstOrDefault(a => a.accountNumber == accountNumber);
                if (account == null)
                {
                    Console.WriteLine("Account not found");

                }
                account.Deposit(accountNumber, Amount);
                return account.accountBalance;
            }
            public float Withdraw(long accountNumber, float Amount)
            {
                var account = accountList.FirstOrDefault(a => a.accountNumber == accountNumber);
                if (account == null)
                {
                    Console.WriteLine("Account not found");
                }
                account.Withdraw(accountNumber, Amount);
                return account.accountBalance;
            }
            public void Transfer(long From, long To, float Amount)
            {
                var from = accountList.FirstOrDefault(a => a.accountNumber == From);
                var to = accountList.FirstOrDefault(b => b.accountNumber == To);
                if (from == null || to == null)
                {
                    Console.WriteLine("Account not found");
                }
                from.Withdraw(From, Amount);
                to.Deposit(To, Amount);
                Console.WriteLine("Successfully transfered");
            }
            public string GetAccountDetails(long accountNumber)
            {
                var account = accountList.FirstOrDefault(a => a.accountNumber == accountNumber);
                if (account == null)
                {
                    Console.WriteLine("Account not found");
                }
                return account.ToString();
            }
            public List<Transactions> GetTransactions(DateTime from, DateTime to)
            {
                List<Transactions> result = new List<Transactions>();
                foreach (Transactions t in transactionList)
                {
                    if (t.dateTime >= from && t.dateTime <= to)
                    {
                        result.Add(t);
                    }
                }
                return result;
            }
        }
    

}

