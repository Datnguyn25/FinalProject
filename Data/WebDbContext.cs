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
        public DbSet<ProductReview> tb_ProductReview { get; set; }

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
                new ProductCategory { CateId = 1, CateName = "Men's Fashion", SeoTitle = "mens-fashion", Statuss = true, Sort = 1, ParentId = null, MetaKeywords = "men clothing, fashion", MetaDescription = "High quality clothing for men", CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateId = 2, CateName = "Women's Fashion", SeoTitle = "womens-fashion", Statuss = true, Sort = 2, ParentId = null, MetaKeywords = "dresses, women fashion", MetaDescription = "Latest fashion trends for women", CreatedDate = new DateTime(2026, 3, 23) },
                
                new ProductCategory { CateId = 3, CateName = "Accessories", SeoTitle = "accessories", Statuss = true, Sort = 3, ParentId = null, MetaKeywords = "bags, watches, jewelry", MetaDescription = "Fashion accessories for everyone", CreatedDate = new DateTime(2026, 3, 23) },
                
                new ProductCategory { CateId = 4, CateName = "Men's Torso", SeoTitle = "mens-torso", ParentId = 1, Statuss = true, Sort = 1, MetaKeywords = "t-shirts, polo", MetaDescription = "Stylish shirts for men", CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateId = 5, CateName = "Men's Leggings", SeoTitle = "mens-leggings", ParentId = 1, Statuss = true, Sort = 2, MetaKeywords = "jeans, trousers", MetaDescription = "Comfortable pants for men", CreatedDate = new DateTime(2026, 3, 23) },
                
                new ProductCategory { CateId = 6, CateName = "Dresses & Skirts", SeoTitle = "dresses-skirts", ParentId = 2, Statuss = true, Sort = 1, MetaKeywords = "maxi dress, skirts", MetaDescription = "Beautiful dresses for ladies", CreatedDate = new DateTime(2026, 3, 23) },
                new ProductCategory { CateId = 7, CateName = "Women's Handbags", SeoTitle = "womens-handbags", ParentId = 2, Statuss = true, Sort = 2, MetaKeywords = "purses, totes", MetaDescription = "Luxury handbags for women", CreatedDate = new DateTime(2026, 3, 23) },
                
                new ProductCategory { CateId = 8, CateName = "Footwear", SeoTitle = "footwear", ParentId = 3, Statuss = true, Sort = 1, MetaKeywords = "shoes, sneakers", MetaDescription = "Quality footwear for all ages", CreatedDate = new DateTime(2026, 3, 23) },
                
                new ProductCategory { CateId = 9, CateName = "Watches & Jewelry", SeoTitle = "watches-jewelry", ParentId = 3, Statuss = true, Sort = 2, MetaKeywords = "gold, luxury watches", MetaDescription = "Premium timepieces and jewelry", CreatedDate = new DateTime(2026, 3, 23) }
            );

            modelBuilder.Entity<Brand>().HasData(
                new Brand { BrandId = 1, BrandName = "Nike" },
                new Brand { BrandId = 2, BrandName = "Adidas" },
                new Brand { BrandId = 3, BrandName = "Uniqlo" },
                new Brand { BrandId = 4, BrandName = "Zara" },
                new Brand { BrandId = 5, BrandName = "H&M" }
            );

            // 2. Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "Premium White Oxford Shirt", SeoTitle = "premium-white-oxford-shirt", Status = true, Image = "white-shirt.jpg", ListImages = "white-shirt.jpg", Price = 450000m, VAT = true, Quantity = 100, Hot = true, ProductDescription = "Classic fit white oxford shirt", Detail = "100% Cotton", ViewCount = 0, MetaKeywords = "shirt", MetaDescription = "shirt", CateId = 4, BrandId = 3, ShopId = 1, CreatedDate = new DateTime(2026, 3, 23) },
                new Product { ProductId = 2, ProductName = "Floral Summer Maxi Dress", SeoTitle = "floral-summer-maxi-dress", Status = true, Image = "floral-dress.jpg", ListImages = "floral-dress.jpg", Price = 550000m, VAT = true, Quantity = 50, Hot = false, ProductDescription = "Elegant floral print dress", Detail = "Chiffon material", ViewCount = 0, MetaKeywords = "dress", MetaDescription = "dress", CateId = 6, BrandId = 4, ShopId = 1, CreatedDate = new DateTime(2026, 3, 23) },
                new Product { ProductId = 3, ProductName = "Slim Fit Navy Chinos", SeoTitle = "slim-fit-navy-chinos", Status = true, Image = "anh1sanpham.jpg", ListImages = "anh1sanpham.jpg", Price = 600000m, VAT = true, Quantity = 80, Hot = true, ProductDescription = "High-quality khaki pants", Detail = "Stretchy fabric", ViewCount = 0, MetaKeywords = "chinos", MetaDescription = "pants", CateId = 5, BrandId = 2, ShopId = 1, CreatedDate = new DateTime(2026, 3, 25) },
                new Product { ProductId = 4, ProductName = "Leather Crossbody Bag", SeoTitle = "leather-crossbody-bag", Status = true, Image = "anh2sanpham.jpg", ListImages = "anh2sanpham.jpg", Price = 1200000m, VAT = true, Quantity = 30, Hot = false, ProductDescription = "Genuine leather bag", Detail = "Handmade", ViewCount = 0, MetaKeywords = "bag", MetaDescription = "bag", CateId = 7, BrandId = 5, ShopId = 1, CreatedDate = new DateTime(2026, 3, 26) },
                new Product { ProductId  = 5, ProductName = "Classic Gold Watch", SeoTitle = "classic-gold-watch", Status = true, Image = "anh3sanpham.jpg", ListImages = "anh3sanpham.jpg", Price = 3500000m, VAT = true, Quantity = 15, Hot = true, ProductDescription = "Elegant gold-plated watch", Detail = "Waterproof 50m", ViewCount = 0, MetaKeywords = "watch", MetaDescription = "watch", CateId = 9, BrandId = 1, ShopId = 1, CreatedDate = new DateTime(2026, 3, 27) },
                new Product { ProductId = 6, ProductName = "Nike Air Max 270", SeoTitle = "nike-air-max-270", Status = true, Image = "anh4sanpham.jpg", ListImages = "anh4sanpham.jpg", Price = 3500000m, VAT = true, Quantity = 40, Hot = true, ProductDescription = "Advanced cushioning for daily comfort.", Detail = "Breathable mesh upper", ViewCount = 0, MetaKeywords = "nike, shoes", MetaDescription = "nike air max", CateId = 8, BrandId = 1, ShopId = 1, CreatedDate = new DateTime(2026, 4, 1) },
                new Product { ProductId = 7, ProductName = "Adidas Ultraboost 22", SeoTitle = "adidas-ultraboost-22", Status = true, Image = "anh5sanpham.jpg", ListImages = "anh5sanpham.jpg", Price = 4200000m, VAT = true, Quantity = 35, Hot = true, ProductDescription = "Ultimate energy return for runners.", Detail = "Primeknit upper", ViewCount = 0, MetaKeywords = "adidas, running", MetaDescription = "adidas ultraboost", CateId = 8, BrandId = 2, ShopId = 1, CreatedDate = new DateTime(2026, 4, 1) },
                new Product { ProductId = 8, ProductName = "Uniqlo Airism T-Shirt", SeoTitle = "uniqlo-airism-tshirt", Status = true, Image = "anh6sanpham.jpg", ListImages = "nh6sanpham.jpg", Price = 350000m, VAT = true, Quantity = 200, Hot = false, ProductDescription = "Smooth and quick-drying fabric.", Detail = "Airism technology", ViewCount = 0, MetaKeywords = "uniqlo, t-shirt", MetaDescription = "airism shirt", CateId = 4, BrandId = 3, ShopId = 1, CreatedDate = new DateTime(2026, 4, 2) },
                new Product { ProductId = 9, ProductName = "Zara Slim Fit Suit", SeoTitle = "zara-slim-suit", Status = true, Image = "anh7sanpham.jpg", ListImages = "anh7sanpham.jpg", Price = 2500000m, VAT = true, Quantity = 20, Hot = true, ProductDescription = "Modern slim fit for formal events.", Detail = "Premium wool blend", ViewCount = 0, MetaKeywords = "zara, suit", MetaDescription = "slim fit suit", CateId = 4, BrandId = 4, ShopId = 1, CreatedDate = new DateTime(2026, 4, 2) },
                new Product { ProductId = 10, ProductName = "H&M Denim Jacket", SeoTitle = "hm-denim-jacket", Status = true, Image = "anh8sanpham.jpg", ListImages = "anh8sanpham.jpg", Price = 900000m, VAT = true, Quantity = 60, Hot = false, ProductDescription = "Classic denim jacket with a modern twist.", Detail = "100% Cotton denim", ViewCount = 0, MetaKeywords = "hm, denim", MetaDescription = "denim jacket", CateId = 4, BrandId = 5, ShopId = 1, CreatedDate = new DateTime(2026, 4, 3) },
                new Product { ProductId = 11, ProductName = "Nike Tech Fleece", SeoTitle = "nike-tech-fleece", Status = true, Image = "anh9sanpham.jpg", ListImages = "anh9sanpham.jpg", Price = 2200000m, VAT = true, Quantity = 45, Hot = true, ProductDescription = "Lightweight warmth for cold days.", Detail = "Tech fleece fabric", ViewCount = 0, MetaKeywords = "nike, fleece", MetaDescription = "nike tech fleece", CateId = 5, BrandId = 1, ShopId = 1, CreatedDate = new DateTime(2026, 4, 3) },
                new Product { ProductId = 12, ProductName = "Adidas Originals Hoodie", SeoTitle = "adidas-hoodie", Status = true, Image = "anh10sanpham.jpg", ListImages = "anh10sanpham.jpg", Price = 1500000m, VAT = true, Quantity = 70, Hot = false, ProductDescription = "Iconic style with cozy comfort.", Detail = "French terry cotton", ViewCount = 0, MetaKeywords = "adidas, hoodie", MetaDescription = "adidas originals", CateId = 4, BrandId = 2, ShopId = 1, CreatedDate = new DateTime(2026, 4, 4) },
                new Product { ProductId = 13, ProductName = "Uniqlo Selvedge Jeans", SeoTitle = "uniqlo-jeans", Status = true, Image = "anh11sanpham.jpg", ListImages = "anh11sanpham.jpg", Price = 1200000m, VAT = true, Quantity = 90, Hot = true, ProductDescription = "Authentic selvedge denim.", Detail = "Slim straight cut", ViewCount = 0, MetaKeywords = "uniqlo, jeans", MetaDescription = "selvedge denim", CateId = 5, BrandId = 3, ShopId = 1, CreatedDate = new DateTime(2026, 4, 4) },
                new Product { ProductId = 14, ProductName = "Zara Floral Skirt", SeoTitle = "zara-floral-skirt", Status = true, Image = "anh12sanpham.jpg", ListImages = "anh12sanpham.jpg", Price = 800000m, VAT = true, Quantity = 55, Hot = false, ProductDescription = "Beautiful floral pattern for spring.", Detail = "Lightweight fabric", ViewCount = 0, MetaKeywords = "zara, skirt", MetaDescription = "floral skirt", CateId = 6, BrandId = 4, ShopId    = 1, CreatedDate = new DateTime(2026, 4, 5) },
                new Product { ProductId = 15, ProductName = "H&M Oversized Sweatshirt", SeoTitle = "hm-sweatshirt", Status = true, Image = "anh13sanpham.jpg", ListImages = "anh13sanpham.jpg", Price = 500000m, VAT = true, Quantity = 120, Hot = false, ProductDescription = "Relaxed fit for everyday wear.", Detail = "Soft brushed inside", ViewCount = 0, MetaKeywords = "hm, sweatshirt", MetaDescription = "oversized top", CateId = 4, BrandId = 5, ShopId = 1, CreatedDate = new DateTime(2026, 4, 5) },
                new Product { ProductId = 16, ProductName = "Nike Heritage Backpack", SeoTitle = "nike-backpack", Status = true, Image = "anh14sanpham.jpg", ListImages = "anh14sanpham.jpg", Price = 850000m, VAT = true, Quantity = 40, Hot = false, ProductDescription = "Classic design with ample storage.", Detail = "Padded shoulder straps", ViewCount = 0, MetaKeywords = "nike, bag", MetaDescription = "nike backpack", CateId = 3, BrandId = 1, ShopId = 1, CreatedDate = new DateTime(2026, 4, 6) },
                new Product { ProductId = 17, ProductName = "Adidas Stan Smith", SeoTitle = "adidas-stan-smith", Status = true, Image = "anh15sanpham.jpg", ListImages = "anh15sanpham.jpg", Price = 2300000m, VAT = true, Quantity = 50, Hot = true, ProductDescription = "Timeless tennis-inspired sneakers.", Detail = "Synthetic leather upper", ViewCount = 0, MetaKeywords = "adidas, sneakers", MetaDescription = "stan smith shoes", CateId = 8, BrandId = 2, ShopId = 1, CreatedDate = new DateTime(2026, 4, 6) },
                new Product { ProductId = 18, ProductName = "Uniqlo Heattech Leggings", SeoTitle = "uniqlo-heattech", Status = true, Image = "anh16sanpham.jpg", ListImages = "anh16sanpham.jpg", Price = 450000m, VAT = true, Quantity = 150, Hot = false, ProductDescription = "Thermal leggings for winter warmth.", Detail = "Heattech technology", ViewCount = 0, MetaKeywords = "uniqlo, leggings", MetaDescription = "thermal wear", CateId = 5, BrandId = 3, ShopId = 1, CreatedDate = new DateTime(2026, 4, 7) },
                new Product { ProductId = 19, ProductName = "Zara Leather Belt", SeoTitle = "zara-belt", Status = true, Image = "anh17sanpham.jpg", ListImages = "anh17sanpham.jpg", Price = 600000m, VAT = true, Quantity = 100, Hot = false, ProductDescription = "100% genuine leather belt.", Detail = "Metallic buckle", ViewCount = 0, MetaKeywords = "zara, belt", MetaDescription = "leather belt", CateId = 3, BrandId = 4, ShopId = 1, CreatedDate = new DateTime(2026, 4, 7) },
                new Product { ProductId = 20, ProductName = "H&M Ribbed Tank Top", SeoTitle = "hm-tank-top", Status = true, Image = "anh18sanpham.jpg", ListImages = "anh18sanpham.jpg", Price = 250000m, VAT = true, Quantity = 180, Hot = false, ProductDescription = "Simple and stylish ribbed top.", Detail = "Cotton and elastane", ViewCount = 0, MetaKeywords = "hm, top", MetaDescription = "tank top", CateId = 4, BrandId = 5, ShopId = 1, CreatedDate = new DateTime(2026, 4, 8) },
                new Product { ProductId = 21, ProductName = "Nike Dri-FIT Shorts", SeoTitle = "nike-shorts", Status = true, Image = "anh19sanpham.jpg", ListImages = "anh19sanpham.jpg", Price = 750000m, VAT = true, Quantity = 85, Hot = true, ProductDescription = "Stay dry during your workout.", Detail = "Sweat-wicking fabric", ViewCount = 0, MetaKeywords = "nike, shorts", MetaDescription = "sport shorts", CateId = 5, BrandId = 1, ShopId = 1, CreatedDate = new DateTime(2026, 4, 8) },
                new Product { ProductId = 22, ProductName = "Adidas Baseball Cap", SeoTitle = "adidas-cap", Status = true, Image = "anh20sanpham.jpg", ListImages = "anh20sanpham.jpg", Price = 450000m, VAT = true, Quantity = 110, Hot = false, ProductDescription = "Classic 6-panel cap.", Detail = "Adjustable strap", ViewCount = 0, MetaKeywords = "adidas, cap", MetaDescription = "baseball cap", CateId = 3, BrandId = 2, ShopId = 1, CreatedDate = new DateTime(2026, 4, 9) },
                new Product { ProductId = 23, ProductName = "Uniqlo Linen Shirt", SeoTitle = "uniqlo-linen-shirt", Status = true, Image = "anh21sanpham.jpg", ListImages = "anh21sanpham.jpg", Price = 750000m, VAT = true, Quantity = 65, Hot = true, ProductDescription = "Cool and breathable linen shirt.", Detail = "Linen cotton blend", ViewCount = 0, MetaKeywords = "uniqlo, shirt", MetaDescription = "linen shirt", CateId = 4, BrandId = 3, ShopId = 1, CreatedDate = new DateTime(2026, 4, 9) },
                new Product { ProductId = 24, ProductName = "Zara Wide Leg Trousers", SeoTitle = "zara-trousers", Status = true, Image = "anh22sanpham.jpg", ListImages = "anh22sanpham.jpg", Price = 1300000m, VAT = true, Quantity = 45, Hot = true, ProductDescription = "High-waisted wide leg pants.", Detail = "Flowy material", ViewCount = 0, MetaKeywords = "zara, trousers", MetaDescription = "wide leg", CateId = 5, BrandId = 4, ShopId = 1, CreatedDate = new DateTime(2026, 4, 10) },
                new Product { ProductId = 25, ProductName = "H&M Wool Blend Coat", SeoTitle = "hm-wool-coat", Status = true, Image = "anh23sanpham.jpg", ListImages = "anh23sanpham.jpg", Price = 2800000m, VAT = true, Quantity = 15, Hot = true, ProductDescription = "Stay warm and stylish in winter.", Detail = "Soft wool blend", ViewCount = 0, MetaKeywords = "hm, coat", MetaDescription = "winter coat", CateId = 4, BrandId = 5, ShopId = 1, CreatedDate = new DateTime(2026, 4, 10) }
            );

            // 5. Seed Orders (Dữ liệu doanh thu cho Dashboard)
            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, UserId = 3, OrderDate = new DateTime(2026, 4, 10), TotalPrice = 1250000, ShippingAddress = "Hanoi, Vietnam", ReceiverPhone = "0912345678", OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 10) },
                new Order { OrderId = 2, UserId = 4, OrderDate = new DateTime(2026, 4, 11), TotalPrice = 850000, ShippingAddress = "HCMC, Vietnam", ReceiverPhone = "0988776655", OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 11) },
                new Order { OrderId = 3, UserId = 5, OrderDate = new DateTime(2026, 4, 12), TotalPrice = 2100000, ShippingAddress = "London, UK", ReceiverPhone = "0901234567", OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 12) },
                new Order { OrderId = 4, UserId = 6, OrderDate = new DateTime(2026, 4, 13), TotalPrice = 1500000, ShippingAddress = "Da Nang, Vietnam", ReceiverPhone = "0944332211", OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 13) },
                new Order { OrderId = 5, UserId = 7, OrderDate = new DateTime(2026, 4, 14), TotalPrice = 3200000, ShippingAddress = "Seattle, USA", ReceiverPhone = "0355667788", OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 14) },
                new Order { OrderId = 6, UserId = 8, OrderDate = new DateTime(2026, 4, 15), TotalPrice = 950000, ShippingAddress = "Can Tho, Vietnam", ReceiverPhone = "0322114455", OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 15) },
                new Order { OrderId = 7, UserId = 9, OrderDate = new DateTime(2026, 4, 16), TotalPrice = 1800000, ShippingAddress = "Hue, Vietnam", ReceiverPhone = "0977889900", OrderStatus = "Completed", PaymentStatus = "Paid", Delivered = true, DeliveryDate = new DateTime(2026, 4, 16) }
            );

            // 6. Seed OrderDetails (Dữ liệu cho Top Selling Products)
            modelBuilder.Entity<OrderDetails>().HasData(
                // Đơn 1 mua 2 áo Oxford
                new OrderDetails { OrderDetailId = 1, OrderId = 1, ProductId = 1, Quantity = 2, Size = "M", Price = 450000 },
                // Đơn 2 mua 1 váy Floral
                new OrderDetails { OrderDetailId = 2, OrderId = 2, ProductId = 2, Quantity = 1, Size = "L", Price = 550000 },
                // Các đơn khác mua dồn vào để tạo bảng xếp hạng
                new OrderDetails { OrderDetailId = 3, OrderId = 3, ProductId = 1, Quantity = 5, Size = "M", Price = 450000 },
                new OrderDetails { OrderDetailId = 4, OrderId = 4, ProductId = 2, Quantity = 3, Size = "L", Price = 550000 },
                new OrderDetails { OrderDetailId = 5, OrderId = 5, ProductId = 1, Quantity = 10, Size = "M", Price = 450000 },
                new OrderDetails { OrderDetailId = 6, OrderId = 6, ProductId = 2, Quantity = 2, Size = "L", Price = 550000 },
                new OrderDetails { OrderDetailId = 7, OrderId = 7, ProductId = 1, Quantity = 4, Size = "M", Price = 450000 }
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

            modelBuilder.Entity<User>(entity => entity.ToTable("tb_Users"));
            modelBuilder.Entity<IdentityRole<int>>(entity => entity.ToTable("tb_Roles"));
            modelBuilder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable("tb_UserClaims"));
            modelBuilder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable("tb_UserLogins"));
            modelBuilder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable("tb_RoleClaims"));
            modelBuilder.Entity<IdentityUserToken<int>>(entity => entity.ToTable("tb_UserTokens"));

            // 3. Seed User Admin
            var adminUserId = 1;
            var sellerUserId = 2;
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
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@123");

            var sellerUser = new User
            {
                Id = sellerUserId,
                UserName = "seller@urbanchic.com",
                NormalizedUserName = "SELLER@URBANCHIC.COM",
                Email = "seller@urbanchic.com",
                NormalizedEmail = "SELLER@URBANCHIC.COM",
                EmailConfirmed = true,
                FullName = "Nguyen Thanh Dat",
                IsActive = true,
                IsBanned = false,
                CreatedAt = new DateTime(2026, 3, 27),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            sellerUser.PasswordHash = hasher.HashPassword(sellerUser, "Seller@123");

            modelBuilder.Entity<User>().HasData(adminUser, sellerUser);

            var customers = new List<User> {
                new User { Id = 3, UserName = "leminh@gmail.com", NormalizedUserName = "LEMINH@GMAIL.COM", Email = "leminh@gmail.com", NormalizedEmail = "LEMINH@GMAIL.COM", EmailConfirmed = true, FullName = "Lê Minh", Gender = "Male", City = "Hanoi", Country = "Vietnam", CreatedAt = new DateTime(2026, 4, 1), SecurityStamp = Guid.NewGuid().ToString() },
                new User { Id = 4, UserName = "hoangyen@gmail.com", NormalizedUserName = "HOANGYEN@GMAIL.COM", Email = "hoangyen@gmail.com", NormalizedEmail = "HOANGYEN@GMAIL.COM", EmailConfirmed = true, FullName = "Hoàng Yến", Gender = "Female", City = "HCMC", Country = "Vietnam", CreatedAt = new DateTime(2026, 4, 2), SecurityStamp = Guid.NewGuid().ToString() },
                new User { Id = 5, UserName = "alex.smith@gmail.com", NormalizedUserName = "ALEX.SMITH@GMAIL.COM", Email = "alex.smith@gmail.com", NormalizedEmail = "ALEX.SMITH@GMAIL.COM", EmailConfirmed = true, FullName = "Alex Smith", Gender = "Other", City = "London", Country = "UK", CreatedAt = new DateTime(2026, 4, 3), SecurityStamp = Guid.NewGuid().ToString() },
                new User { Id = 6, UserName = "tranbao@gmail.com", NormalizedUserName = "TRANBAO@GMAIL.COM", Email = "tranbao@gmail.com", NormalizedEmail = "TRANBAO@GMAIL.COM", EmailConfirmed = true, FullName = "Trần Bảo", Gender = "Male", City = "Da Nang", Country = "Vietnam", CreatedAt = new DateTime(2026, 4, 5), SecurityStamp = Guid.NewGuid().ToString() },
                new User { Id = 7, UserName = "katie.ng@gmail.com", NormalizedUserName = "KATIE.NG@GMAIL.COM", Email = "katie.ng@gmail.com", NormalizedEmail = "KATIE.NG@GMAIL.COM", EmailConfirmed = true, FullName = "Katie Nguyen", Gender = "Female", City = "Seattle", Country = "USA", CreatedAt = new DateTime(2026, 4, 7), SecurityStamp = Guid.NewGuid().ToString() },
                new User { Id = 8, UserName = "phamduy@gmail.com", NormalizedUserName = "PHAMDUY@GMAIL.COM", Email = "phamduy@gmail.com", NormalizedEmail = "PHAMDUY@GMAIL.COM", EmailConfirmed = true, FullName = "Phạm Duy", Gender = "Male", City = "Can Tho", Country = "Vietnam", CreatedAt = new DateTime(2026, 4, 10), SecurityStamp = Guid.NewGuid().ToString() },
                new User { Id = 9, UserName = "lananh@gmail.com", NormalizedUserName = "LANANH@GMAIL.COM", Email = "lananh@gmail.com", NormalizedEmail = "LANANH@GMAIL.COM", EmailConfirmed = true, FullName = "Nguyễn Lan Anh", Gender = "Female", City = "Hue", Country = "Vietnam", CreatedAt = new DateTime(2026, 4, 12), SecurityStamp = Guid.NewGuid().ToString() }
            };

            foreach (var u in customers) u.PasswordHash = hasher.HashPassword(u, "Customer@123");
            modelBuilder.Entity<User>().HasData(customers);


            modelBuilder.Entity<Shop>().HasData(
                new Shop
                {
                    ShopId = 1,
                    
                    ShopName = "Urban Chic Fashion",
                    OwnerId = sellerUserId, // <--- Đổi từ 1 sang 2
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
                    
                }
            );

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { RoleId = 1, UserId = adminUserId }, // Admin gán Role Admin
                new IdentityUserRole<int> { RoleId = 3, UserId = sellerUserId }, // Seller gán Role Shop
                new IdentityUserRole<int> { RoleId = 2, UserId = 3 }, new IdentityUserRole<int> { RoleId = 2, UserId = 4 },
                new IdentityUserRole<int> { RoleId = 2, UserId = 5 }, new IdentityUserRole<int> { RoleId = 2, UserId = 6 },
                new IdentityUserRole<int> { RoleId = 2, UserId = 7 }, new IdentityUserRole<int> { RoleId = 2, UserId = 8 },
                new IdentityUserRole<int> { RoleId = 2, UserId = 9 }
            );
        }
    }

}
