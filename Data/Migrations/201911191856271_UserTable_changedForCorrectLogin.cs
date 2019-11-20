namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTable_changedForCorrectLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Users", "Password", c => c.String(nullable: false));
            DropColumn("dbo.Users", "FirstName");
            DropColumn("dbo.Users", "SecondName");
            DropColumn("dbo.Users", "Patronimic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Patronimic", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Users", "SecondName", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Users", "FirstName", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Users", "Password");
            DropColumn("dbo.Users", "Email");
        }
    }
}
