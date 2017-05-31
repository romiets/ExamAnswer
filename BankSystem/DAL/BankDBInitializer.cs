using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BankSystem.Models;

namespace BankSystem.DAL
{
    public class BankDBInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<BankDBContext>
    {
        protected override void Seed(BankDBContext context)
        {
            var accounts = new List<Account>
            {
            new Account{AccountNumber="10001",AccountName="Romeo Payawal", Password="password", Balance=0,CreatedDate=DateTime.Parse("2017-01-01")}
            };

            accounts.ForEach(s => context.Accounts.Add(s));
            context.SaveChanges();

            var transactionTypes = new List<TransactionType>
            {
                new TransactionType{Name="Deposit",},
                new TransactionType{Name="Withdraw",},
                new TransactionType{Name="FundTransfer",}
            };

            transactionTypes.ForEach(s => context.TransactionTypes.Add(s));
            context.SaveChanges();

        }
    }
}