﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Models;

public partial class redisContext : DbContext
{
    public redisContext(DbContextOptions<redisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Course { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<TokenTable> TokenTable { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.IdCourse);

            entity.HasIndex(e => e.UserId, "IX_Course_UserId");

            entity.Property(e => e.IdCourse).HasMaxLength(100);
            entity.Property(e => e.CourseName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.Property(e => e.RoleId).HasMaxLength(100);
            entity.Property(e => e.IdUser).HasColumnName("Id_User");
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Roles)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Roles_User");
        });

        modelBuilder.Entity<TokenTable>(entity =>
        {
            entity.HasKey(e => e.IdToken);

            entity.Property(e => e.IdToken).ValueGeneratedNever();
            entity.Property(e => e.AccessToken).IsUnicode(false);
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("Create_At");
            entity.Property(e => e.RefreshToken).IsUnicode(false);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TokenTable)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TokenTable_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValueSql("(N'')");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}