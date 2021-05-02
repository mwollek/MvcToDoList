namespace MvcToDoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTask : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        ToDoId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        PlannedFinishDate = c.DateTime(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ProgressState = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ToDoId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Tasks", new[] { "ApplicationUserId" });
            DropTable("dbo.Tasks");
        }
    }
}
