namespace Tider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RealName", c => c.String());
            DropColumn("dbo.AspNetUsers", "Nickname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Nickname", c => c.String());
            DropColumn("dbo.AspNetUsers", "RealName");
        }
    }
}
