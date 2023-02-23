using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AYUS_RestAPI.ASP.Migrations
{
    /// <inheritdoc />
    public partial class migration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_serviceOffers",
                table: "serviceOffers");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceID",
                table: "serviceOffers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UUID",
                table: "serviceOffers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_serviceOffers",
                table: "serviceOffers",
                column: "UUID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_serviceOffers",
                table: "serviceOffers");

            migrationBuilder.DropColumn(
                name: "UUID",
                table: "serviceOffers");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceID",
                table: "serviceOffers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_serviceOffers",
                table: "serviceOffers",
                column: "ServiceID");
        }
    }
}
