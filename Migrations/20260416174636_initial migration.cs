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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Delivered = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromotionId = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_tb_Product_tb_Promotion_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "tb_Promotion",
                        principalColumn: "PromotionID");
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
                    { 1, "ef13c806-04c4-49cf-bc8c-db22786294d4", "Admin", "ADMIN" },
                    { 2, "05dfec67-bfca-4f86-afb4-768728a5d7b7", "User", "USER" },
                    { 3, "4845c675-36ee-46c8-8fe3-091fb3126fa2", "Shop", "SHOP" }
                });

            migrationBuilder.InsertData(
                table: "tb_Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "AvatarUrl", "City", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "EmailVerified", "FullName", "Gender", "IsActive", "IsBanned", "LastLogin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhoneVerified", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { 1, 0, null, null, null, "881370c9-277a-49c8-914c-8ad53940efa7", null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "admin@fashionstore.com", true, false, "System Administrator", null, true, false, null, false, null, "ADMIN@FASHIONSTORE.COM", "ADMIN@FASHIONSTORE.COM", "AQAAAAIAAYagAAAAEH25h7ZQvQE8hOwTaBbjOB32HV7wnHtkWSfxmcVzInrNEyS3y96uH2cNkm9/UPRG1A==", null, false, false, "23e669af-337e-4e6a-910b-8f6801457c72", false, null, "admin@fashionstore.com" });

            migrationBuilder.InsertData(
                table: "tb_Order",
                columns: new[] { "OrderID", "CreatedDate", "CustomerID", "Delivered", "DeliveryDate", "Discount", "OrderDate", "OrderStatus", "PaymentStatus", "TotalPrice" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 17, 0, 46, 35, 686, DateTimeKind.Local).AddTicks(486), 1, true, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "Paid", 1250000m },
                    { 2, new DateTime(2026, 4, 17, 0, 46, 35, 686, DateTimeKind.Local).AddTicks(495), 1, true, new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "Paid", 850000m },
                    { 3, new DateTime(2026, 4, 17, 0, 46, 35, 686, DateTimeKind.Local).AddTicks(498), 1, true, new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "Paid", 2100000m },
                    { 4, new DateTime(2026, 4, 17, 0, 46, 35, 686, DateTimeKind.Local).AddTicks(500), 1, true, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "Paid", 1500000m },
                    { 5, new DateTime(2026, 4, 17, 0, 46, 35, 686, DateTimeKind.Local).AddTicks(503), 1, true, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "Paid", 3200000m },
                    { 6, new DateTime(2026, 4, 17, 0, 46, 35, 686, DateTimeKind.Local).AddTicks(505), 1, true, new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "Paid", 950000m },
                    { 7, new DateTime(2026, 4, 17, 0, 46, 35, 686, DateTimeKind.Local).AddTicks(507), 1, true, new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "Paid", 1800000m }
                });

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
                columns: new[] { "ProductID", "BrandID", "CateID", "CreatedBy", "CreatedDate", "Detail", "Hot", "Image", "ListImages", "MetaDescription", "MetaKeywords", "Price", "ProductDescription", "ProductName", "PromotionId", "PromotionPrice", "Quantity", "SeoTitle", "ShopID", "Status", "UpdatedBy", "UpdatedDate", "VAT", "ViewCount" },
                values: new object[,]
                {
                    { 1, 3, 4, null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "100% Cotton", true, "white-shirt.jpg", "img1.jpg,img2.jpg", "shirt", "shirt", 450000m, "Classic fit white oxford shirt", "Premium White Oxford Shirt", null, 399000m, 100, "premium-white-oxford-shirt", 1, true, null, null, true, 0 },
                    { 2, 4, 6, null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chiffon material", false, "floral-dress.jpg", "img3.jpg,img4.jpg", "dress", "dress", 550000m, "Elegant floral print dress", "Floral Summer Maxi Dress", null, 490000m, 50, "floral-summer-maxi-dress", 1, true, null, null, true, 0 },
                    { 3, 2, 5, null, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stretchy fabric", true, "anh1sanpham.jpg", "img5.jpg", "pants", "chinos", 600000m, "High-quality khaki pants", "Slim Fit Navy Chinos", null, 550000m, 80, "slim-fit-navy-chinos", 1, true, null, null, true, 0 },
                    { 4, 5, 7, null, new DateTime(2026, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Handmade", false, "anh2sanpham.jpg", "img6.jpg", "bag", "bag", 1200000m, "Genuine leather bag", "Leather Crossbody Bag", null, 990000m, 30, "leather-crossbody-bag", 1, true, null, null, true, 0 },
                    { 5, 1, 9, null, new DateTime(2026, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Waterproof 50m", true, "anh3sanpham.jpg", "img7.jpg", "watch", "watch", 3500000m, "Elegant gold-plated watch", "Classic Gold Watch", null, 3200000m, 15, "classic-gold-watch", 1, true, null, null, true, 0 },
                    { 6, 1, 8, null, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Breathable mesh upper", true, "anh4sanpham.jpg", "nike1.jpg,nike2.jpg", "nike air max", "nike, shoes", 3500000m, "Advanced cushioning for daily comfort.", "Nike Air Max 270", null, 3200000m, 40, "nike-air-max-270", 1, true, null, null, true, 0 },
                    { 7, 2, 8, null, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Primeknit upper", true, "anh5sanpham.jpg", "adi1.jpg,adi2.jpg", "adidas ultraboost", "adidas, running", 4200000m, "Ultimate energy return for runners.", "Adidas Ultraboost 22", null, 3800000m, 35, "adidas-ultraboost-22", 1, true, null, null, true, 0 },
                    { 8, 3, 4, null, new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Airism technology", false, "anh6sanpham.jpg", "uni1.jpg", "airism shirt", "uniqlo, t-shirt", 350000m, "Smooth and quick-drying fabric.", "Uniqlo Airism T-Shirt", null, 299000m, 200, "uniqlo-airism-tshirt", 1, true, null, null, true, 0 },
                    { 9, 4, 4, null, new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium wool blend", true, "anh7sanpham.jpg", "zara1.jpg", "slim fit suit", "zara, suit", 2500000m, "Modern slim fit for formal events.", "Zara Slim Fit Suit", null, 2100000m, 20, "zara-slim-suit", 1, true, null, null, true, 0 },
                    { 10, 5, 4, null, new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "100% Cotton denim", false, "anh8sanpham.jpg", "hm1.jpg", "denim jacket", "hm, denim", 900000m, "Classic denim jacket with a modern twist.", "H&M Denim Jacket", null, 750000m, 60, "hm-denim-jacket", 1, true, null, null, true, 0 },
                    { 11, 1, 5, null, new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tech fleece fabric", true, "anh9sanpham.jpg", "nike3.jpg", "nike tech fleece", "nike, fleece", 2200000m, "Lightweight warmth for cold days.", "Nike Tech Fleece", null, 1900000m, 45, "nike-tech-fleece", 1, true, null, null, true, 0 },
                    { 12, 2, 4, null, new DateTime(2026, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "French terry cotton", false, "anh10sanpham.jpg", "adi3.jpg", "adidas originals", "adidas, hoodie", 1500000m, "Iconic style with cozy comfort.", "Adidas Originals Hoodie", null, 1200000m, 70, "adidas-hoodie", 1, true, null, null, true, 0 },
                    { 13, 3, 5, null, new DateTime(2026, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Slim straight cut", true, "anh11sanpham.jpg", "uni2.jpg", "selvedge denim", "uniqlo, jeans", 1200000m, "Authentic selvedge denim.", "Uniqlo Selvedge Jeans", null, 999000m, 90, "uniqlo-jeans", 1, true, null, null, true, 0 },
                    { 14, 4, 6, null, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lightweight fabric", false, "anh12sanpham.jpg", "zara2.jpg", "floral skirt", "zara, skirt", 800000m, "Beautiful floral pattern for spring.", "Zara Floral Skirt", null, 650000m, 55, "zara-floral-skirt", 1, true, null, null, true, 0 },
                    { 15, 5, 4, null, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Soft brushed inside", false, "anh13sanpham.jpg", "hm2.jpg", "oversized top", "hm, sweatshirt", 500000m, "Relaxed fit for everyday wear.", "H&M Oversized Sweatshirt", null, 400000m, 120, "hm-sweatshirt", 1, true, null, null, true, 0 },
                    { 16, 1, 3, null, new DateTime(2026, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Padded shoulder straps", false, "anh14sanpham.jpg", "nike4.jpg", "nike backpack", "nike, bag", 850000m, "Classic design with ample storage.", "Nike Heritage Backpack", null, 700000m, 40, "nike-backpack", 1, true, null, null, true, 0 },
                    { 17, 2, 8, null, new DateTime(2026, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Synthetic leather upper", true, "anh15sanpham.jpg", "adi4.jpg", "stan smith shoes", "adidas, sneakers", 2300000m, "Timeless tennis-inspired sneakers.", "Adidas Stan Smith", null, 1950000m, 50, "adidas-stan-smith", 1, true, null, null, true, 0 },
                    { 18, 3, 5, null, new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Heattech technology", false, "anh16sanpham.jpg", "uni3.jpg", "thermal wear", "uniqlo, leggings", 450000m, "Thermal leggings for winter warmth.", "Uniqlo Heattech Leggings", null, 350000m, 150, "uniqlo-heattech", 1, true, null, null, true, 0 },
                    { 19, 4, 3, null, new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Metallic buckle", false, "anh17sanpham.jpg", "zara3.jpg", "leather belt", "zara, belt", 600000m, "100% genuine leather belt.", "Zara Leather Belt", null, 450000m, 100, "zara-belt", 1, true, null, null, true, 0 },
                    { 20, 5, 4, null, new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cotton and elastane", false, "anh18sanpham.jpg", "hm3.jpg", "tank top", "hm, top", 250000m, "Simple and stylish ribbed top.", "H&M Ribbed Tank Top", null, 180000m, 180, "hm-tank-top", 1, true, null, null, true, 0 },
                    { 21, 1, 5, null, new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sweat-wicking fabric", true, "anh19sanpham.jpg", "nike5.jpg", "sport shorts", "nike, shorts", 750000m, "Stay dry during your workout.", "Nike Dri-FIT Shorts", null, 600000m, 85, "nike-shorts", 1, true, null, null, true, 0 },
                    { 22, 2, 3, null, new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adjustable strap", false, "anh20sanpham.jpg", "adi5.jpg", "baseball cap", "adidas, cap", 450000m, "Classic 6-panel cap.", "Adidas Baseball Cap", null, 350000m, 110, "adidas-cap", 1, true, null, null, true, 0 },
                    { 23, 3, 4, null, new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Linen cotton blend", true, "anh21sanpham.jpg", "uni4.jpg", "linen shirt", "uniqlo, shirt", 750000m, "Cool and breathable linen shirt.", "Uniqlo Linen Shirt", null, 599000m, 65, "uniqlo-linen-shirt", 1, true, null, null, true, 0 },
                    { 24, 4, 5, null, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flowy material", true, "anh22sanpham.jpg", "zara4.jpg", "wide leg", "zara, trousers", 1300000m, "High-waisted wide leg pants.", "Zara Wide Leg Trousers", null, 1100000m, 45, "zara-trousers", 1, true, null, null, true, 0 },
                    { 25, 5, 4, null, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Soft wool blend", true, "anh23sanpham.jpg", "hm4.jpg", "winter coat", "hm, coat", 2800000m, "Stay warm and stylish in winter.", "H&M Wool Blend Coat", null, 2400000m, 15, "hm-wool-coat", 1, true, null, null, true, 0 }
                });

            migrationBuilder.InsertData(
                table: "tb_OrderDetails",
                columns: new[] { "OrderDetailId", "OrderId", "Price", "ProductId", "Quantity", "ShopId" },
                values: new object[,]
                {
                    { 1, 1, 450000m, 1, 2, null },
                    { 2, 2, 550000m, 2, 1, null },
                    { 3, 3, 450000m, 1, 5, null },
                    { 4, 4, 550000m, 2, 3, null },
                    { 5, 5, 450000m, 1, 10, null },
                    { 6, 6, 550000m, 2, 2, null },
                    { 7, 7, 450000m, 1, 4, null }
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
                name: "IX_tb_Product_PromotionId",
                table: "tb_Product",
                column: "PromotionId");

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
                name: "tb_Promotion");

            migrationBuilder.DropTable(
                name: "tb_Shop");

            migrationBuilder.DropTable(
                name: "tb_Roles");

            migrationBuilder.DropTable(
                name: "tb_Users");
        }
    }
}
