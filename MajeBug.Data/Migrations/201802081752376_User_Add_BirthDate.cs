namespace MajeBug.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Add_BirthDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "BirthDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "BirthDate");
        }
    }
}
