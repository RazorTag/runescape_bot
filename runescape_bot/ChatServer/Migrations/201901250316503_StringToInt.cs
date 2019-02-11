namespace ChatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StringToInt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Conversations", "Player_ID", "dbo.Players");
            DropIndex("dbo.Conversations", new[] { "Player_ID" });
            DropColumn("dbo.Conversations", "PlayerID");
            RenameColumn(table: "dbo.Conversations", name: "Player_ID", newName: "PlayerID");
            AlterColumn("dbo.Conversations", "PlayerID", c => c.Int(nullable: false));
            AlterColumn("dbo.Conversations", "PlayerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Conversations", "PlayerID");
            AddForeignKey("dbo.Conversations", "PlayerID", "dbo.Players", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Conversations", "PlayerID", "dbo.Players");
            DropIndex("dbo.Conversations", new[] { "PlayerID" });
            AlterColumn("dbo.Conversations", "PlayerID", c => c.Int());
            AlterColumn("dbo.Conversations", "PlayerID", c => c.String());
            RenameColumn(table: "dbo.Conversations", name: "PlayerID", newName: "Player_ID");
            AddColumn("dbo.Conversations", "PlayerID", c => c.String());
            CreateIndex("dbo.Conversations", "Player_ID");
            AddForeignKey("dbo.Conversations", "Player_ID", "dbo.Players", "ID");
        }
    }
}
