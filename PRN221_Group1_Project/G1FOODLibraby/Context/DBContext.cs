using System;
using System.Collections.Generic;
using G1FOODLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Context;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountStatus> AccountStatuses { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Categogy> Categogies { get; set; }

    public virtual DbSet<CategogyWarehouseItem> CategogyWarehouseItems { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommentStatus> CommentStatuses { get; set; }

    public virtual DbSet<Destruction> Destructions { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductStatus> ProductStatuses { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleDetail> ScheduleDetails { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<VoucherStatus> VoucherStatuses { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<WarehouseExport> WarehouseExports { get; set; }

    public virtual DbSet<WarehouseImport> WarehouseImports { get; set; }

    public virtual DbSet<WarehouseItem> WarehouseItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true).Build();
        var strConn = config["ConnectionString:G1FoodDB"];
        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3213E83F8F69CAA1");

            entity.ToTable("Account");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("email");
            entity.Property(e => e.EncryptedPassword)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("encryptedPassword");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.StatusId).HasColumnName("statusId");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Account__roleId__5070F446");

            entity.HasOne(d => d.Status).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__Account__statusI__5165187F");
        });

        modelBuilder.Entity<AccountStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account___3213E83FB946C030");

            entity.ToTable("Account_Status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'no name')")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3213E83FCADB2561");

            entity.ToTable("Cart");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Account).WithMany(p => p.Carts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Cart__accountId__70DDC3D8");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Cart__productId__6FE99F9F");
        });

        modelBuilder.Entity<Categogy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categogy__3213E83F5822C0CE");

            entity.ToTable("Categogy");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'no name')")
                .HasColumnName("name");
        });

        modelBuilder.Entity<CategogyWarehouseItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categogy__3213E83F9D68BD88");

            entity.ToTable("Categogy_Warehouse_Item");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'no name')")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3213E83F87444081");

            entity.ToTable("Comment");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.ParentCommentId).HasColumnName("parentCommentId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.StatusId).HasColumnName("statusId");

            entity.HasOne(d => d.Account).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Comment__account__1EA48E88");

            entity.HasOne(d => d.Product).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Comment__product__1F98B2C1");

            entity.HasOne(d => d.Status).WithMany(p => p.Comments)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__Comment__statusI__1DB06A4F");
        });

        modelBuilder.Entity<CommentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment___3213E83FA13CE0B5");

            entity.ToTable("Comment_Status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'no name')")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Destruction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Destruct__3213E83F3ACAD3DD");

            entity.ToTable("Destruction");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");
            entity.Property(e => e.WarehouseExportId).HasColumnName("warehouseExportId");

            entity.HasOne(d => d.Account).WithMany(p => p.Destructions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Destructi__accou__18EBB532");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Destructions)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Destructi__sched__19DFD96B");

            entity.HasOne(d => d.WarehouseExport).WithMany(p => p.Destructions)
                .HasForeignKey(d => d.WarehouseExportId)
                .HasConstraintName("FK__Destructi__wareh__1AD3FDA4");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3213E83FB8D71540");

            entity.ToTable("Menu");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");

            entity.HasOne(d => d.Product).WithMany(p => p.Menus)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Menu__productId__160F4887");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Menus)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Menu__scheduleId__151B244E");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3213E83FA7BF373E");

            entity.ToTable("Order");

            entity.ToTable("Order", tb => tb.HasTrigger("MyTable_Insert"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");
            entity.Property(e => e.StatusId).HasColumnName("statusId");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.VoucherId).HasColumnName("voucherId");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Order__scheduleI__75A278F5");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__Order__statusId__73BA3083");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Order__userId__74AE54BC");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK__Order__voucherId__76969D2E");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3213E83FA2479753");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SalePercent).HasColumnName("salePercent");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__7A672E12");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__produ__797309D9");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order_St__3213E83FC39CF784");

            entity.ToTable("Order_Status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'no name')")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3213E83F87CC5A22");

            entity.ToTable("Product");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CategogyId).HasColumnName("categogyId");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'no name')")
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.SalePercent).HasColumnName("salePercent");
            entity.Property(e => e.StatusId).HasColumnName("statusId");

            entity.HasOne(d => d.Categogy).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategogyId)
                .HasConstraintName("FK__Product__categog__619B8048");

            entity.HasOne(d => d.Status).WithMany(p => p.Products)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__Product__statusI__60A75C0F");
        });

        modelBuilder.Entity<ProductStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product___3213E83F8119D44D");

            entity.ToTable("Product_Status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'no name')")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipe__3213E83FA6439E76");

            entity.ToTable("Recipe");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.WarehouseItemId).HasColumnName("warehouseItemId");

            entity.HasOne(d => d.Product).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Recipe__productI__06CD04F7");

            entity.HasOne(d => d.WarehouseItem).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.WarehouseItemId)
                .HasConstraintName("FK__Recipe__warehous__07C12930");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83F4BA6A8E6");

            entity.ToTable("Role");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'no name')")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3213E83F49C9CBD5");

            entity.ToTable("Schedule");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
        });

        modelBuilder.Entity<ScheduleDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3213E83F985F391A");

            entity.ToTable("Schedule_Detail");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");

            entity.HasOne(d => d.Account).WithMany(p => p.ScheduleDetails)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Schedule___accou__6C190EBB");

            entity.HasOne(d => d.Schedule).WithMany(p => p.ScheduleDetails)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Schedule___sched__6D0D32F4");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Unit__3213E83F00B5CF19");

            entity.ToTable("Unit");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83FDC9B372B");

            entity.ToTable("User");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.AddressDetail)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("addressDetail");
            entity.Property(e => e.DefaultUser).HasColumnName("defaultUser");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'no name')")
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("phone");
            entity.Property(e => e.StatusId).HasColumnName("statusId");

            entity.HasOne(d => d.Account).WithMany(p => p.Users)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__User__accountId__571DF1D5");

            entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__User__statusId__5812160E");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Voucher__3213E83F1BCF4699");

            entity.ToTable("Voucher");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SalePercent).HasColumnName("salePercent");
            entity.Property(e => e.StatusId).HasColumnName("statusId");

            entity.HasOne(d => d.Status).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__Voucher__statusI__6477ECF3");
        });

        modelBuilder.Entity<VoucherStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Voucher___3213E83F97B0D90F");

            entity.ToTable("Voucher_Status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'no name')")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warehous__3213E83FBE0EF01C");

            entity.ToTable("Warehouse");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.WarehouseItemId).HasColumnName("warehouseItemId");

            entity.HasOne(d => d.WarehouseItem).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.WarehouseItemId)
                .HasConstraintName("FK__Warehouse__wareh__0A9D95DB");
        });

        modelBuilder.Entity<WarehouseExport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warehous__3213E83FC782DFF6");

            entity.ToTable("Warehouse_Export");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouseId");

            entity.HasOne(d => d.Schedule).WithMany(p => p.WarehouseExports)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Warehouse__sched__114A936A");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.WarehouseExports)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__Warehouse__wareh__123EB7A3");
        });

        modelBuilder.Entity<WarehouseImport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warehous__3213E83F2913DFD7");

            entity.ToTable("Warehouse_Import");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouseId");

            entity.HasOne(d => d.Account).WithMany(p => p.WarehouseImports)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Warehouse__accou__0E6E26BF");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.WarehouseImports)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__Warehouse__wareh__0D7A0286");
        });

        modelBuilder.Entity<WarehouseItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warehous__3213E83F75E94033");

            entity.ToTable("Warehouse_Item");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UnitId).HasColumnName("unitId");

            entity.HasOne(d => d.CategogyItem).WithMany(p => p.WarehouseItems)
                .HasForeignKey(d => d.CategogyItemId)
                .HasConstraintName("FK__Warehouse__Categ__02FC7413");

            entity.HasOne(d => d.Unit).WithMany(p => p.WarehouseItems)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK__Warehouse__unitI__03F0984C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
