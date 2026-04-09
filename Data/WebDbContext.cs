using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Data
{
    public class WebDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public WebDbContext(DbContextOptions<WebDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> tb_Users { get; set; }
        public DbSet<Shop> tb_Shop { get; set; }
        public DbSet<Product> tb_Product { get; set; }
        public DbSet<ProductCategory> tb_ProductCategory { get; set; }
        public DbSet<Carts> tb_Carts { get; set; }
        public DbSet<CartItems> tb_CartItems { get; set; }
        public DbSet<Order> tb_Order { get; set; }
        public DbSet<OrderDetails> tb_OrderDetails { get; set; }
        public DbSet<Brand> tb_Brand { get; set; }
        public DbSet<Promotion> tb_Promotion { get; set; }
        public DbSet<SystemSetting> tb_SystemSetting { get; set; }
        public DbSet<ShopReview> tb_ShopReview { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int>
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole<int>
                {
                    Id = 2,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole<int>
                {
                    Id = 3,
                    Name = "Shop",
                    NormalizedName = "SHOP",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                // Cấp 1
                new ProductCategory { CateID = 1, CateName = "Men's Fashion", SeoTitle = "mens-fashion", Statuss = true, CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateID = 2, CateName = "Women's Fashion", SeoTitle = "womens-fashion", Statuss = true, CreatedDate = new DateTime(2026, 3, 23) },

                // ĐỔI TỪ 4 THÀNH 3 ĐỂ PHÙ HỢP VỚI PARENTID CỦA CON NÓ
                new ProductCategory { CateID = 3, CateName = "Accessories", SeoTitle = "accessories", Statuss = true, Sort = 3, CreatedDate = new DateTime(2026, 3, 23) },

                // Cấp 2 - Con của Men's Fashion (ParentID = 1)
                new ProductCategory { CateID = 4, CateName = "Men's Shirts", ParentID = 1, Statuss = true, CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateID = 5, CateName = "Men's Pants", ParentID = 1, Statuss = true, CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateID = 6, CateName = "Men's Jackets", ParentID = 1, Statuss = true, CreatedDate = new DateTime(2026, 3, 23) },

                // Cấp 2 - Con của Women's Fashion (ParentID = 2)
                new ProductCategory { CateID = 7, CateName = "Dresses & Skirts", ParentID = 2, Statuss = true, CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateID = 8, CateName = "Women's Tops", ParentID = 2, Statuss = true, CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateID = 9, CateName = "Women's Handbags", ParentID = 2, Statuss = true, CreatedDate = new DateTime(2026, 3, 23) },

                // Cấp 2 - Con của Accessories (ParentID = 3)
                new ProductCategory { CateID = 10, CateName = "Footwear", ParentID = 3, Statuss = true, Sort = 1, CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateID = 11, CateName = "Watches & Jewelry", ParentID = 3, Statuss = true, Sort = 2, CreatedDate = new DateTime(2026, 3, 23) }
            );

            modelBuilder.Entity<Brand>().HasData(
                new Brand { BrandID = 1, BrandName = "Nike" },
                new Brand { BrandID = 2, BrandName = "Adidas" },
                new Brand { BrandID = 3, BrandName = "Uniqlo" },
                new Brand { BrandID = 4, BrandName = "Zara" },
                new Brand { BrandID = 5, BrandName = "H&M" }
            );

            // 2. Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductID = 1,
                    ProductName = "Premium White Oxford Shirt",
                    SeoTitle = "premium-white-oxford-shirt",
                    Status = true,
                    Image = "white-shirt.jpg",
                    ListImages = "img1.jpg,img2.jpg",
                    Price = 45.000m,
                    PromotionPrice = 39.99m,
                    VAT = true,
                    Quantity = 100,
                    Hot = true,
                    ProductDescription = "Classic fit white oxford shirt made from 100% cotton.",
                    Detail = "<p>Breathable fabric, perfect for office and formal events.</p>",
                    ViewCount = 0,
                    MetaKeywords = "white shirt, oxford shirt, formal",
                    MetaDescription = "Buy premium white oxford shirt at the best price.",
                    CateID = 1,
                    BrandID = 1,
                    ShopID = 1,
                    CreatedDate = new DateTime(2026, 3, 23)
                },
                new Product
                {
                    ProductID = 2,
                    ProductName = "Floral Summer Maxi Dress",
                    SeoTitle = "floral-summer-maxi-dress",
                    Status = true,
                    Image = "floral-dress.jpg",
                    ListImages = "img3.jpg,img4.jpg",
                    Price = 55.000m,
                    PromotionPrice = 49.00m,
                    VAT = true,
                    Quantity = 50,
                    Hot = false,
                    ProductDescription = "Elegant floral print dress for summer outings.",
                    Detail = "<p>Soft chiffon material with adjustable waist strap.</p>",
                    ViewCount = 0,
                    MetaKeywords = "summer dress, floral dress, maxi dress",
                    MetaDescription = "Beautiful floral dress for your summer vacation.",
                    CateID = 2,
                    BrandID = 1,
                    ShopID = 1,
                    CreatedDate = new DateTime(2026, 3, 23)
                }
            );

            modelBuilder.Entity<CartItems>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            // 1. Đổi tên các bảng Identity sang tên bạn muốn
            modelBuilder.Entity<User>(entity => {
                    entity.ToTable("tb_Users");
            });

            modelBuilder.Entity<IdentityRole<int>>(entity => {
                entity.ToTable("tb_Roles"); // Đây là nơi bạn đặt tên bảng Role của mình
            });

            // 2. Sửa lỗi Multiple Cascade Paths (Lỗi bạn vừa gặp)
            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("tb_UserRoles");

                // Tắt Cascade Delete để tránh vòng lặp xóa trong SQL Server
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<IdentityRole<int>>()
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.NoAction);

            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.ToTable("tb_Shop"); // Đảm bảo map đúng tên bảng của bạn

                entity.HasOne(s => s.Role)
                      .WithMany()
                      .HasForeignKey(s => s.RoleId)
                      .OnDelete(DeleteBehavior.NoAction); // Tắt Cascade để tránh lỗi Multiple Cascade Paths
            });

            modelBuilder.Entity<Shop>().HasData(
                new Shop
                {
                    ShopID = 1,
                    ShopName = "Urban Chic Fashion",
                    LogoUrl = "logo-urban.png",
                    CoverImageUrl = "cover-urban.jpg",
                    ShopDescription = "Premium streetwear and modern fashion trends for the young generation.",
                    ContactEmail = "support@urbanchic.com",
                    ContactPhone = "+84987654321",
                    ShopAddress = "123 ABC Street, District 1",
                    City = "Ho Chi Minh City",
                    Country = "Vietnam",
                    TotalProducts = 50,
                    TotalFollowers = 1200,
                    RatingAverage = 4.80m,
                    TotalReviews = 450,
                    TotalSold = 2500,
                    IsVerified = true,
                    IsActive = true,
                    IsBanned = false,
                    CreatedAt = new DateTime(2026, 3, 27),
                    RoleId = 2
                }
            );

            modelBuilder.Entity<User>(entity => entity.ToTable("tb_Users"));
            modelBuilder.Entity<IdentityRole<int>>(entity => entity.ToTable("tb_Roles"));
            modelBuilder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable("tb_UserClaims"));
            modelBuilder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable("tb_UserLogins"));
            modelBuilder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable("tb_RoleClaims"));
            modelBuilder.Entity<IdentityUserToken<int>>(entity => entity.ToTable("tb_UserTokens"));
        }
    }

}
