namespace ChatTU.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitConversationAndKeys : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Messages", newName: "Message");
            DropForeignKey("dbo.UserRoles", "RoleID", "dbo.Role");
            DropForeignKey("dbo.UserRoles", "UserID", "dbo.Users");
            CreateTable(
                "dbo.Conversation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromUserId = c.Int(nullable: false),
                        ToUserId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FromUserId)
                .ForeignKey("dbo.Users", t => t.ToUserId)
                .Index(t => t.FromUserId)
                .Index(t => t.ToUserId);
            
            AddColumn("dbo.Message", "SendBy", c => c.String(nullable: false));
            AddColumn("dbo.Message", "Conversation_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "LoggedInStatus", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Message", "Conversation_Id");
            AddForeignKey("dbo.Message", "Conversation_Id", "dbo.Conversation", "Id");
            AddForeignKey("dbo.UserRoles", "RoleID", "dbo.Role", "Id");
            AddForeignKey("dbo.UserRoles", "UserID", "dbo.Users", "Id");
            DropColumn("dbo.Message", "FromUser");
            DropColumn("dbo.Message", "ToUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Message", "ToUser", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.Message", "FromUser", c => c.String(nullable: false, maxLength: 30));
            DropForeignKey("dbo.UserRoles", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleID", "dbo.Role");
            DropForeignKey("dbo.Message", "Conversation_Id", "dbo.Conversation");
            DropForeignKey("dbo.Conversation", "ToUserId", "dbo.Users");
            DropForeignKey("dbo.Conversation", "FromUserId", "dbo.Users");
            DropIndex("dbo.Message", new[] { "Conversation_Id" });
            DropIndex("dbo.Conversation", new[] { "ToUserId" });
            DropIndex("dbo.Conversation", new[] { "FromUserId" });
            DropColumn("dbo.Users", "LoggedInStatus");
            DropColumn("dbo.Message", "Conversation_Id");
            DropColumn("dbo.Message", "SendBy");
            DropTable("dbo.Conversation");
            AddForeignKey("dbo.UserRoles", "UserID", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserRoles", "RoleID", "dbo.Role", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Message", newName: "Messages");
        }
    }
}
