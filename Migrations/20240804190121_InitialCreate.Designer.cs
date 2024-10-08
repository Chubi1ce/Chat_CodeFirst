﻿// <auto-generated />
using System;
using Chat_CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Chat_CodeFirst.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20240804190121_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Chat_CodeFirst.Models.Messages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("FromUserId")
                        .HasColumnType("integer")
                        .HasColumnName("fromUserId");

                    b.Property<bool>("Received")
                        .HasColumnType("boolean");

                    b.Property<string>("Text")
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.Property<int?>("ToUserId")
                        .HasColumnType("integer")
                        .HasColumnName("toUserId");

                    b.HasKey("Id")
                        .HasName("message_pkey");

                    b.HasIndex("FromUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("Messages", (string)null);
                });

            modelBuilder.Entity("Chat_CodeFirst.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("user_pkey");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Chat_CodeFirst.Models.Messages", b =>
                {
                    b.HasOne("Chat_CodeFirst.Models.Users", "FromUser")
                        .WithMany("FromMessages")
                        .HasForeignKey("FromUserId")
                        .HasConstraintName("messagesFromUserId_fkey");

                    b.HasOne("Chat_CodeFirst.Models.Users", "ToUser")
                        .WithMany("ToMessages")
                        .HasForeignKey("ToUserId")
                        .HasConstraintName("messagesToUserId_fkey");

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("Chat_CodeFirst.Models.Users", b =>
                {
                    b.Navigation("FromMessages");

                    b.Navigation("ToMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
