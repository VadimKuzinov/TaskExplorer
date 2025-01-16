using FluentMigrator;

namespace TaskExplorer.Migrations;

[Migration(1)]
public class AddUsersMigration : Migration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("FirstName").AsString().NotNullable()
            .WithColumn("LastName").AsString().NotNullable()
            .WithColumn("UserName").AsString().NotNullable().Unique()
            .WithColumn("Password").AsString().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Users");
    }
}
