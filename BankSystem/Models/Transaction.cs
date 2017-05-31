using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankSystem.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public int? TransactionType_Id { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        public virtual string TransferTo { get; set; }
        public virtual string TransactionType { get; set; }
    }
}