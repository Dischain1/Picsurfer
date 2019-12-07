namespace Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class toFileModel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Pictures", newName: "Files");
            DropForeignKey("dbo.Rates", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.Rates", "UserId", "dbo.Users");
            DropIndex("dbo.Rates", new[] { "UserId" });
            DropIndex("dbo.Rates", new[] { "PictureId" });
            CreateTable(
                "dbo.Downloads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.FileId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.FileId);
            
            DropTable("dbo.Rates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PictureId = c.Int(nullable: false),
                        Like = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Downloads", "UserId", "dbo.Users");
            DropForeignKey("dbo.Downloads", "FileId", "dbo.Files");
            DropIndex("dbo.Downloads", new[] { "FileId" });
            DropIndex("dbo.Downloads", new[] { "UserId" });
            DropTable("dbo.Downloads");
            CreateIndex("dbo.Rates", "PictureId");
            CreateIndex("dbo.Rates", "UserId");
            AddForeignKey("dbo.Rates", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Rates", "PictureId", "dbo.Pictures", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Files", newName: "Pictures");
        }
    }
}
