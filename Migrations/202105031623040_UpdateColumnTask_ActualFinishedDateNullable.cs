namespace MvcToDoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnTask_ActualFinishedDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tasks", "ActualFinishedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tasks", "ActualFinishedDate", c => c.DateTime(nullable: false));
        }
    }
}
