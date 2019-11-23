namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hashMd5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PasswordHash", c => c.String());
            AlterColumn("dbo.Users", "Password", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false));
            DropColumn("dbo.Users", "PasswordHash");
        }
    }
}
