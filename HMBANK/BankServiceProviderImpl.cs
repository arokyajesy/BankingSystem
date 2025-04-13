using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBANK
{
        internal class BankServiceProviderImpl : CustomerServiceProviderImpl, IBankServiceProvider
        {

            string branchName;
            string branchAddress;
            public void CreateAccount(Customer customer, string accountType, float accountBalance)
            {
                Account newAccount;
                if (accountType == "Savings")
                {
                    newAccount = new SavingsAccount(customer, accountBalance);
                }
                else if (accountType == "Current")
                {
                    newAccount = new CurrentAccount(customer, accountBalance);
                }
                else if (accountType == "ZeroBalance")
                {
                    newAccount = new ZeroAccountBalance(customer, accountBalance);
                }
                else
                {
                    throw new ArgumentException("Invalid account type");
                }
                accountList.Add(newAccount);
            }

            public void ListAccounts()
            {
                foreach (var account in accountList)
                {
                    Console.WriteLine(account);
                }
            }
            public void CalculateInterest()
            {
                foreach (var acc in accountList.OfType<SavingsAccount>())
                {
                    acc.Deposit(acc.accountNumber, acc.accountBalance * acc.InterestRate);
                }

            }

        }
    

}

