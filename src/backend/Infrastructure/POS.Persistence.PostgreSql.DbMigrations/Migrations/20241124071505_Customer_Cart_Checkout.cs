using Microsoft.EntityFrameworkCore.Migrations;
using POS.Domains.Customer.Abstractions.Carts;

#nullable disable

namespace POS.Persistence.PostgreSql.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Customer_Cart_Checkout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                schema: "Customer",
                table: "Carts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "Customer",
                table: "Carts",
                keyColumns: [],
                keyValues: [],
                columns: ["State"],
                values: [CartStates.Created.ToString()]
            );

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastChangedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartCheckoutInfos",
                schema: "Customer",
                columns: table => new
                {
                    CartId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CheckedOutAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartCheckoutInfos", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_CartCheckoutInfos_Carts_CartId",
                        column: x => x.CartId,
                        principalSchema: "Customer",
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartCheckoutInfos_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Customer",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CartItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UnitPriceGross = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitPriceNet = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitPriceVat = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitPriceCurrency = table.Column<string>(type: "text", nullable: false),
                    UnitPriceRegVatPercent = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    TotalPriceGross = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPriceNet = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPriceVat = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPriceCurrency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_CartItems_CartItemId",
                        column: x => x.CartItemId,
                        principalSchema: "Customer",
                        principalTable: "CartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Customer",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderPriceInfos",
                schema: "Customer",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalItemPriceGross = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalItemPriceNet = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalItemPriceVat = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalItemPriceCurrency = table.Column<string>(type: "text", nullable: false),
                    TotalPriceGross = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPriceNet = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPriceVat = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPriceCurrency = table.Column<string>(type: "text", nullable: false),
                    DeliverCostsGross = table.Column<decimal>(type: "numeric", nullable: false),
                    DeliverCostsNet = table.Column<decimal>(type: "numeric", nullable: false),
                    DeliverCostsVat = table.Column<decimal>(type: "numeric", nullable: false),
                    DeliverCostsCurrency = table.Column<string>(type: "text", nullable: false),
                    DiscountGross = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountNet = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountVat = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountCurrency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPriceInfos", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_OrderPriceInfos_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Customer",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartCheckoutInfos_OrderId",
                schema: "Customer",
                table: "CartCheckoutInfos",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CartItemId",
                schema: "Customer",
                table: "OrderItems",
                column: "CartItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                schema: "Customer",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartCheckoutInfos",
                schema: "Customer");

            migrationBuilder.DropTable(
                name: "OrderItems",
                schema: "Customer");

            migrationBuilder.DropTable(
                name: "OrderPriceInfos",
                schema: "Customer");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "Customer");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "Customer",
                table: "Carts");
        }
    }
}
