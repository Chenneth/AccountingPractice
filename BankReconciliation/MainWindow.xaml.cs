using System;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;

//using System.Linq;


namespace BankReconciliation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //var declarations
        private int _bankUnadjusted, _bookUnadjusted, _reconciledBalance;
        private List<Transaction> _transactions;

        public MainWindow()
        {
            _transactions = new List<Transaction>(40);
            InitializeComponent();
        }
        
        /// <summary>
        /// Called when user presses start button
        /// </summary>
        private void InitializeValues()
        {
            _transactions.Clear();
            var extraBound = RandomNumberGenerator.GetInt32(300, 701);
            _reconciledBalance = RandomNumberGenerator.GetInt32(1000, 100001);
            if(RandomNumberGenerator.GetInt32(2)==1)
            {
                _bankUnadjusted =
                    RandomNumberGenerator.GetInt32((int)(_reconciledBalance * .7)-extraBound, (int)(_reconciledBalance * 1.3)+extraBound);
                _bookUnadjusted =
                    RandomNumberGenerator.GetInt32((int)(_bankUnadjusted * .7), (int)(_bankUnadjusted * 1.3));
            }
            else
            {
                _bookUnadjusted =
                    RandomNumberGenerator.GetInt32((int)(_reconciledBalance * .7)-extraBound, (int)(_reconciledBalance * 1.3)+extraBound);
                _bankUnadjusted =
                    RandomNumberGenerator.GetInt32((int)(_bookUnadjusted * .7), (int)(_bookUnadjusted * 1.3));
            }
        }

        private void InitializeTransactions()
        {
            //suggestion: add a difficulty option
            //should only affect the amt of transactions? so lower limit changes (or we just enforce 40 on hard)
            var transactionCount = RandomNumberGenerator.GetInt32(5, 40);
            var bookSum = _bookUnadjusted;
            var bankSum = _bankUnadjusted;
            for (int i = transactionCount-2;
                 transactionCount >= 0;
                 transactionCount--)
            {
                Transaction transaction;
                transaction.Amount = RandomNumberGenerator.GetInt32(1, (int)(_bankUnadjusted * .4));
                if (RandomNumberGenerator.GetInt32(2) == 1) //so we get a roughly equal amount of bank and book transactions
                {
                    transaction.IsBankSide = true;
                    bankSum += transaction.Amount;
                    transaction.Type = (Transaction.TransactionType)RandomNumberGenerator.GetInt32(1, 4);
                }
                else
                {
                    transaction.IsBankSide = false;
                    bookSum += transaction.Amount;
                    transaction.Type = (Transaction.TransactionType)RandomNumberGenerator.GetInt32(4, 10);
                }
                _transactions.Add(transaction);
            }
            _transactions.Add(new Transaction());
        }
    }

    public struct Transaction
    {
        public enum TransactionType
        {
            DIT = 1, //Deposit In-Transit
            OutCheck = 2, //Outstanding Check
            BankErr = 3, //Bank Error
            EFTReceipt = 4, //EFT Receipt
            EFTPayment = 5, //EFT Payment
            IntRev = 6, //Interest Revenue
            NSF = 7, //NSF Check (Insufficient funds check)
            BookErr = 8, //Book Error
            BankCollect = 9 //Bank Collection of Accounts Receivable
            ,NONE = 0
        }

        public Transaction(int amount, TransactionType type)
        {
            Amount = amount;
            Type = type;
            IsBankSide = !((int)type >= 4);
        }
        
        public int Amount;
        public TransactionType Type;
        public bool IsBankSide;
    }
  
    /// <summary>
    /// Generates a pseudo-random DateTime value using the System Random class
    /// </summary>
    public class RandomDateTime //from st. ackoverfl ow.com/a/ 26 263 669
    {
        private readonly DateTime _start;
        private readonly Random _gen;
        private readonly int _range;

        public RandomDateTime()
        {
            _start = new DateTime(1995, 1, 1);
            _gen = new Random();
            _range = (DateTime.Today - _start).Days;
        }

        public DateTime Next()
        {
            return _start.AddDays(_gen.Next(_range)).AddHours(_gen.Next(0, 24)).AddMinutes(_gen.Next(0, 60))
                .AddSeconds(_gen.Next(0, 60));
        }
    }
}