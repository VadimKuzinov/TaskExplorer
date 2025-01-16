using FluentMigrator;

namespace TaskExplorer.Migrations;

[Migration(6)]
public sealed class AddTasksProjectTitleColumn : Migration
{
    public override void Up()
    {
        Alter.Table("Tasks")
            .AddColumn("ProjectTitle").AsString().Nullable();
    }

    public override void Down()
    {
        Delete.Column("ProjectTitle").FromTable("Tasks");
    }
}
