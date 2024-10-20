using System;
using System.Collections.Generic;
using LearnApiDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnApiDemo.Data;

public partial class LearnApiDbContext : DbContext
{
    public LearnApiDbContext()
    {
    }

    public LearnApiDbContext(DbContextOptions<LearnApiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__tbl_Cust__A25C5AA6A1A7AF1D");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__tbl_User__F3DBC573CDB969CB");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
