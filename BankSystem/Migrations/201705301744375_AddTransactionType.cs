namespace BankSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransactionType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "TransactionType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "TransactionType");
        }
    }
}
