using FluentMigrator;

namespace TaskExplorer.Migrations;

[Migration(3)]
public class AddProjectsMigration : Migration
{
    public override void Up()
    {
        Create.Table("Projects")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Number").AsString().NotNullable()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("CreatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable();
    }

    public override void Down()
    {
        Delete.Table("Projects");
    }
}
