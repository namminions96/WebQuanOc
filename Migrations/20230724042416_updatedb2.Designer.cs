﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web_Report.Data;

#nullable disable

namespace CheckApiWeb.Migrations
{
    [DbContext(typeof(UseDbcontext))]
    [Migration("20230724042416_updatedb2")]
    partial class updatedb2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CheckApiWeb.Models.CartSave", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Giaban")
                        .HasColumnType("double");

                    b.Property<int?>("Masp")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Soluong")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Tensp")
                        .HasColumnType("longtext");

                    b.Property<double?>("TongTien")
                        .HasColumnType("double");

                    b.Property<string>("userid")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("cartSaves");
                });

            modelBuilder.Entity("CheckApiWeb.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Dvt")
                        .HasColumnType("longtext");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<string>("Tensp")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<string>("images")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("CheckApiWeb.Models.cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Giaban")
                        .HasColumnType("double");

                    b.Property<int?>("Masp")
                        .HasColumnType("int");

                    b.Property<int>("Soluong")
                        .HasColumnType("int");

                    b.Property<string>("Tensp")
                        .HasColumnType("longtext");

                    b.Property<double?>("TongTien")
                        .HasColumnType("double");

                    b.Property<string>("images")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Carts");
                });
#pragma warning restore 612, 618
        }
    }
}
