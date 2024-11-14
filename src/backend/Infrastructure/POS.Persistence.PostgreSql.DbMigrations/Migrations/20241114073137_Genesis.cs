using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Persistence.PostgreSql.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Genesis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Customer");

            migrationBuilder.CreateTable(
                name: "Menus",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastChangedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    ActivatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuSections",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MenuId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuSections_Menus_MenuId",
                        column: x => x.MenuId,
                        principalSchema: "Customer",
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MenuSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Ingredients = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    PriceCur = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_MenuSections_MenuSectionId",
                        column: x => x.MenuSectionId,
                        principalSchema: "Customer",
                        principalTable: "MenuSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuSectionId",
                schema: "Customer",
                table: "MenuItems",
                column: "MenuSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_IsActive",
                schema: "Customer",
                table: "Menus",
                column: "IsActive",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuSections_MenuId",
                schema: "Customer",
                table: "MenuSections",
                column: "MenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItems",
                schema: "Customer");

            migrationBuilder.DropTable(
                name: "MenuSections",
                schema: "Customer");

            migrationBuilder.DropTable(
                name: "Menus",
                schema: "Customer");
        }
    }
}
