using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace travoul.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    State = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Continent",
                columns: table => new
                {
                    ContinentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Continent", x => x.ContinentId);
                });

            migrationBuilder.CreateTable(
                name: "LocationType",
                columns: table => new
                {
                    LocationTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationType", x => x.LocationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RetroType",
                columns: table => new
                {
                    RetroTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetroType", x => x.RetroTypeId);
                });

            migrationBuilder.CreateTable(
                name: "TravelType",
                columns: table => new
                {
                    TravelTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelType", x => x.TravelTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    TripId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ContinentId = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    TripDates = table.Column<string>(nullable: false),
                    Accommodation = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Budget = table.Column<double>(nullable: true),
                    IsPreTrip = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_Trip_Continent_ContinentId",
                        column: x => x.ContinentId,
                        principalTable: "Continent",
                        principalColumn: "ContinentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trip_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripRetro",
                columns: table => new
                {
                    TripRetroId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TripId = table.Column<int>(nullable: false),
                    RetroTypeId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripRetro", x => x.TripRetroId);
                    table.ForeignKey(
                        name: "FK_TripRetro_RetroType_RetroTypeId",
                        column: x => x.RetroTypeId,
                        principalTable: "RetroType",
                        principalColumn: "RetroTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripRetro_Trip_TripId",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TripTravelType",
                columns: table => new
                {
                    TripTravelTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TripId = table.Column<int>(nullable: false),
                    TravelTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripTravelType", x => x.TripTravelTypeId);
                    table.ForeignKey(
                        name: "FK_TripTravelType_TravelType_TravelTypeId",
                        column: x => x.TravelTypeId,
                        principalTable: "TravelType",
                        principalColumn: "TravelTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripTravelType_Trip_TripId",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TripVisitLocation",
                columns: table => new
                {
                    TripVisitLocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TripId = table.Column<int>(nullable: false),
                    LocationTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsCompleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripVisitLocation", x => x.TripVisitLocationId);
                    table.ForeignKey(
                        name: "FK_TripVisitLocation_LocationType_LocationTypeId",
                        column: x => x.LocationTypeId,
                        principalTable: "LocationType",
                        principalColumn: "LocationTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripVisitLocation_Trip_TripId",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "State", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "32c1d491-5df4-49e1-8cc1-6447588952d7", 0, "Nashville", "dd21e85b-9e40-4aa6-998a-f052d504a171", "kayla@kayla.com", true, "Kayla", "Reid", false, null, "KAYLA@KAYLA.COM", "KAYLA@KAYLA.COM", "AQAAAAEAACcQAAAAEHUM5GEW7Z6B/0nrvJgKJ87KeyZWevH42Fks2uQehXa/h9KRMKz4mONSEAYhq26Ixg==", null, false, "336af88e-f91b-4a2b-a774-ca00f73e0b92", "TN", false, "kayla@kayla.com" },
                    { "2c4f71e2-b63e-46b6-982d-4a5ec8b294b3", 0, "Nashville", "17b96150-0fc4-483e-ad73-047d8603b6a7", "ricky@ricky.com", true, "Ricky", "Bruner", false, null, "RICKY@RICKY.COM", "RICKY@RICKY.COM", "AQAAAAEAACcQAAAAEIaDBY7zAT5RpEb7mTcQeSJwuz/VUACyW2MSxCVwDITYnjVHhiDOZcOhN83aL+rQcg==", null, false, "3e2b2889-a9fe-4777-82b2-d47ac0769559", "TN", false, "ricky@ricky.com" }
                });

            migrationBuilder.InsertData(
                table: "Continent",
                columns: new[] { "ContinentId", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "AF", "Africa" },
                    { 2, "AN", "Antarctica" },
                    { 3, "AS", "Asia" },
                    { 4, "EU", "Europe" },
                    { 5, "NA", "North America" },
                    { 6, "OC", "Oceania" },
                    { 7, "SA", "South America" }
                });

            migrationBuilder.InsertData(
                table: "LocationType",
                columns: new[] { "LocationTypeId", "Type" },
                values: new object[,]
                {
                    { 2, "Place" },
                    { 1, "Food" }
                });

            migrationBuilder.InsertData(
                table: "RetroType",
                columns: new[] { "RetroTypeId", "Type" },
                values: new object[,]
                {
                    { 1, "Do Again" },
                    { 2, "Do Different" }
                });

            migrationBuilder.InsertData(
                table: "TravelType",
                columns: new[] { "TravelTypeId", "Type" },
                values: new object[,]
                {
                    { 7, "Boat" },
                    { 1, "Bus" },
                    { 2, "Plane" },
                    { 3, "Car" },
                    { 4, "Ride Share" },
                    { 5, "Train" },
                    { 6, "Shuttle" },
                    { 8, "Public Transport" }
                });

            migrationBuilder.InsertData(
                table: "Trip",
                columns: new[] { "TripId", "Accommodation", "Budget", "ContinentId", "IsPreTrip", "Location", "Title", "TripDates", "UserId" },
                values: new object[,]
                {
                    { 2, "Campus", 2000.0, 1, false, "Uganda", "Mission Trip", "2010", "32c1d491-5df4-49e1-8cc1-6447588952d7" },
                    { 1, "Cabana Bay", null, 5, false, "Orlando Flordia", "1st Harry Potter World Trip", "5/20/17-5/26/17", "32c1d491-5df4-49e1-8cc1-6447588952d7" },
                    { 3, "Cabana Bay", null, 5, false, "Orlando Flordia", "1st Harry Potter World Trip/Engagement Trip", "5/20/17-5/26/17", "2c4f71e2-b63e-46b6-982d-4a5ec8b294b3" },
                    { 4, "Airbnb", null, 5, true, "Portland OR", "Visit H & T Trip", "2019", "2c4f71e2-b63e-46b6-982d-4a5ec8b294b3" }
                });

            migrationBuilder.InsertData(
                table: "TripRetro",
                columns: new[] { "TripRetroId", "Description", "RetroTypeId", "TripId" },
                values: new object[,]
                {
                    { 3, "It was sooooo amazing!!!!", 1, 2 },
                    { 4, "It was Hot!!!!", 2, 2 },
                    { 1, "It was sooooo amazing!!!!", 1, 1 },
                    { 2, "It was Hot!!!!", 2, 1 },
                    { 5, "It was sooooo amazing!!!!", 1, 3 },
                    { 6, "It was Hot!!!!", 2, 3 }
                });

            migrationBuilder.InsertData(
                table: "TripTravelType",
                columns: new[] { "TripTravelTypeId", "TravelTypeId", "TripId" },
                values: new object[,]
                {
                    { 3, 2, 2 },
                    { 4, 8, 2 },
                    { 8, 3, 4 },
                    { 1, 2, 1 },
                    { 2, 4, 1 },
                    { 7, 2, 4 },
                    { 5, 2, 3 },
                    { 6, 4, 3 }
                });

            migrationBuilder.InsertData(
                table: "TripVisitLocation",
                columns: new[] { "TripVisitLocationId", "Description", "IsCompleted", "LocationTypeId", "Name", "TripId" },
                values: new object[,]
                {
                    { 6, "Its soooo pretty", true, 2, "HP world", 3 },
                    { 5, "Yummy food joint that sells butter beer", true, 1, "The three broomsticks", 3 },
                    { 2, "its sooo pretty", true, 2, "castle", 1 },
                    { 7, "they have awesome stuff", false, 2, "flee market", 4 },
                    { 1, "Yummy food joint that sells butter beer", true, 1, "The three broomsticks", 1 },
                    { 3, "See an elephant", false, 2, "Safari", 2 },
                    { 4, "They have the pink donut thing from the show", false, 1, "Donut Place", 1 },
                    { 8, "can't wait to see their cute place", false, 2, "H and T's house", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_ContinentId",
                table: "Trip",
                column: "ContinentId");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_UserId",
                table: "Trip",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TripRetro_RetroTypeId",
                table: "TripRetro",
                column: "RetroTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TripRetro_TripId",
                table: "TripRetro",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TripTravelType_TravelTypeId",
                table: "TripTravelType",
                column: "TravelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TripTravelType_TripId",
                table: "TripTravelType",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TripVisitLocation_LocationTypeId",
                table: "TripVisitLocation",
                column: "LocationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TripVisitLocation_TripId",
                table: "TripVisitLocation",
                column: "TripId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "TripRetro");

            migrationBuilder.DropTable(
                name: "TripTravelType");

            migrationBuilder.DropTable(
                name: "TripVisitLocation");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "RetroType");

            migrationBuilder.DropTable(
                name: "TravelType");

            migrationBuilder.DropTable(
                name: "LocationType");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropTable(
                name: "Continent");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
