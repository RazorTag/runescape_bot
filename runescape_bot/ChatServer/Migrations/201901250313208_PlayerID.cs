namespace ChatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Conversations", "PlayerID", c => c.String());
            DropColumn("dbo.Conversations", "PlayerName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Conversations", "PlayerName", c => c.String());
            DropColumn("dbo.Conversations", "PlayerID");
        }
    }
}
