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
        public DbSet<SystemLog> tb_SystemLog { get; set; }

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
                new ProductCategory { CateID = 1, CateName = "Men's Fashion", SeoTitle = "mens-fashion", Statuss = true, Sort = 1, ParentID = null, MetaKeywords = "men clothing, fashion", MetaDescription = "High quality clothing for men", CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateID = 2, CateName = "Women's Fashion", SeoTitle = "womens-fashion", Statuss = true, Sort = 2, ParentID = null, MetaKeywords = "dresses, women fashion", MetaDescription = "Latest fashion trends for women", CreatedDate = new DateTime(2026, 3, 23) },
                
                new ProductCategory { CateID = 3, CateName = "Accessories", SeoTitle = "accessories", Statuss = true, Sort = 3, ParentID = null, MetaKeywords = "bags, watches, jewelry", MetaDescription = "Fashion accessories for everyone", CreatedDate = new DateTime(2026, 3, 23) },
                
                new ProductCategory { CateID = 4, CateName = "Men's Torso", SeoTitle = "mens-torso", ParentID = 1, Statuss = true, Sort = 1, MetaKeywords = "t-shirts, polo", MetaDescription = "Stylish shirts for men", CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateID = 5, CateName = "Men's Leggings", SeoTitle = "mens-leggings", ParentID = 1, Statuss = true, Sort = 2, MetaKeywords = "jeans, trousers", MetaDescription = "Comfortable pants for men", CreatedDate = new DateTime(2026, 3, 23) },
                
                new ProductCategory { CateID = 6, CateName = "Dresses & Skirts", SeoTitle = "dresses-skirts", ParentID = 2, Statuss = true, Sort = 1, MetaKeywords = "maxi dress, skirts", MetaDescription = "Beautiful dresses for ladies", CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateID = 7, CateName = "Women's Handbags", SeoTitle = "womens-handbags", ParentID = 2, Statuss = true, Sort = 2, MetaKeywords = "purses, totes", MetaDescription = "Luxury handbags for women", CreatedDate = new DateTime(2026, 3, 23) },
                
                new ProductCategory { CateID = 8, CateName = "Footwear", SeoTitle = "footwear", ParentID = 3, Statuss = true, Sort = 1, MetaKeywords = "shoes, sneakers", MetaDescription = "Quality footwear for all ages", CreatedDate = new DateTime(2026, 3, 23) },
                
                new ProductCategory { CateID = 9, CateName = "Watches & Jewelry", SeoTitle = "watches-jewelry", ParentID = 3, Statuss = true, Sort = 2, MetaKeywords = "gold, luxury watches", MetaDescription = "Premium timepieces and jewelry", CreatedDate = new DateTime(2026, 3, 23) }
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
                new Product { ProductID = 1, ProductName = "Premium White Oxford Shirt", SeoTitle = "premium-white-oxford-shirt", Status = true, Image = "white-shirt.jpg", ListImages = "img1.jpg,img2.jpg", Price = 450000m, PromotionPrice = 399000m, VAT = true, Quantity = 100, Hot = true, ProductDescription = "Classic fit white oxford shirt", Detail = "100% Cotton", ViewCount = 0, MetaKeywords = "shirt", MetaDescription = "shirt", CateID = 4, BrandID = 3, ShopID = 1, CreatedDate = new DateTime(2026, 3, 23) },
                new Product { ProductID = 2, ProductName = "Floral Summer Maxi Dress", SeoTitle = "floral-summer-maxi-dress", Status = true, Image = "floral-dress.jpg", ListImages = "img3.jpg,img4.jpg", Price = 550000m, PromotionPrice = 490000m, VAT = true, Quantity = 50, Hot = false, ProductDescription = "Elegant floral print dress", Detail = "Chiffon material", ViewCount = 0, MetaKeywords = "dress", MetaDescription = "dress", CateID = 6, BrandID = 4, ShopID = 1, CreatedDate = new DateTime(2026, 3, 23) },
                new Product { ProductID = 3, ProductName = "Slim Fit Navy Chinos", SeoTitle = "slim-fit-navy-chinos", Status = true, Image = "anh1sanpham.jpg", ListImages = "img5.jpg", Price = 600000m, PromotionPrice = 550000m, VAT = true, Quantity = 80, Hot = true, ProductDescription = "High-quality khaki pants", Detail = "Stretchy fabric", ViewCount = 0, MetaKeywords = "chinos", MetaDescription = "pants", CateID = 5, BrandID = 2, ShopID = 1, CreatedDate = new DateTime(2026, 3, 25) },
                new Product { ProductID = 4, ProductName = "Leather Crossbody Bag", SeoTitle = "leather-crossbody-bag", Status = true, Image = "anh2sanpham.jpg", ListImages = "img6.jpg", Price = 1200000m, PromotionPrice = 990000m, VAT = true, Quantity = 30, Hot = false, ProductDescription = "Genuine leather bag", Detail = "Handmade", ViewCount = 0, MetaKeywords = "bag", MetaDescription = "bag", CateID = 7, BrandID = 5, ShopID = 1, CreatedDate = new DateTime(2026, 3, 26) },
                new Product { ProductID = 5, ProductName = "Classic Gold Watch", SeoTitle = "classic-gold-watch", Status = true, Image = "anh3sanpham.jpg", ListImages = "img7.jpg", Price = 3500000m, PromotionPrice = 3200000m, VAT = true, Quantity = 15, Hot = true, ProductDescription = "Elegant gold-plated watch", Detail = "Waterproof 50m", ViewCount = 0, MetaKeywords = "watch", MetaDescription = "watch", CateID = 9, BrandID = 1, ShopID = 1, CreatedDate = new DateTime(2026, 3, 27) },
                new Product { ProductID = 6, ProductName = "Nike Air Max 270", SeoTitle = "nike-air-max-270", Status = true, Image = "anh4sanpham.jpg", ListImages = "nike1.jpg,nike2.jpg", Price = 3500000m, PromotionPrice = 3200000m, VAT = true, Quantity = 40, Hot = true, ProductDescription = "Advanced cushioning for daily comfort.", Detail = "Breathable mesh upper", ViewCount = 0, MetaKeywords = "nike, shoes", MetaDescription = "nike air max", CateID = 8, BrandID = 1, ShopID = 1, CreatedDate = new DateTime(2026, 4, 1) },
                new Product { ProductID = 7, ProductName = "Adidas Ultraboost 22", SeoTitle = "adidas-ultraboost-22", Status = true, Image = "anh5sanpham.jpg", ListImages = "adi1.jpg,adi2.jpg", Price = 4200000m, PromotionPrice = 3800000m, VAT = true, Quantity = 35, Hot = true, ProductDescription = "Ultimate energy return for runners.", Detail = "Primeknit upper", ViewCount = 0, MetaKeywords = "adidas, running", MetaDescription = "adidas ultraboost", CateID = 8, BrandID = 2, ShopID = 1, CreatedDate = new DateTime(2026, 4, 1) },
                new Product { ProductID = 8, ProductName = "Uniqlo Airism T-Shirt", SeoTitle = "uniqlo-airism-tshirt", Status = true, Image = "anh6sanpham.jpg", ListImages = "uni1.jpg", Price = 350000m, PromotionPrice = 299000m, VAT = true, Quantity = 200, Hot = false, ProductDescription = "Smooth and quick-drying fabric.", Detail = "Airism technology", ViewCount = 0, MetaKeywords = "uniqlo, t-shirt", MetaDescription = "airism shirt", CateID = 4, BrandID = 3, ShopID = 1, CreatedDate = new DateTime(2026, 4, 2) },
                new Product { ProductID = 9, ProductName = "Zara Slim Fit Suit", SeoTitle = "zara-slim-suit", Status = true, Image = "anh7sanpham.jpg", ListImages = "zara1.jpg", Price = 2500000m, PromotionPrice = 2100000m, VAT = true, Quantity = 20, Hot = true, ProductDescription = "Modern slim fit for formal events.", Detail = "Premium wool blend", ViewCount = 0, MetaKeywords = "zara, suit", MetaDescription = "slim fit suit", CateID = 4, BrandID = 4, ShopID = 1, CreatedDate = new DateTime(2026, 4, 2) },
                new Product { ProductID = 10, ProductName = "H&M Denim Jacket", SeoTitle = "hm-denim-jacket", Status = true, Image = "anh8sanpham.jpg", ListImages = "hm1.jpg", Price = 900000m, PromotionPrice = 750000m, VAT = true, Quantity = 60, Hot = false, ProductDescription = "Classic denim jacket with a modern twist.", Detail = "100% Cotton denim", ViewCount = 0, MetaKeywords = "hm, denim", MetaDescription = "denim jacket", CateID = 4, BrandID = 5, ShopID = 1, CreatedDate = new DateTime(2026, 4, 3) },
                new Product { ProductID = 11, ProductName = "Nike Tech Fleece", SeoTitle = "nike-tech-fleece", Status = true, Image = "anh9sanpham.jpg", ListImages = "nike3.jpg", Price = 2200000m, PromotionPrice = 1900000m, VAT = true, Quantity = 45, Hot = true, ProductDescription = "Lightweight warmth for cold days.", Detail = "Tech fleece fabric", ViewCount = 0, MetaKeywords = "nike, fleece", MetaDescription = "nike tech fleece", CateID = 5, BrandID = 1, ShopID = 1, CreatedDate = new DateTime(2026, 4, 3) },
                new Product { ProductID = 12, ProductName = "Adidas Originals Hoodie", SeoTitle = "adidas-hoodie", Status = true, Image = "anh10sanpham.jpg", ListImages = "adi3.jpg", Price = 1500000m, PromotionPrice = 1200000m, VAT = true, Quantity = 70, Hot = false, ProductDescription = "Iconic style with cozy comfort.", Detail = "French terry cotton", ViewCount = 0, MetaKeywords = "adidas, hoodie", MetaDescription = "adidas originals", CateID = 4, BrandID = 2, ShopID = 1, CreatedDate = new DateTime(2026, 4, 4) },
                new Product { ProductID = 13, ProductName = "Uniqlo Selvedge Jeans", SeoTitle = "uniqlo-jeans", Status = true, Image = "anh11sanpham.jpg", ListImages = "uni2.jpg", Price = 1200000m, PromotionPrice = 999000m, VAT = true, Quantity = 90, Hot = true, ProductDescription = "Authentic selvedge denim.", Detail = "Slim straight cut", ViewCount = 0, MetaKeywords = "uniqlo, jeans", MetaDescription = "selvedge denim", CateID = 5, BrandID = 3, ShopID = 1, CreatedDate = new DateTime(2026, 4, 4) },
                new Product { ProductID = 14, ProductName = "Zara Floral Skirt", SeoTitle = "zara-floral-skirt", Status = true, Image = "anh12sanpham.jpg", ListImages = "zara2.jpg", Price = 800000m, PromotionPrice = 650000m, VAT = true, Quantity = 55, Hot = false, ProductDescription = "Beautiful floral pattern for spring.", Detail = "Lightweight fabric", ViewCount = 0, MetaKeywords = "zara, skirt", MetaDescription = "floral skirt", CateID = 6, BrandID = 4, ShopID = 1, CreatedDate = new DateTime(2026, 4, 5) },
                new Product { ProductID = 15, ProductName = "H&M Oversized Sweatshirt", SeoTitle = "hm-sweatshirt", Status = true, Image = "anh13sanpham.jpg", ListImages = "hm2.jpg", Price = 500000m, PromotionPrice = 400000m, VAT = true, Quantity = 120, Hot = false, ProductDescription = "Relaxed fit for everyday wear.", Detail = "Soft brushed inside", ViewCount = 0, MetaKeywords = "hm, sweatshirt", MetaDescription = "oversized top", CateID = 4, BrandID = 5, ShopID = 1, CreatedDate = new DateTime(2026, 4, 5) },
                new Product { ProductID = 16, ProductName = "Nike Heritage Backpack", SeoTitle = "nike-backpack", Status = true, Image = "anh14sanpham.jpg", ListImages = "nike4.jpg", Price = 850000m, PromotionPrice = 700000m, VAT = true, Quantity = 40, Hot = false, ProductDescription = "Classic design with ample storage.", Detail = "Padded shoulder straps", ViewCount = 0, MetaKeywords = "nike, bag", MetaDescription = "nike backpack", CateID = 3, BrandID = 1, ShopID = 1, CreatedDate = new DateTime(2026, 4, 6) },
                new Product { ProductID = 17, ProductName = "Adidas Stan Smith", SeoTitle = "adidas-stan-smith", Status = true, Image = "anh15sanpham.jpg", ListImages = "adi4.jpg", Price = 2300000m, PromotionPrice = 1950000m, VAT = true, Quantity = 50, Hot = true, ProductDescription = "Timeless tennis-inspired sneakers.", Detail = "Synthetic leather upper", ViewCount = 0, MetaKeywords = "adidas, sneakers", MetaDescription = "stan smith shoes", CateID = 8, BrandID = 2, ShopID = 1, CreatedDate = new DateTime(2026, 4, 6) },
                new Product { ProductID = 18, ProductName = "Uniqlo Heattech Leggings", SeoTitle = "uniqlo-heattech", Status = true, Image = "anh16sanpham.jpg", ListImages = "uni3.jpg", Price = 450000m, PromotionPrice = 350000m, VAT = true, Quantity = 150, Hot = false, ProductDescription = "Thermal leggings for winter warmth.", Detail = "Heattech technology", ViewCount = 0, MetaKeywords = "uniqlo, leggings", MetaDescription = "thermal wear", CateID = 5, BrandID = 3, ShopID = 1, CreatedDate = new DateTime(2026, 4, 7) },
                new Product { ProductID = 19, ProductName = "Zara Leather Belt", SeoTitle = "zara-belt", Status = true, Image = "anh17sanpham.jpg", ListImages = "zara3.jpg", Price = 600000m, PromotionPrice = 450000m, VAT = true, Quantity = 100, Hot = false, ProductDescription = "100% genuine leather belt.", Detail = "Metallic buckle", ViewCount = 0, MetaKeywords = "zara, belt", MetaDescription = "leather belt", CateID = 3, BrandID = 4, ShopID = 1, CreatedDate = new DateTime(2026, 4, 7) },
                new Product { ProductID = 20, ProductName = "H&M Ribbed Tank Top", SeoTitle = "hm-tank-top", Status = true, Image = "anh18sanpham.jpg", ListImages = "hm3.jpg", Price = 250000m, PromotionPrice = 180000m, VAT = true, Quantity = 180, Hot = false, ProductDescription = "Simple and stylish ribbed top.", Detail = "Cotton and elastane", ViewCount = 0, MetaKeywords = "hm, top", MetaDescription = "tank top", CateID = 4, BrandID = 5, ShopID = 1, CreatedDate = new DateTime(2026, 4, 8) },
                new Product { ProductID = 21, ProductName = "Nike Dri-FIT Shorts", SeoTitle = "nike-shorts", Status = true, Image = "anh19sanpham.jpg", ListImages = "nike5.jpg", Price = 750000m, PromotionPrice = 600000m, VAT = true, Quantity = 85, Hot = true, ProductDescription = "Stay dry during your workout.", Detail = "Sweat-wicking fabric", ViewCount = 0, MetaKeywords = "nike, shorts", MetaDescription = "sport shorts", CateID = 5, BrandID = 1, ShopID = 1, CreatedDate = new DateTime(2026, 4, 8) },
                new Product { ProductID = 22, ProductName = "Adidas Baseball Cap", SeoTitle = "adidas-cap", Status = true, Image = "anh20sanpham.jpg", ListImages = "adi5.jpg", Price = 450000m, PromotionPrice = 350000m, VAT = true, Quantity = 110, Hot = false, ProductDescription = "Classic 6-panel cap.", Detail = "Adjustable strap", ViewCount = 0, MetaKeywords = "adidas, cap", MetaDescription = "baseball cap", CateID = 3, BrandID = 2, ShopID = 1, CreatedDate = new DateTime(2026, 4, 9) },
                new Product { ProductID = 23, ProductName = "Uniqlo Linen Shirt", SeoTitle = "uniqlo-linen-shirt", Status = true, Image = "anh21sanpham.jpg", ListImages = "uni4.jpg", Price = 750000m, PromotionPrice = 599000m, VAT = true, Quantity = 65, Hot = true, ProductDescription = "Cool and breathable linen shirt.", Detail = "Linen cotton blend", ViewCount = 0, MetaKeywords = "uniqlo, shirt", MetaDescription = "linen shirt", CateID = 4, BrandID = 3, ShopID = 1, CreatedDate = new DateTime(2026, 4, 9) },
                new Product { ProductID = 24, ProductName = "Zara Wide Leg Trousers", SeoTitle = "zara-trousers", Status = true, Image = "anh22sanpham.jpg", ListImages = "zara4.jpg", Price = 1300000m, PromotionPrice = 1100000m, VAT = true, Quantity = 45, Hot = true, ProductDescription = "High-waisted wide leg pants.", Detail = "Flowy material", ViewCount = 0, MetaKeywords = "zara, trousers", MetaDescription = "wide leg", CateID = 5, BrandID = 4, ShopID = 1, CreatedDate = new DateTime(2026, 4, 10) },
                new Product { ProductID = 25, ProductName = "H&M Wool Blend Coat", SeoTitle = "hm-wool-coat", Status = true, Image = "anh23sanpham.jpg", ListImages = "hm4.jpg", Price = 2800000m, PromotionPrice = 2400000m, VAT = true, Quantity = 15, Hot = true, ProductDescription = "Stay warm and stylish in winter.", Detail = "Soft wool blend", ViewCount = 0, MetaKeywords = "hm, coat", MetaDescription = "winter coat", CateID = 4, BrandID = 5, ShopID = 1, CreatedDate = new DateTime(2026, 4, 10) }



            );

            // 5. Seed Orders (Dữ liệu doanh thu cho Dashboard)
            modelBuilder.Entity<Order>().HasData(
                new Order { OrderID = 1, CustomerID = 1, OrderDate = new DateTime(2026, 4, 10), TotalPrice = 1250000, OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 10) },
                new Order { OrderID = 2, CustomerID = 1, OrderDate = new DateTime(2026, 4, 11), TotalPrice = 850000, OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 11) },
                new Order { OrderID = 3, CustomerID = 1, OrderDate = new DateTime(2026, 4, 12), TotalPrice = 2100000, OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 12) },
                new Order { OrderID = 4, CustomerID = 1, OrderDate = new DateTime(2026, 4, 13), TotalPrice = 1500000, OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 13) },
                new Order { OrderID = 5, CustomerID = 1, OrderDate = new DateTime(2026, 4, 14), TotalPrice = 3200000, OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 14) },
                new Order { OrderID = 6, CustomerID = 1, OrderDate = new DateTime(2026, 4, 15), TotalPrice = 950000, OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 15) },
                new Order { OrderID = 7, CustomerID = 1, OrderDate = new DateTime(2026, 4, 16), TotalPrice = 1800000, OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 16) }
            );

            // 6. Seed OrderDetails (Dữ liệu cho Top Selling Products)
            modelBuilder.Entity<OrderDetails>().HasData(
                // Đơn 1 mua 2 áo Oxford
                new OrderDetails { OrderDetailId = 1, OrderId = 1, ProductId = 1, Quantity = 2, Price = 450000 },
                // Đơn 2 mua 1 váy Floral
                new OrderDetails { OrderDetailId = 2, OrderId = 2, ProductId = 2, Quantity = 1, Price = 550000 },
                // Các đơn khác mua dồn vào để tạo bảng xếp hạng
                new OrderDetails { OrderDetailId = 3, OrderId = 3, ProductId = 1, Quantity = 5, Price = 450000 },
                new OrderDetails { OrderDetailId = 4, OrderId = 4, ProductId = 2, Quantity = 3, Price = 550000 },
                new OrderDetails { OrderDetailId = 5, OrderId = 5, ProductId = 1, Quantity = 10, Price = 450000 },
                new OrderDetails { OrderDetailId = 6, OrderId = 6, ProductId = 2, Quantity = 2, Price = 550000 },
                new OrderDetails { OrderDetailId = 7, OrderId = 7, ProductId = 1, Quantity = 4, Price = 450000 }
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

            // 3. Seed User Admin
            var adminUserId = 1;
            var hasher = new PasswordHasher<User>();

            var adminUser = new User
            {
                Id = adminUserId,
                UserName = "admin@fashionstore.com",
                NormalizedUserName = "ADMIN@FASHIONSTORE.COM",
                Email = "admin@fashionstore.com",
                NormalizedEmail = "ADMIN@FASHIONSTORE.COM",
                EmailConfirmed = true,
                FullName = "System Administrator",
                IsActive = true,
                IsBanned = false,
                CreatedAt = new DateTime(2026, 3, 23),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            // Mật khẩu là: Admin@123
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@123");

            modelBuilder.Entity<User>().HasData(adminUser);

            // 4. Gán Role Admin (Id = 1) cho User Admin (Id = 1) thông qua bảng trung gian
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    RoleId = 1, // Khớp với Id của Role Admin bạn đã seed ở trên
                    UserId = adminUserId
                }
            );
        }
    }

}
