namespace BankSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransferTo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "TransferTo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "TransferTo");
        }
    }
}
