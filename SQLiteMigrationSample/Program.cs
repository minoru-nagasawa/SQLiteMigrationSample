using SQLiteMigrationSample.DataAccess;
using SQLiteMigrationSample.Migrations;
using SQLiteMigrationSample.Models;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

namespace SQLiteMigrationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var exeDir  = AppDomain.CurrentDomain.BaseDirectory;
            var dbPath  = $"{exeDir}db.sqlite3";
            var connStr = $"DATA Source={dbPath}";
            using (var connection = new SQLiteConnection(connStr))
            {
                using (var context = new ApplicationDbContext(connection))
                {
                    // providerNameをコードを使って取得する。
                    // コードを使わずに、直接"System.Data.SQLite"を使ってもいい
                    // https://stackoverflow.com/questions/36060478/dbmigrator-does-not-detect-pending-migrations-after-switching-database
                    var internalContext = context.GetType().GetProperty("InternalContext", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(context);
                    var providerName = (string)internalContext.GetType().GetProperty("ProviderName").GetValue(internalContext);

                    // Migratorが使うConfigurationを生成する。
                    // TargetDatabaseはDbMigratorの方ではなく、Configurationの方に設定しないと効果が無い。
                    var configuration = new Configuration()
                    {
                        TargetDatabase = new DbConnectionInfo(context.Database.Connection.ConnectionString, providerName)
                    };

                    // DbMigratorを生成する
                    var migrator = new DbMigrator(configuration);

                    // EF6.13では問題ないが、EF6.2の場合にUpdateのタイミングで以下の例外が吐かれないようにする対策
                    // System.ObjectDisposedException: '破棄されたオブジェクトにアクセスできません。
                    // オブジェクト名 'SQLiteConnection' です。'
                    // https://stackoverflow.com/questions/47329496/updating-to-ef-6-2-0-from-ef-6-1-3-causes-cannot-access-a-disposed-object-error/47518197
                    var _historyRepository = migrator.GetType().GetField("_historyRepository", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(migrator);
                    var _existingConnection = _historyRepository.GetType().BaseType.GetField("_existingConnection", BindingFlags.Instance | BindingFlags.NonPublic);
                    _existingConnection.SetValue(_historyRepository, null);
                    
                    // Migrationを実行する。
                    migrator.Update();
                    
                    // データベースにアクセスして保存する例
                    if (context.Samples.Count() == 0)
                    {
                        var dummyItem = new SamplePoco()
                        {
                            Id = 1,
                            Name = "Dummy"
                        };
                        context.Samples.Add(dummyItem);
                        context.SaveChanges();
                    }

                    // データベースにアクセスして保存する例
                    if (context.Tests.Count() == 0)
                    {
                        var dummyItem = new TestPoco()
                        {
                            Id = 1,
                            Name = "Dummy"
                        };
                        context.Tests.Add(dummyItem);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
