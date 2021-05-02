namespace MvcToDoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskRenameToDoIdTaskId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Tasks");
            DropColumn("dbo.Tasks", "ToDoId");
            AddColumn("dbo.Tasks", "TaskId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tasks", "TaskId");
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "ToDoId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Tasks");
            DropColumn("dbo.Tasks", "TaskId");
            AddPrimaryKey("dbo.Tasks", "ToDoId");
        }
    }
}
