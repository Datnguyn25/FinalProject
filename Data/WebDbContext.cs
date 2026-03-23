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
        public DbSet<Roles> tb_Roles { get; set; }
        public DbSet<Shop> tb_Shop { get; set; }
        public DbSet<Product> tb_Product { get; set; }
        public DbSet<ProductCategory> tb_ProductCategory { get; set; }
        public DbSet<Carts> tb_Carts { get; set; }
        public DbSet<CartItems> tb_CartItems { get; set; }
        public DbSet<Order> tb_Order { get; set; }
        public DbSet<OrderDetails> tb_OrderDetails { get; set; }
        public DbSet<Brand> tb_Brand { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Roles>().HasData(
                new Roles { RoleId = 1, RoleName = "Admin" },
                new Roles { RoleId = 2, RoleName = "User" },
                new Roles { RoleId = 3, RoleName = "Shop" }
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory
                {
                    CateID = 1,
                    CateName = "Men's Fashion",
                    SeoTitle = "mens-fashion",
                    Statuss = true,
                    MetaKeywords = "men clothing, fashion, shirts",
                    MetaDescription = "High quality clothing for men",
                    CreatedDate = new DateTime(2026, 3, 23)
                },
                new ProductCategory
                {
                    CateID = 2,
                    CateName = "Women's Fashion",
                    SeoTitle = "womens-fashion",
                    Statuss = true,
                    MetaKeywords = "dresses, women clothing, skirts",
                    MetaDescription = "Latest fashion trends for women",
                    CreatedDate = new DateTime(2026, 3, 23)
                }
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
                    Price = 45.00m,
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
                    Price = 55.00m,
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
        }
    }

}
