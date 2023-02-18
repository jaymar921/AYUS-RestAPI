using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AYUS_RestAPI.ASP.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_serviceOffers_shops_ShopID",
                table: "serviceOffers");

            migrationBuilder.AlterColumn<string>(
                name: "ShopID",
                table: "serviceOffers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_serviceOffers_shops_ShopID",
                table: "serviceOffers",
                column: "ShopID",
                principalTable: "shops",
                principalColumn: "ShopID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_serviceOffers_shops_ShopID",
                table: "serviceOffers");

            migrationBuilder.AlterColumn<string>(
                name: "ShopID",
                table: "serviceOffers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_serviceOffers_shops_ShopID",
                table: "serviceOffers",
                column: "ShopID",
                principalTable: "shops",
                principalColumn: "ShopID");
        }
    }
}
