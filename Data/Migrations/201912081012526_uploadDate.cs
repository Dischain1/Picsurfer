namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uploadDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "UploadDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "UploadDate");
        }
    }
}
