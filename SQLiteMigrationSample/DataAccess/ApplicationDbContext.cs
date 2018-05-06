using SQLiteMigrationSample.Models;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;

namespace SQLiteMigrationSample.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Update-Database用のsqliteのDBのパスを格納します
        /// </summary>
        static private string s_migrationSqlitePath;

        /// <summary>
        /// Update-Database用のsqliteのDBのパスを設定する
        /// </summary>
        static ApplicationDbContext()
        {
            var exeDir     = AppDomain.CurrentDomain.BaseDirectory;
            var exeDirInfo = new DirectoryInfo(exeDir);
            var projectDir = exeDirInfo.Parent.Parent.FullName;
            s_migrationSqlitePath = $@"{projectDir}\MigrationDb.sqlite3";
        }

        /// <summary>
        /// Update-Database実行時に使用されるコンストラクタ。
        /// baseで呼び出すコンストラクタの第2引数をfalseにしないと、Update-Databaseで
        /// "System.ObjectDisposedException: 破棄されたオブジェクトにアクセスできません。"
        /// の例外が発生する。
        /// </summary>
        public ApplicationDbContext() : base(new SQLiteConnection($"DATA Source={s_migrationSqlitePath}"), false)
        {
        }

        /// <summary>
        /// Update-Database実行時以外に使うコンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public ApplicationDbContext(DbConnection connection) : base(connection, true)
        {
        }

        /// <summary>
        /// Sampleテーブルにアクセスします
        /// </summary>
        public DbSet<SamplePoco> Samples { get; set; }

        /// <summary>
        /// Testテーブルにアクセスします
        /// </summary>
        public DbSet<TestPoco> Tests { get; set; }
    }
}
