namespace DataAccess.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Address1", c => c.String());
            AddColumn("dbo.Orders", "Address2", c => c.String());
            AddColumn("dbo.Orders", "Street", c => c.String());
            AddColumn("dbo.Orders", "District", c => c.String());
            AddColumn("dbo.Orders", "PostCode", c => c.String());
            AddColumn("dbo.Orders", "PaymentLink", c => c.String());
            AddColumn("dbo.Orders", "IsPaid", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "Country");
            DropColumn("dbo.Orders", "PostalCode");
            DropColumn("dbo.Orders", "Building");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Building", c => c.String());
            AddColumn("dbo.Orders", "PostalCode", c => c.String());
            AddColumn("dbo.Orders", "Country", c => c.String());
            DropColumn("dbo.Orders", "IsPaid");
            DropColumn("dbo.Orders", "PaymentLink");
            DropColumn("dbo.Orders", "PostCode");
            DropColumn("dbo.Orders", "District");
            DropColumn("dbo.Orders", "Street");
            DropColumn("dbo.Orders", "Address2");
            DropColumn("dbo.Orders", "Address1");
        }
    }
}
