using FluentMigrator;

namespace TaskExplorer.Migrations;

[Migration(5)]
public sealed class AddTasksStatusColumn : Migration
{
    public override void Up()
    {
        Alter.Table("Tasks")
            .AddColumn("Status").AsInt32().NotNullable().WithDefaultValue(0);
    }

    public override void Down()
    {
        Delete.Column("Status").FromTable("Tasks");
    }
}
