using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AYUS_RestAPI.ASP.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accountStatus",
                columns: table => new
                {
                    UUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountStatus", x => x.UUID);
                });

            migrationBuilder.CreateTable(
                name: "credential",
                columns: table => new
                {
                    UUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credential", x => x.UUID);
                });

            migrationBuilder.CreateTable(
                name: "personalInformation",
                columns: table => new
                {
                    UUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personalInformation", x => x.UUID);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    ServiceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.ServiceID);
                });

            migrationBuilder.CreateTable(
                name: "shops",
                columns: table => new
                {
                    ShopID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShopDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shops", x => x.ShopID);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    PlateNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.PlateNumber);
                });

            migrationBuilder.CreateTable(
                name: "wallets",
                columns: table => new
                {
                    UUID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallets", x => x.UUID);
                });

            migrationBuilder.CreateTable(
                name: "billing",
                columns: table => new
                {
                    BillingID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BillingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceFee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceRemark = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billing", x => x.BillingID);
                    table.ForeignKey(
                        name: "FK_billing_shops_ShopID",
                        column: x => x.ShopID,
                        principalTable: "shops",
                        principalColumn: "ShopID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "serviceOffers",
                columns: table => new
                {
                    ServiceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceExpertise = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShopID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serviceOffers", x => x.ServiceID);
                    table.ForeignKey(
                        name: "FK_serviceOffers_shops_ShopID",
                        column: x => x.ShopID,
                        principalTable: "shops",
                        principalColumn: "ShopID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_billing_ShopID",
                table: "billing",
                column: "ShopID");

            migrationBuilder.CreateIndex(
                name: "IX_serviceOffers_ShopID",
                table: "serviceOffers",
                column: "ShopID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accountStatus");

            migrationBuilder.DropTable(
                name: "billing");

            migrationBuilder.DropTable(
                name: "credential");

            migrationBuilder.DropTable(
                name: "personalInformation");

            migrationBuilder.DropTable(
                name: "serviceOffers");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "wallets");

            migrationBuilder.DropTable(
                name: "shops");
        }
    }
}
