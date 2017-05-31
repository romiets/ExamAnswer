namespace BankSystem.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BankSystem.Models;
    using BankSystem.DAL;

    internal sealed class Configuration : DbMigrationsConfiguration<BankSystem.DAL.BankDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BankSystem.DAL.BankDBContext";
        }

        protected override void Seed(BankSystem.DAL.BankDBContext context)
        {
            var accounts = new List<Account>
            {
                new Account { AccountNumber = "10001",   AccountName = "Romeo Payawal", Password="password", Balance= 0, 
                    CreatedDate = DateTime.Parse("2017-01-01") }
            };

            accounts.ForEach(s => context.Accounts.AddOrUpdate(p => p.AccountNumber, s));
            context.SaveChanges();

            var transactionTypes = new List<TransactionType>
            {
                new TransactionType { Name="Deposit" },
                new TransactionType { Name="Withdraw" },
                new TransactionType { Name="FundTransfer" }
            };

            transactionTypes.ForEach(s => context.TransactionTypes.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

        }
    }
}
