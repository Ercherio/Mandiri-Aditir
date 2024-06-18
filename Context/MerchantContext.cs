using System;
using System.Collections.Generic;
using MerchantService.Models;
using Microsoft.EntityFrameworkCore;

namespace MerchantService.Context;

public partial class MerchantContext : DbContext
{
    public MerchantContext()
    {
    }

    public MerchantContext(DbContextOptions<MerchantContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuthAssignment> AuthAssignments { get; set; }

    public virtual DbSet<AuthItem> AuthItems { get; set; }

    public virtual DbSet<AuthItemChild> AuthItemChildren { get; set; }

    public virtual DbSet<CashierMerchant> CashierMerchants { get; set; }

    public virtual DbSet<Logging> Loggings { get; set; }

    public virtual DbSet<Merchant> Merchants { get; set; }

    public virtual DbSet<OwnerMerchant> OwnerMerchants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:database_merchant");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Auth_Ass__3214EC07029EC377");

            entity.ToTable("Auth_Assignment");

            entity.HasIndex(e => e.Id, "IX_Auth_Assignment");

            entity.Property(e => e.CreatedAt).HasColumnName("Created_at");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .HasColumnName("Item_name");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.ItemNameNavigation).WithMany(p => p.AuthAssignments)
                .HasForeignKey(d => d.ItemName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auth_Assignment_Auth_Item");

            entity.HasOne(d => d.User).WithMany(p => p.AuthAssignments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auth_Assignment_Users");
        });

        modelBuilder.Entity<AuthItem>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__Auth_Ite__3214EC07A0DD7661");

            entity.ToTable("Auth_Item");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnName("Created_at");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("Update_at");
        });

        modelBuilder.Entity<AuthItemChild>(entity =>
        {
            entity.ToTable("Auth_Item_Child");

            entity.Property(e => e.Child).HasMaxLength(255);
            entity.Property(e => e.Parent).HasMaxLength(255);

            entity.HasOne(d => d.ChildNavigation).WithMany(p => p.AuthItemChildChildNavigations)
                .HasForeignKey(d => d.Child)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auth_Item_Child_Auth_Item_child");

            entity.HasOne(d => d.ParentNavigation).WithMany(p => p.AuthItemChildParentNavigations)
                .HasForeignKey(d => d.Parent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auth_Item_Child_Auth_Item_parent");
        });

        modelBuilder.Entity<CashierMerchant>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__CashierM__206A9DF8CE9B62C8");

            entity.ToTable("CashierMerchant");

            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.Mid).HasMaxLength(255);

            entity.HasOne(d => d.MidNavigation).WithMany(p => p.CashierMerchants)
                .HasForeignKey(d => d.Mid)
                .HasConstraintName("FK_CashierMerchant_Merchant");
        });

        modelBuilder.Entity<Logging>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logging__3214EC07120AE80F");

            entity.ToTable("Logging");

            entity.Property(e => e.Action).HasMaxLength(255);
            entity.Property(e => e.Datetime).HasColumnType("datetime");
            entity.Property(e => e.ElapsedTime).HasColumnName("Elapsed_time");
            entity.Property(e => e.Error).HasMaxLength(255);
            entity.Property(e => e.ErrorDescription)
                .HasMaxLength(255)
                .HasColumnName("Error_description");
            entity.Property(e => e.FinishedTime).HasColumnName("Finished_time");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(100)
                .HasColumnName("Ip_address");
            entity.Property(e => e.RequestHeader).HasColumnName("Request_header");
            entity.Property(e => e.ResponseHeader).HasColumnName("Response_header");
            entity.Property(e => e.StartTime).HasColumnName("Start_time");
            entity.Property(e => e.Url).HasMaxLength(255);
        });

        modelBuilder.Entity<Merchant>(entity =>
        {
            entity.HasKey(e => e.Mid).HasName("PK__Merchant__C79638C23CE1A2FB");

            entity.ToTable("Merchant");

            entity.Property(e => e.Mid).HasMaxLength(255);
            entity.Property(e => e.Nama).HasMaxLength(255);
        });

        modelBuilder.Entity<OwnerMerchant>(entity =>
        {
            entity.HasKey(e => e.Mid).HasName("PK__OwnerMer__C79638C27C329C93");

            entity.ToTable("OwnerMerchant");

            entity.Property(e => e.Mid).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.OwnerMerchants)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_OwnerMerchant_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC072899CB78");

            entity.Property(e => e.AttemptLoginFailed).HasColumnName("Attempt_login_failed");
            entity.Property(e => e.CreatedAt).HasColumnName("Created_at");
            entity.Property(e => e.CreatedDate).HasColumnName("Created_date");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstLoginAt).HasColumnName("First_login_at");
            entity.Property(e => e.Fullname).HasMaxLength(255);
            entity.Property(e => e.LastLoginAt).HasColumnName("Last_login_at");
            entity.Property(e => e.LastLoginFailed).HasColumnName("Last_login_failed");
            entity.Property(e => e.MobilePhone)
                .HasMaxLength(15)
                .HasColumnName("Mobile_phone");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Salt).HasMaxLength(50);
            entity.Property(e => e.PasswordChange).HasColumnName("Password_change");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
