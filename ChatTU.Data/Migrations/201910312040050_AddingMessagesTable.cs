namespace ChatTU.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMessagesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromUser = c.String(nullable: false, maxLength: 30),
                        ToUser = c.String(nullable: false, maxLength: 30),
                        Content = c.String(nullable: false),
                        Time = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
