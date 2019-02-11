namespace ChatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConversationComment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Conversations", "PlayerID", "dbo.Players");
            DropIndex("dbo.Conversations", new[] { "PlayerID" });
            DropPrimaryKey("dbo.Players");
            AddColumn("dbo.Conversations", "Player_PlayerName", c => c.String(maxLength: 128));
            AlterColumn("dbo.Players", "PlayerName", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Players", "PlayerName");
            CreateIndex("dbo.Conversations", "Player_PlayerName");
            AddForeignKey("dbo.Conversations", "Player_PlayerName", "dbo.Players", "PlayerName");
            DropColumn("dbo.Players", "ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "ID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Conversations", "Player_PlayerName", "dbo.Players");
            DropIndex("dbo.Conversations", new[] { "Player_PlayerName" });
            DropPrimaryKey("dbo.Players");
            AlterColumn("dbo.Players", "PlayerName", c => c.String());
            DropColumn("dbo.Conversations", "Player_PlayerName");
            AddPrimaryKey("dbo.Players", "ID");
            CreateIndex("dbo.Conversations", "PlayerID");
            AddForeignKey("dbo.Conversations", "PlayerID", "dbo.Players", "ID", cascadeDelete: true);
        }
    }
}
