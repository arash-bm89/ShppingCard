﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShoppingCard.Repository;

#nullable disable

namespace ShoppingCard.Repository.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230125111852_addUserTable")]
    partial class addUserTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ShoppingCard.Domain.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("FinalPrice")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("SeqId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SeqId"));

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id");

                    b.HasIndex("SeqId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("ShoppingCard.Domain.Models.OrderProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<long>("SeqId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SeqId"));

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SeqId")
                        .IsUnique();

                    b.ToTable("OrderProduct");
                });

            modelBuilder.Entity("ShoppingCard.Domain.Models.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("ConfirmedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DeliveryAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<long>("SeqId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SeqId"));

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("SeqId")
                        .IsUnique();

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("ShoppingCard.Domain.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<long>("SeqId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SeqId"));

                    b.Property<long>("Stock")
                        .HasColumnType("bigint");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("\"IsDeleted\" = false");

                    b.HasIndex("SeqId")
                        .IsUnique();

                    b.ToTable("Product");
                });

            modelBuilder.Entity("ShoppingCard.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("SeqId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SeqId"));

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("SeqId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ShoppingCard.Domain.Models.Order", b =>
                {
                    b.HasOne("ShoppingCard.Domain.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ShoppingCard.Domain.Models.OrderProduct", b =>
                {
                    b.HasOne("ShoppingCard.Domain.Models.Order", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShoppingCard.Domain.Models.Product", "Product")
                        .WithMany("OrderProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ShoppingCard.Domain.Models.Payment", b =>
                {
                    b.HasOne("ShoppingCard.Domain.Models.Order", "Order")
                        .WithMany("Payments")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("ShoppingCard.Domain.Models.Order", b =>
                {
                    b.Navigation("Payments");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("ShoppingCard.Domain.Models.Product", b =>
                {
                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("ShoppingCard.Domain.Models.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
