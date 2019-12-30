namespace ChatTU.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableRenamesToConvention : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Conversation", newName: "Conversations");
            RenameTable(name: "dbo.Role", newName: "Roles");
            RenameTable(name: "dbo.File", newName: "Files");
            RenameTable(name: "dbo.Message", newName: "Messages");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Messages", newName: "Message");
            RenameTable(name: "dbo.Files", newName: "File");
            RenameTable(name: "dbo.Roles", newName: "Role");
            RenameTable(name: "dbo.Conversations", newName: "Conversation");
        }
    }
}
