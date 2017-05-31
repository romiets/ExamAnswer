using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Models
{
    public enum TransactionTypeEnum
        {
        Deposit=1,
        Withdraw=2,
        FundTransfer=3
    }
    public class TransactionType
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}