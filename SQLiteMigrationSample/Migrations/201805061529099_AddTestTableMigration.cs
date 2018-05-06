namespace SQLiteMigrationSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTestTableMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Test",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 2147483647),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Test");
        }
    }
}
