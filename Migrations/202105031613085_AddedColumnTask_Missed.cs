namespace MvcToDoList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumnTask_Missed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Missed", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Missed");
        }
    }
}
