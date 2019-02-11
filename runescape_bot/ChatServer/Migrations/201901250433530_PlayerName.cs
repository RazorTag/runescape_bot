namespace ChatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Conversations", name: "Player_PlayerName", newName: "PlayerName");
            RenameIndex(table: "dbo.Conversations", name: "IX_Player_PlayerName", newName: "IX_PlayerName");
            DropColumn("dbo.Conversations", "PlayerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Conversations", "PlayerID", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Conversations", name: "IX_PlayerName", newName: "IX_Player_PlayerName");
            RenameColumn(table: "dbo.Conversations", name: "PlayerName", newName: "Player_PlayerName");
        }
    }
}
