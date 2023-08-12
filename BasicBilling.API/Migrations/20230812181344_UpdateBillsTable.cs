using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicBilling.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBillsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "MonthYear",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Bills",
                newName: "Category");

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "Bills",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Period",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Bills",
                newName: "Type");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Bills",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "MonthYear",
                table: "Bills",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
