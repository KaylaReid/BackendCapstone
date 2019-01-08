using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace travoul.Migrations
{
    public partial class Initial : Migration
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
                    ImagePath = table.Column<string>(nullable: true),
                    DateFinished = table.Column<DateTime>(nullable: false),
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
                    { "c95a6a90-19a1-4ccd-b028-9776d91eb991", 0, "Nashville", "95a55c12-e1ec-4529-9215-8286d6a64458", "kayla@kayla.com", true, "Kayla", "Reid", false, null, "KAYLA@KAYLA.COM", "KAYLA@KAYLA.COM", "AQAAAAEAACcQAAAAEKHNVJaXO1l3fvMIn7qERtK8LUTuGHd0l4bTzXQQ9MefhNM/+b55r+zfY7yjMKUzRw==", null, false, "684a060d-cb5b-4836-9695-716e77ac8a2b", "TN", false, "kayla@kayla.com" },
                    { "2afae0c5-52dc-4524-acee-7e9dc85d4995", 0, "Nashville", "0c13c6b9-b09d-4da2-b326-83e9c464a8c0", "ricky@ricky.com", true, "Ricky", "Bruner", false, null, "RICKY@RICKY.COM", "RICKY@RICKY.COM", "AQAAAAEAACcQAAAAELS0WBJo5AG8Jz3dxTdbZHkNmaW0gD+5PF/hzo5zWhrmGaA0K25zR02fVrzRMz3o7Q==", null, false, "9894671a-a56a-49da-b3ee-38a7be301d36", "TN", false, "ricky@ricky.com" }
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
                columns: new[] { "TripId", "Accommodation", "Budget", "ContinentId", "DateFinished", "ImagePath", "IsPreTrip", "Location", "Title", "TripDates", "UserId" },
                values: new object[,]
                {
                    { 2, "Campus", 2000.0, 1, new DateTime(2019, 1, 7, 20, 17, 11, 128, DateTimeKind.Local), "https://images.unsplash.com/photo-1516026672322-bc52d61a55d5?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60", false, "Uganda", "Mission Trip", "2010", "c95a6a90-19a1-4ccd-b028-9776d91eb991" },
                    { 1, "Cabana Bay", null, 5, new DateTime(2019, 1, 7, 20, 17, 11, 125, DateTimeKind.Local), "https://scontent.fbna1-1.fna.fbcdn.net/v/t31.0-8/19221504_10156323805382729_8735263925981311453_o.jpg?_nc_cat=111&_nc_ht=scontent.fbna1-1.fna&oh=27f178631784a8d9e88e2b7a117e8948&oe=5CC3F599", false, "Orlando Florida", "1st Harry Potter World Trip", "5/20/17-5/26/17", "c95a6a90-19a1-4ccd-b028-9776d91eb991" },
                    { 3, "Cabana Bay", null, 5, new DateTime(2019, 1, 7, 20, 17, 11, 128, DateTimeKind.Local), "https://scontent.fbna1-1.fna.fbcdn.net/v/t31.0-8/19221723_10156323925257729_6632941831644369725_o.jpg?_nc_cat=108&_nc_ht=scontent.fbna1-1.fna&oh=25026fc7793a7dea17da4116d35de668&oe=5CC052A1", false, "Orlando Flordia", "1st Harry Potter World Trip/Engagement Trip", "5/20/17-5/26/17", "2afae0c5-52dc-4524-acee-7e9dc85d4995" },
                    { 4, "Airbnb", null, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Portland OR", "Visit H & T Trip", "2019", "2afae0c5-52dc-4524-acee-7e9dc85d4995" }
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
