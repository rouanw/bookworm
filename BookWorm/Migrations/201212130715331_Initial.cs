namespace BookWorm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Subtitle = c.String(),
                        Author = c.String(nullable: false),
                        Illustrator = c.String(),
                        Editor = c.String(nullable: false),
                        Isbn = c.String(nullable: false),
                        Publisher = c.String(nullable: false),
                        Language = c.String(nullable: false),
                        Genre = c.String(nullable: false),
                        RecommendedAgeGroup = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        CoverImageUrl = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StaticPages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 20),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StaticPages");
            DropTable("dbo.Books");
        }
    }
}
