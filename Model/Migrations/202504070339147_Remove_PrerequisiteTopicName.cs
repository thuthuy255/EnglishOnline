namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_PrerequisiteTopicName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Topics", "PrerequisiteTopicId");
            RenameColumn(table: "dbo.Topics", name: "PrerequisiteTopic_TopicID", newName: "PrerequisiteTopicId");
            RenameIndex(table: "dbo.Topics", name: "IX_PrerequisiteTopic_TopicID", newName: "IX_PrerequisiteTopicId");
            DropColumn("dbo.Topics", "PrerequisiteTopicName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Topics", "PrerequisiteTopicName", c => c.String());
            RenameIndex(table: "dbo.Topics", name: "IX_PrerequisiteTopicId", newName: "IX_PrerequisiteTopic_TopicID");
            RenameColumn(table: "dbo.Topics", name: "PrerequisiteTopicId", newName: "PrerequisiteTopic_TopicID");
            AddColumn("dbo.Topics", "PrerequisiteTopicId", c => c.Guid());
        }
    }
}
