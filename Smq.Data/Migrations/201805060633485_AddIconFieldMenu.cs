namespace Smq.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIconFieldMenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Icon", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Menus", "Icon");
        }
    }
}
