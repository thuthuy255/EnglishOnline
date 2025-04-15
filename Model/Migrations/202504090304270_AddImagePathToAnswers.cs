namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImagePathToAnswers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "ImagePath", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "ImagePath");
        }
    }
}
