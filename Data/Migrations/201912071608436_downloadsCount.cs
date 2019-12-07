namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class downloadsCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Downloads", "Count", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Downloads", "Count");
        }
    }
}
