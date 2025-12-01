using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Airdnd.Migrations
{
    /// <inheritdoc />
    public partial class Phase4_Restructured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SSN = table.Column<string>(type: "TEXT", nullable: true),
                    UserType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Residences",
                columns: table => new
                {
                    ResidenceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ResidencePicture = table.Column<string>(type: "TEXT", nullable: false),
                    GuestNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    BedroomNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    BathroomNumber = table.Column<double>(type: "REAL", nullable: false),
                    PricePerNight = table.Column<double>(type: "REAL", nullable: false),
                    LocationId = table.Column<string>(type: "TEXT", nullable: false),
                    BuiltYear = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residences", x => x.ResidenceId);
                    table.ForeignKey(
                        name: "FK_Residences_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Residences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservationStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReservationEndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ResidenceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservations_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalTable: "Residences",
                        principalColumn: "ResidenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Name" },
                values: new object[,]
                {
                    { "atlanta", "Atlanta" },
                    { "chicago", "Chicago" },
                    { "miami", "Miami" },
                    { "nyc", "New York" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DOB", "Email", "Name", "PhoneNumber", "SSN", "UserType" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner@airdnd.com", "Admin Owner", null, null, "Owner" },
                    { 2, new DateTime(1995, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "client@test.com", "Test Client", null, null, "Client" }
                });

            migrationBuilder.InsertData(
                table: "Residences",
                columns: new[] { "ResidenceId", "BathroomNumber", "BedroomNumber", "BuiltYear", "GuestNumber", "LocationId", "Name", "PricePerNight", "ResidencePicture", "UserId" },
                values: new object[,]
                {
                    { 1, 2.0, 2, 2010, 4, "chicago", "Chicago Loop Apartment", 250.0, "chicago_apt.jpg", 1 },
                    { 2, 3.0, 4, 1995, 8, "atlanta", "Atlanta Suburban House", 320.0, "atlanta_house.jpg", 1 },
                    { 3, 4.5, 5, 2018, 10, "miami", "Miami Beach House", 700.0, "miami_house.jpg", 1 },
                    { 4, 1.0, 1, 1980, 2, "nyc", "NYC Studio", 180.0, "nyc_studio.jpg", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ResidenceId",
                table: "Reservations",
                column: "ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Residences_LocationId",
                table: "Residences",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Residences_UserId",
                table: "Residences",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Residences");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
