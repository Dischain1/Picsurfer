namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class password_columnn_deleted : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Password", c => c.String());
        }
    }
}
