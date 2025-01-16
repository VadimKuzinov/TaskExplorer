using FluentMigrator;

namespace TaskExplorer.Migrations;

[Migration(4)]
public class AddTasksNumberValueColumn : Migration
{
    public override void Up()
    {
        Delete.Column("Number").FromTable("Tasks");
        Alter.Table("Tasks")
            .AddColumn("NumberPrefix").AsString().NotNullable()
            .AddColumn("NumberValue").AsInt32().NotNullable();
    }

    public override void Down()
    {
        Delete.Column("NumberPrefix").FromTable("Tasks");
        Delete.Column("NumberValue").FromTable("Tasks");
        Alter.Table("Tasks")
            .AddColumn("Number").AsString().NotNullable();
    }
}
