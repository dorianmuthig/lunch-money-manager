using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LunchMoneyManager.Library.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public required Debtor Debtor { get; set; }
        public string? Reference { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public float Amount { get; set; }
        [Range(0.0, 100.0)]
        public float InterestRate { get; set; } // Current APR is 4.5%, may not be higher than 12 points above APR at time of transaction, must be 0 for repayments
    }
}
