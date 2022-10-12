﻿// <auto-generated />
using System;
using JoggingApp.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JoggingApp.EntityFramework.Migrations
{
    [DbContext(typeof(JoggingAppDbContext))]
    partial class JoggingAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("JoggingApp.Core.Jogs.Jog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Distance")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Date");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("Date"), new[] { "Id", "Distance", "Time", "UserId" });

                    b.HasIndex("UserId");

                    b.ToTable("Jog", (string)null);
                });

            modelBuilder.Entity("JoggingApp.Core.Jogs.JogLocation", b =>
                {
                    b.Property<Guid>("JogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("FeelsLikeTemperature")
                        .HasColumnType("real");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<float>("Temperature")
                        .HasColumnType("real");

                    b.HasKey("JogId");

                    b.ToTable("JogLocation", (string)null);
                });

            modelBuilder.Entity("JoggingApp.Core.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessage", (string)null);
                });

            modelBuilder.Entity("JoggingApp.Core.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("Email"), new[] { "Id", "Password", "State" });

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("JoggingApp.Core.Users.UserActivationToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserActivationToken", (string)null);
                });

            modelBuilder.Entity("JoggingApp.Core.Jogs.Jog", b =>
                {
                    b.HasOne("JoggingApp.Core.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("JoggingApp.Core.Jogs.JogLocation", b =>
                {
                    b.HasOne("JoggingApp.Core.Jogs.Jog", "Jog")
                        .WithOne("JogLocation")
                        .HasForeignKey("JoggingApp.Core.Jogs.JogLocation", "JogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Jog");
                });

            modelBuilder.Entity("JoggingApp.Core.Users.UserActivationToken", b =>
                {
                    b.HasOne("JoggingApp.Core.Users.User", "User")
                        .WithMany("ActivationTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("JoggingApp.Core.Jogs.Jog", b =>
                {
                    b.Navigation("JogLocation");
                });

            modelBuilder.Entity("JoggingApp.Core.Users.User", b =>
                {
                    b.Navigation("ActivationTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
