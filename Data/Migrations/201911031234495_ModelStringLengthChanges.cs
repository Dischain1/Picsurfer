namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelStringLengthChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pictures", "Path", c => c.String(nullable: false));
            AlterColumn("dbo.Pictures", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Users", "FirstName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Users", "SecondName", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Users", "Patronimic", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Patronimic", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Users", "SecondName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Users", "FirstName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Pictures", "Name", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Pictures", "Path", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
