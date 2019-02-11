namespace ChatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlayerName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SpeakerName = c.String(),
                        PlayerName = c.String(),
                        Player_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Players", t => t.Player_ID)
                .Index(t => t.Player_ID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Moment = c.DateTime(nullable: false),
                        ConversationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Conversations", t => t.ConversationID, cascadeDelete: true)
                .Index(t => t.ConversationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Conversations", "Player_ID", "dbo.Players");
            DropForeignKey("dbo.Comments", "ConversationID", "dbo.Conversations");
            DropIndex("dbo.Comments", new[] { "ConversationID" });
            DropIndex("dbo.Conversations", new[] { "Player_ID" });
            DropTable("dbo.Comments");
            DropTable("dbo.Conversations");
            DropTable("dbo.Players");
        }
    }
}
