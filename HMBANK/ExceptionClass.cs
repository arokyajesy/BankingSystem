using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBANK.Exceptions
{
    //internal class ExceptionClass

        //internal class ExceptionClass
        //{

        public class InsufficientFundException : Exception
        {
            public InsufficientFundException(string message) : base(message) { }
        }

        public class InvalidAccountException : Exception
        {
            public InvalidAccountException(string message) : base(message) { }
        }

        public class OverDraftLimitExceededException : Exception
        {
            public OverDraftLimitExceededException(string message) : base(message) { }
        }
        public class NotUpdatedException : Exception
        {
            public NotUpdatedException(string message) : base(message) { }
        }
        public class AccountNotFoundException : Exception
        {
            public AccountNotFoundException(string message) : base(message) { }
        }
        public class InvalidAmountException:Exception
        {
            public InvalidAmountException(string message) : base(message) { }
    }
    }







