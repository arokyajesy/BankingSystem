using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using HMBANK.Exceptions;

namespace HMBANK
{
    public class BankRepositoryImpl : IBankRepository
    {
        SqlConnection con = null;
        SqlCommand command = null;

        public void CreateAccount(Customer customer, long accNo, string accType, float balance)
        {
            int rowsAffected = 0;
            string query = $"insert into accounts (account_id, customer_id, account_type,balance)values(@aid,@cid,@at,@b)";
            string query1 = $"insert into customers(customer_id,first_name,last_name,DOB,email,phone_number,address(@cid,@fn,@ln,@DOB,@email,@phone,@add)";
            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@aid", accNo));
                    command.Parameters.Add(new SqlParameter("@cid", customer.customerID));
                    command.Parameters.Add(new SqlParameter("@at", accType));
                    command.Parameters.Add(new SqlParameter("@b", balance));
                    rowsAffected = command.ExecuteNonQuery();
                }
                if (rowsAffected <= 0)
                {
                    throw new NotUpdatedException("Account not added");
                }
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query1, con);
                    command.Parameters.Add(new SqlParameter("@cid", customer.customerID));
                    command.Parameters.Add(new SqlParameter("@fn", customer.firstName));
                    command.Parameters.Add(new SqlParameter("@ln", customer.lastName));
                    command.Parameters.Add(new SqlParameter("@DOB", customer.DOB));
                    command.Parameters.Add(new SqlParameter("@email", customer.emailAdress));
                    command.Parameters.Add(new SqlParameter("@phone", customer.address));
                    rowsAffected = command.ExecuteNonQuery();
                }
                if (rowsAffected <= 0)
                {
                    throw new NotUpdatedException("Account not updated");
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public float GetAccountBalance(long accountNumber)
        {
            float balance = 0;
            SqlConnection con = null;
            SqlCommand command = null;
            Account account = null;
            account = new Account();

            String query = "select balance from accounts where account_id @accountNumber";
            try
            {
                if (account == null)
                {
                    throw new AccountNotFoundException("Account not found");
                }
                else
                {
                    using (con = DBUtility.GetConnection())
                    {
                        command = new SqlCommand(query, con);
                        command.Parameters.AddWithValue("@accountNumber", accountNumber);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            balance = (float)reader["balance"];
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return balance;
        }
        public float Deposit(long accountNumber, float amount)
        {
            int rowsAffected = 0;
            Account account = GetAccountDetails(accountNumber);
            float balance = account.accountBalance;
            if (account == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if (amount <= 0)
            {
                throw new InvalidAmountException("Enter amount greater than zero");
            }
            else
            {
                string query = "update accounts set balance=@balance where account_id=@id";
                try
                {
                    using (con = DBUtility.GetConnection())
                    {
                        balance += amount;
                        command = con.CreateCommand();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@balance", balance);
                        command.Parameters.AddWithValue("@id", accountNumber);
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    if (rowsAffected <= 0)
                    {
                        throw new NotUpdatedException("Account not updated");
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return balance;
        }
        public float Withdraw(long accountNumber, float amount)
        {
            int rowsAffected = 0;
            Account account = GetAccountDetails(accountNumber);
            float balance = account.accountBalance;
            if (account == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if (account.accountType == "Savings" || balance - amount < 500)
            {
                throw new InsufficientFundException("insufficient balance");
            }
            else
            {
                string query = "update accounts set balance=@balance where account_id=@accountNumber";
                try
                {
                    using (con = DBUtility.GetConnection())
                    {
                        balance -= amount;
                        command = con.CreateCommand();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@balance", balance);
                        command.Parameters.AddWithValue("@id", accountNumber);
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    if (rowsAffected <= 0)
                    {
                        throw new NotUpdatedException("Account not updated");
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return balance;
        }
        public Account GetAccountDetails(long accountNumber)
        {
            Account account = null;
            SqlConnection con = null;
            SqlCommand command = null;
            string query = "select* from accounts where account_id=@id";
            try
            {

                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@id", accountNumber);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        account = new Account();
                        account.accountNumber = (int)reader["account_id"];
                        account.accountType = (string)reader["account_type"];
                        account.accountBalance = (float)reader["balance"];

                    }
                    if (account == null)
                    {
                        throw new AccountNotFoundException("Account not found");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return account;
        }
        public void Transfer(long from, long to, float amount)
        {
            int rowsAffected = 0;
            Account account = GetAccountDetails(from);
            Account account1 = GetAccountDetails(to);
            float balance = account.accountBalance;
            float bal = account1.accountBalance;
            if (account == null || account1 == null)
            {
                throw new AccountNotFoundException("account not found");
            }
            if (account.accountType == "Savings" && balance - amount < 500)
            {
                throw new InsufficientFundException("insufficient balance");
            }
            else
            {
                string query = "update accounts set balance=@balance where account_id=@id";
                string query1 = "update accounts set balance=@b where account_id=@to";
                try
                {
                    using (con = DBUtility.GetConnection())
                    {
                        balance -= amount;
                        command = con.CreateCommand();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@balance", balance);
                        command.Parameters.AddWithValue("@id", from);
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    if (rowsAffected <= 0)
                    {
                        throw new NotUpdatedException("Account not updated");
                    }
                    using (con = DBUtility.GetConnection())
                    {
                        balance += amount;
                        command = con.CreateCommand();
                        command.CommandText = query1;
                        command.Parameters.AddWithValue("@b", balance);
                        command.Parameters.AddWithValue("@to", to);
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    if (rowsAffected <= 0)
                    {
                        throw new NotUpdatedException("Account not updated");
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public List<Account> ListAccounts()
        {
            List<Account> accounts = new List<Account>();   
            Account account = null;
            SqlConnection con = null;
            SqlCommand command = null;
            string query = "select * from accounts";
            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        account = new Account();
                        account.accountBalance = reader.GetFloat(2);
                        account.accountNumber = reader.GetInt32(0);
                        account.accountType = reader.GetString(3);
                        accounts.Add(account);
                    }
                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accounts;
        }
        public List<Transactions> GetTransactions(DateTime from, DateTime to)
        {
            Transactions transaction = null;
            List<Transactions> transactions = new List<Transactions>();
            SqlConnection con = null;
            SqlCommand command = null;
            string query = "select * from transactions where transaction_date>=@from and transaction_date<=@to";
            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        transaction = new Transactions();
                        transaction.transactionAmount = reader.GetFloat(2);
                        transaction.dateTime = (DateTime)reader["transaction_date"];
                        transaction.transactionType = reader.GetString(3);
                        transactions.Add(transaction);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return transactions;
        }

        public void CalculateInterest()
        {

        }
    }
}
    


