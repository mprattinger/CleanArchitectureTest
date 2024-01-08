﻿// <auto-generated />
using System;
using CleanArchitectureTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CleanArchitectureTest.Data.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240108185742_todoapp2")]
    partial class todoapp2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("CleanArchitectureTest.Data.DTOs.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("CleanArchitectureTest.Data.DTOs.Todo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Todos");
                });

            modelBuilder.Entity("CleanArchitectureTest.Data.DTOs.TodoAppointee", b =>
                {
                    b.Property<Guid>("TodoId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AppointeeId")
                        .HasColumnType("TEXT");

                    b.HasKey("TodoId", "AppointeeId");

                    b.HasIndex("AppointeeId");

                    b.ToTable("TodoAppointees");
                });

            modelBuilder.Entity("CleanArchitectureTest.Data.DTOs.Todo", b =>
                {
                    b.HasOne("CleanArchitectureTest.Data.DTOs.Member", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("CleanArchitectureTest.Data.DTOs.TodoAppointee", b =>
                {
                    b.HasOne("CleanArchitectureTest.Data.DTOs.Member", "Appointee")
                        .WithMany("TodoAppointees")
                        .HasForeignKey("AppointeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CleanArchitectureTest.Data.DTOs.Todo", "Todo")
                        .WithMany("TodoAppointees")
                        .HasForeignKey("TodoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointee");

                    b.Navigation("Todo");
                });

            modelBuilder.Entity("CleanArchitectureTest.Data.DTOs.Member", b =>
                {
                    b.Navigation("TodoAppointees");
                });

            modelBuilder.Entity("CleanArchitectureTest.Data.DTOs.Todo", b =>
                {
                    b.Navigation("TodoAppointees");
                });
#pragma warning restore 612, 618
        }
    }
}
