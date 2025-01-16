using FluentMigrator;

namespace TaskExplorer.Migrations;

[Migration(2)]
public class AddTasksMigration : Migration
{
    public override void Up()
    {
        Create.Table("Tasks")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Number").AsString().NotNullable()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Description").AsString().NotNullable()
            .WithColumn("DueDate").AsDateTimeOffset().Nullable()
            .WithColumn("ProjectId").AsInt32().Nullable()
            .WithColumn("SprintId").AsInt32().Nullable()
            .WithColumn("CreatorLogin").AsString().NotNullable()
            .WithColumn("AssignedToLogin").AsString().Nullable()
            .WithColumn("ReporterLogin").AsString().Nullable()
            .WithColumn("CreatedAt").AsDateTimeOffset().Nullable()
            .WithColumn("UpdatedAt").AsDateTimeOffset().Nullable();
    }

    public override void Down()
    {
        Delete.Table("Users");
    }
}
