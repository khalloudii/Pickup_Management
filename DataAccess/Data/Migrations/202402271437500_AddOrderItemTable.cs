namespace DataAccess.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderItemTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "OrderId", "dbo.Orders");
            DropIndex("dbo.Items", new[] { "OrderId" });
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.ItemID)
                .Index(t => t.OrderId);
            
            AddColumn("dbo.Items", "InitialBalance", c => c.Int(nullable: false));
            AddColumn("dbo.Items", "CurrentBalance", c => c.Int(nullable: false));
            AddColumn("dbo.Items", "Amount", c => c.Double(nullable: false));
            DropColumn("dbo.Items", "Quantity");
            DropColumn("dbo.Items", "OrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "OrderId", c => c.Int(nullable: false));
            AddColumn("dbo.Items", "Quantity", c => c.Int(nullable: false));
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderItems", "ItemID", "dbo.Items");
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            DropIndex("dbo.OrderItems", new[] { "ItemID" });
            DropColumn("dbo.Items", "Amount");
            DropColumn("dbo.Items", "CurrentBalance");
            DropColumn("dbo.Items", "InitialBalance");
            DropTable("dbo.OrderItems");
            CreateIndex("dbo.Items", "OrderId");
            AddForeignKey("dbo.Items", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
    }
}
