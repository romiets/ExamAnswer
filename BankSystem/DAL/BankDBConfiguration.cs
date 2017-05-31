using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace BankSystem.DAL
{
    public class BankDBConfiguration : DbConfiguration
    {
        public BankDBConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}