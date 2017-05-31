namespace BankSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "AccountNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "AccountNumber");
        }
    }
}
