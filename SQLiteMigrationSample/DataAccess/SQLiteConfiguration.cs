using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

namespace SQLiteMigrationSample.DataAccess
{
    /// <seealso cref="https://msdn.microsoft.com/en-us/data/jj680699"/>
    /// <seealso cref="https://www.codeproject.com/Articles/1158937/Using-SQLite-database-with-Csharp-Net-and-Entity-F"/>
    public class SQLiteConfiguration : DbConfiguration
    {
        public SQLiteConfiguration()
        {
            SetProviderFactory("System.Data.SQLite",     SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            SetProviderServices("System.Data.SQLite",    (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }
}
