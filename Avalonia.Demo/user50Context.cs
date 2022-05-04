using System;
using System.Collections.Generic;
using Avalonia.Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Avalonia.Demo
{
    public partial class user50Context : DbContext
    {
        public user50Context()
        {
        }

        public user50Context(DbContextOptions<user50Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<ClientService> ClientServices { get; set; } = null!;
        public virtual DbSet<DocumentByService> DocumentByServices { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductPhoto> ProductPhotos { get; set; } = null!;
        public virtual DbSet<ProductSale> ProductSales { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<ServicePhoto> ServicePhotos { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseLazyLoadingProxies().UseMySql("server=192.168.12.164;port=3306;database=user50;persist security info=False;uid=user50;pwd=26643", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.27-mariadb"));
                optionsBuilder.UseLazyLoadingProxies().UseMySql("server=localhost;port=3306;database=user50;persist security info=False;uid=root;pwd=12()HG34", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.27-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

                entity.HasIndex(e => e.GenderCode, "FK_Client_Gender");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.GenderCode)
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(50)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.PhotoPath)
                    .HasMaxLength(1000)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.RegistrationDate).HasMaxLength(6);

                entity.HasOne(d => d.GenderCodeNavigation)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.GenderCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Client_Gender");

                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Clients)
                    .UsingEntity<Dictionary<string, object>>(
                        "TagOfClient",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TagOfClient_Tag"),
                        r => r.HasOne<Client>().WithMany().HasForeignKey("ClientId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TagOfClient_Client"),
                        j =>
                        {
                            j.HasKey("ClientId", "TagId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("TagOfClient");

                            j.HasIndex(new[] { "TagId" }, "FK_TagOfClient_Tag");

                            j.IndexerProperty<int>("ClientId").HasColumnType("int(11)").HasColumnName("ClientID");

                            j.IndexerProperty<int>("TagId").HasColumnType("int(11)").HasColumnName("TagID");
                        });
            });

            modelBuilder.Entity<ClientService>(entity =>
            {
                entity.ToTable("ClientService");

                entity.HasIndex(e => e.ClientId, "FK_ClientService_Client");

                entity.HasIndex(e => e.ServiceId, "FK_ClientService_Service");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.ClientId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ClientID");

                entity.Property(e => e.Comment)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.ServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceID");

                entity.Property(e => e.StartTime).HasMaxLength(6);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientServices)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientService_Client");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ClientServices)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientService_Service");
            });

            modelBuilder.Entity<DocumentByService>(entity =>
            {
                entity.ToTable("DocumentByService");

                entity.HasIndex(e => e.ClientServiceId, "FK_DocumentByService_ClientService");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.ClientServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ClientServiceID");

                entity.Property(e => e.DocumentPath)
                    .HasMaxLength(1000)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.HasOne(d => d.ClientService)
                    .WithMany(p => p.DocumentByServices)
                    .HasForeignKey(d => d.ClientServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentByService_ClientService");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PRIMARY");

                entity.ToTable("Gender");

                entity.Property(e => e.Code)
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("Manufacturer");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.ManufacturerId, "FK_Product_Manufacturer");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Cost).HasPrecision(19, 4);

                entity.Property(e => e.Description)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.MainImagePath)
                    .HasMaxLength(1000)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.ManufacturerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ManufacturerID");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("FK_Product_Manufacturer");

                entity.HasMany(d => d.AttachedProducts)
                    .WithMany(p => p.MainProducts)
                    .UsingEntity<Dictionary<string, object>>(
                        "AttachedProduct",
                        l => l.HasOne<Product>().WithMany().HasForeignKey("AttachedProductId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AttachedProduct_Product1"),
                        r => r.HasOne<Product>().WithMany().HasForeignKey("MainProductId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AttachedProduct_Product"),
                        j =>
                        {
                            j.HasKey("MainProductId", "AttachedProductId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("AttachedProduct");

                            j.HasIndex(new[] { "AttachedProductId" }, "FK_AttachedProduct_Product1");

                            j.IndexerProperty<int>("MainProductId").HasColumnType("int(11)").HasColumnName("MainProductID");

                            j.IndexerProperty<int>("AttachedProductId").HasColumnType("int(11)").HasColumnName("AttachedProductID");
                        });

                entity.HasMany(d => d.MainProducts)
                    .WithMany(p => p.AttachedProducts)
                    .UsingEntity<Dictionary<string, object>>(
                        "AttachedProduct",
                        l => l.HasOne<Product>().WithMany().HasForeignKey("MainProductId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AttachedProduct_Product"),
                        r => r.HasOne<Product>().WithMany().HasForeignKey("AttachedProductId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AttachedProduct_Product1"),
                        j =>
                        {
                            j.HasKey("MainProductId", "AttachedProductId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("AttachedProduct");

                            j.HasIndex(new[] { "AttachedProductId" }, "FK_AttachedProduct_Product1");

                            j.IndexerProperty<int>("MainProductId").HasColumnType("int(11)").HasColumnName("MainProductID");

                            j.IndexerProperty<int>("AttachedProductId").HasColumnType("int(11)").HasColumnName("AttachedProductID");
                        });
            });

            modelBuilder.Entity<ProductPhoto>(entity =>
            {
                entity.ToTable("ProductPhoto");

                entity.HasIndex(e => e.ProductId, "FK_ProductPhoto_Product");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.PhotoPath)
                    .HasMaxLength(1000)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.ProductId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPhotos)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductPhoto_Product");
            });

            modelBuilder.Entity<ProductSale>(entity =>
            {
                entity.ToTable("ProductSale");

                entity.HasIndex(e => e.ClientServiceId, "FK_ProductSale_ClientService");

                entity.HasIndex(e => e.ProductId, "FK_ProductSale_Product");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.ClientServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ClientServiceID");

                entity.Property(e => e.ProductId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ProductID");

                entity.Property(e => e.Quantity).HasColumnType("int(11)");

                entity.Property(e => e.SaleDate).HasMaxLength(6);

                entity.HasOne(d => d.ClientService)
                    .WithMany(p => p.ProductSales)
                    .HasForeignKey(d => d.ClientServiceId)
                    .HasConstraintName("FK_ProductSale_ClientService");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductSales)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductSale_Product");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Cost).HasPrecision(19, 4);

                entity.Property(e => e.Description)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.DurationInSeconds).HasColumnType("int(11)");

                entity.Property(e => e.MainImagePath)
                    .HasMaxLength(1000)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");
            });

            modelBuilder.Entity<ServicePhoto>(entity =>
            {
                entity.ToTable("ServicePhoto");

                entity.HasIndex(e => e.ServiceId, "FK_ServicePhoto_Service");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.PhotoPath)
                    .HasMaxLength(1000)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.ServiceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ServiceID");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServicePhotos)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ServicePhoto_Service");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Color)
                    .HasMaxLength(6)
                    .IsFixedLength()
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.Title)
                    .HasMaxLength(30)
                    .UseCollation("utf8mb4_general_ci")
                    .HasCharSet("utf8mb4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}