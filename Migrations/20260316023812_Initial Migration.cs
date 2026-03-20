using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_Brand",
                columns: table => new
                {
                    BrandID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Brand", x => x.BrandID);
                });

            migrationBuilder.CreateTable(
                name: "tb_ProductCategory",
                columns: table => new
                {
                    CateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CateName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SeoTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Statuss = table.Column<bool>(type: "bit", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ProductCategory", x => x.CateID);
                    table.ForeignKey(
                        name: "FK_tb_ProductCategory_tb_ProductCategory_ParentID",
                        column: x => x.ParentID,
                        principalTable: "tb_ProductCategory",
                        principalColumn: "CateID");
                });

            migrationBuilder.CreateTable(
                name: "tb_Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "tb_Shop",
                columns: table => new
                {
                    ShopID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShopDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShopAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalProducts = table.Column<int>(type: "int", nullable: false),
                    TotalFollowers = table.Column<int>(type: "int", nullable: false),
                    RatingAverage = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    TotalReviews = table.Column<int>(type: "int", nullable: false),
                    TotalSold = table.Column<int>(type: "int", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Shop", x => x.ShopID);
                    table.ForeignKey(
                        name: "FK_tb_Shop_tb_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "tb_Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    PhoneVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_tb_Users_tb_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SeoTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ListImages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PromotionPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VAT = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Hot = table.Column<bool>(type: "bit", nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    MetaKeywords = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CateID = table.Column<int>(type: "int", nullable: true),
                    BrandID = table.Column<int>(type: "int", nullable: true),
                    ShopID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_tb_Product_tb_Brand_BrandID",
                        column: x => x.BrandID,
                        principalTable: "tb_Brand",
                        principalColumn: "BrandID");
                    table.ForeignKey(
                        name: "FK_tb_Product_tb_ProductCategory_CateID",
                        column: x => x.CateID,
                        principalTable: "tb_ProductCategory",
                        principalColumn: "CateID");
                    table.ForeignKey(
                        name: "FK_tb_Product_tb_Shop_ShopID",
                        column: x => x.ShopID,
                        principalTable: "tb_Shop",
                        principalColumn: "ShopID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_Carts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Carts", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_tb_Carts_tb_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "tb_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_Order",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Delivered = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Order", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_tb_Order_tb_Users_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "tb_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_CartItems",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_CartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_tb_CartItems_tb_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "tb_Carts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_CartItems_tb_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "tb_Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ShopId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_tb_OrderDetails_tb_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "tb_Order",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK_tb_OrderDetails_tb_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tb_Product",
                        principalColumn: "ProductID");
                    table.ForeignKey(
                        name: "FK_tb_OrderDetails_tb_Shop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "tb_Shop",
                        principalColumn: "ShopID");
                });

            migrationBuilder.InsertData(
                table: "tb_Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" },
                    { 3, "Shop" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_CartItems_CartId",
                table: "tb_CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_CartItems_ProductID",
                table: "tb_CartItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Carts_CustomerId",
                table: "tb_Carts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Order_CustomerID",
                table: "tb_Order",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_OrderDetails_OrderId",
                table: "tb_OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_OrderDetails_ProductId",
                table: "tb_OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_OrderDetails_ShopId",
                table: "tb_OrderDetails",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Product_BrandID",
                table: "tb_Product",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Product_CateID",
                table: "tb_Product",
                column: "CateID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Product_ShopID",
                table: "tb_Product",
                column: "ShopID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ProductCategory_ParentID",
                table: "tb_ProductCategory",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Shop_RoleId",
                table: "tb_Shop",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Users_RoleId",
                table: "tb_Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_CartItems");

            migrationBuilder.DropTable(
                name: "tb_OrderDetails");

            migrationBuilder.DropTable(
                name: "tb_Carts");

            migrationBuilder.DropTable(
                name: "tb_Order");

            migrationBuilder.DropTable(
                name: "tb_Product");

            migrationBuilder.DropTable(
                name: "tb_Users");

            migrationBuilder.DropTable(
                name: "tb_Brand");

            migrationBuilder.DropTable(
                name: "tb_ProductCategory");

            migrationBuilder.DropTable(
                name: "tb_Shop");

            migrationBuilder.DropTable(
                name: "tb_Roles");
        }
    }
}
