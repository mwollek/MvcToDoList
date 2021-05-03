namespace MvcToDoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumnTask_ActualFinishedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "ActualFinishedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "ActualFinishedDate");
        }
    }
}
