namespace ChatTU.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.File",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        ContentType = c.String(nullable: false),
                        Data = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Message", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Message", "IsAttachment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.File", "Id", "dbo.Message");
            DropIndex("dbo.File", new[] { "Id" });
            DropColumn("dbo.Message", "IsAttachment");
            DropTable("dbo.File");
        }
    }
}
