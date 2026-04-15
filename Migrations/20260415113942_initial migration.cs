using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
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
                name: "tb_Promotion",
                columns: table => new
                {
                    PromotionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiscountPercent = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Promotion", x => x.PromotionID);
                });

            migrationBuilder.CreateTable(
                name: "tb_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_SystemSetting",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaintenanceMode = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_SystemSetting", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tb_Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_RoleClaims_tb_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_Roles",
                        principalColumn: "Id",
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
                        principalColumn: "Id",
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_Shop",
                columns: table => new
                {
                    ShopID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
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
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Shop", x => x.ShopID);
                    table.ForeignKey(
                        name: "FK_tb_Shop_tb_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tb_Shop_tb_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "tb_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tb_SystemLog",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_SystemLog", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_tb_SystemLog_tb_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "tb_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_UserClaims_tb_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "tb_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_tb_UserLogins_tb_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "tb_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_tb_UserRoles_tb_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tb_UserRoles_tb_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "tb_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tb_UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_tb_UserTokens_tb_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "tb_Users",
                        principalColumn: "Id",
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
                name: "tb_ShopReview",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopId = table.Column<int>(type: "int", nullable: false),
                    ReviewerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_ShopReview", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_tb_ShopReview_tb_Shop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "tb_Shop",
                        principalColumn: "ShopID",
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
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
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
                table: "tb_Brand",
                columns: new[] { "BrandID", "BrandName" },
                values: new object[,]
                {
                    { 1, "Nike" },
                    { 2, "Adidas" },
                    { 3, "Uniqlo" },
                    { 4, "Zara" },
                    { 5, "H&M" }
                });

            migrationBuilder.InsertData(
                table: "tb_ProductCategory",
                columns: new[] { "CateID", "CateName", "CreatedBy", "CreatedDate", "MetaDescription", "MetaKeywords", "ParentID", "SeoTitle", "Sort", "Statuss", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Men's Fashion", null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "High quality clothing for men", "men clothing, fashion", null, "mens-fashion", 1, true, null, null },
                    { 2, "Women's Fashion", null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest fashion trends for women", "dresses, women fashion", null, "womens-fashion", 2, true, null, null },
                    { 3, "Accessories", null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fashion accessories for everyone", "bags, watches, jewelry", null, "accessories", 3, true, null, null }
                });

            migrationBuilder.InsertData(
                table: "tb_Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "4828f4c2-ddb3-4a59-9037-eb3f8db83436", "Admin", "ADMIN" },
                    { 2, "c227cf8f-7cc5-4830-b0a8-cdf8bd07bd9f", "User", "USER" },
                    { 3, "677cdaa2-a80c-4501-8cb8-737c548eef24", "Shop", "SHOP" }
                });

            migrationBuilder.InsertData(
                table: "tb_Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AvatarUrl", "City", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "EmailVerified", "FullName", "Gender", "IsActive", "IsBanned", "LastLogin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhoneVerified", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { 1, 0, null, null, null, "5f752bdb-1878-41cd-a23d-48510b8b7528", null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "admin@fashionstore.com", true, false, "System Administrator", null, true, false, null, false, null, "ADMIN@FASHIONSTORE.COM", "ADMIN@FASHIONSTORE.COM", "AQAAAAIAAYagAAAAEE9ZB7nugbR0USVfdbscK+65vP4r7dypDXAWmNjvreTggBlKL+hezMuBMzahDKNCqw==", null, false, false, "bc1ac220-d26d-4a45-b229-78143b32d605", false, null, "admin@fashionstore.com" });

            migrationBuilder.InsertData(
                table: "tb_ProductCategory",
                columns: new[] { "CateID", "CateName", "CreatedBy", "CreatedDate", "MetaDescription", "MetaKeywords", "ParentID", "SeoTitle", "Sort", "Statuss", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 4, "Men's Torso", null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stylish shirts for men", "t-shirts, polo", 1, "mens-torso", 1, true, null, null },
                    { 5, "Men's Leggings", null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comfortable pants for men", "jeans, trousers", 1, "mens-leggings", 2, true, null, null },
                    { 6, "Dresses & Skirts", null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beautiful dresses for ladies", "maxi dress, skirts", 2, "dresses-skirts", 1, true, null, null },
                    { 7, "Women's Handbags", null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luxury handbags for women", "purses, totes", 2, "womens-handbags", 2, true, null, null },
                    { 8, "Footwear", null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quality footwear for all ages", "shoes, sneakers", 3, "footwear", 1, true, null, null },
                    { 9, "Watches & Jewelry", null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium timepieces and jewelry", "gold, luxury watches", 3, "watches-jewelry", 2, true, null, null }
                });

            migrationBuilder.InsertData(
                table: "tb_Shop",
                columns: new[] { "ShopID", "City", "ContactEmail", "ContactPhone", "Country", "CoverImageUrl", "CreatedAt", "IsActive", "IsBanned", "IsVerified", "LogoUrl", "OwnerId", "RatingAverage", "RoleId", "ShopAddress", "ShopDescription", "ShopName", "TotalFollowers", "TotalProducts", "TotalReviews", "TotalSold", "UpdatedAt" },
                values: new object[] { 1, "Ho Chi Minh City", "support@urbanchic.com", "+84987654321", "Vietnam", "cover-urban.jpg", new DateTime(2026, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, true, "logo-urban.png", null, 4.80m, 2, "123 ABC Street, District 1", "Premium streetwear and modern fashion trends for the young generation.", "Urban Chic Fashion", 1200, 50, 450, 2500, null });

            migrationBuilder.InsertData(
                table: "tb_UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "tb_Product",
                columns: new[] { "ProductID", "BrandID", "CateID", "CreatedBy", "CreatedDate", "Detail", "Hot", "Image", "ListImages", "MetaDescription", "MetaKeywords", "Price", "ProductDescription", "ProductName", "PromotionPrice", "Quantity", "SeoTitle", "ShopID", "Status", "UpdatedBy", "UpdatedDate", "VAT", "ViewCount" },
                values: new object[,]
                {
                    { 1, 1, 1, null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "<p>Breathable fabric, perfect for office and formal events.</p>", true, "white-shirt.jpg", "img1.jpg,img2.jpg", "Buy premium white oxford shirt at the best price.", "white shirt, oxford shirt, formal", 45000m, "Classic fit white oxford shirt made from 100% cotton.", "Premium White Oxford Shirt", 39.99m, 100, "premium-white-oxford-shirt", 1, true, null, null, true, 0 },
                    { 2, 1, 2, null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "<p>Soft chiffon material with adjustable waist strap.</p>", false, "floral-dress.jpg", "img3.jpg,img4.jpg", "Beautiful floral dress for your summer vacation.", "summer dress, floral dress, maxi dress", 55000m, "Elegant floral print dress for summer outings.", "Floral Summer Maxi Dress", 49.00m, 50, "floral-summer-maxi-dress", 1, true, null, null, true, 0 }
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
                name: "IX_tb_RoleClaims_RoleId",
                table: "tb_RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "tb_Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Shop_OwnerId",
                table: "tb_Shop",
                column: "OwnerId",
                unique: true,
                filter: "[OwnerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Shop_RoleId",
                table: "tb_Shop",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ShopReview_ShopId",
                table: "tb_ShopReview",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_SystemLog_UserId",
                table: "tb_SystemLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_UserClaims_UserId",
                table: "tb_UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_UserLogins_UserId",
                table: "tb_UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_UserRoles_RoleId",
                table: "tb_UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "tb_Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "tb_Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_CartItems");

            migrationBuilder.DropTable(
                name: "tb_OrderDetails");

            migrationBuilder.DropTable(
                name: "tb_Promotion");

            migrationBuilder.DropTable(
                name: "tb_RoleClaims");

            migrationBuilder.DropTable(
                name: "tb_ShopReview");

            migrationBuilder.DropTable(
                name: "tb_SystemLog");

            migrationBuilder.DropTable(
                name: "tb_SystemSetting");

            migrationBuilder.DropTable(
                name: "tb_UserClaims");

            migrationBuilder.DropTable(
                name: "tb_UserLogins");

            migrationBuilder.DropTable(
                name: "tb_UserRoles");

            migrationBuilder.DropTable(
                name: "tb_UserTokens");

            migrationBuilder.DropTable(
                name: "tb_Carts");

            migrationBuilder.DropTable(
                name: "tb_Order");

            migrationBuilder.DropTable(
                name: "tb_Product");

            migrationBuilder.DropTable(
                name: "tb_Brand");

            migrationBuilder.DropTable(
                name: "tb_ProductCategory");

            migrationBuilder.DropTable(
                name: "tb_Shop");

            migrationBuilder.DropTable(
                name: "tb_Roles");

            migrationBuilder.DropTable(
                name: "tb_Users");
        }
    }
}
