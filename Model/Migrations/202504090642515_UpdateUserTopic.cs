namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserTopic : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserTopics", "Locked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserTopics", "Locked", c => c.Boolean(nullable: false));
        }
    }
}
