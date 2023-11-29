﻿// <auto-generated />
using System;
using CIS341_project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CIS341_project.Migrations
{
    [DbContext(typeof(BlogContext))]
    [Migration("20231128232508_CommentRepliesModelChanges")]
    partial class CommentRepliesModelChanges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CIS341_project.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("CIS341_project.Models.BlogPost", b =>
                {
                    b.Property<int>("BlogPostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlogPostId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DatePublished")
                        .HasColumnType("datetime2");

                    b.Property<string>("PostAuthor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("BlogPostId");

                    b.ToTable("BlogPosts");
                });

            modelBuilder.Entity("CIS341_project.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AuthorUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BlogPostId")
                        .HasColumnType("int");

                    b.Property<string>("CommentContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentCommentId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("BlogPostId");

                    b.HasIndex("ParentCommentId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("CIS341_project.Models.CommentReaction", b =>
                {
                    b.Property<int>("ReactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReactionId"));

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<string>("ReactionAuthorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ReactionId");

                    b.HasIndex("CommentId");

                    b.ToTable("CommentReactions");
                });

            modelBuilder.Entity("CIS341_project.Models.PostReaction", b =>
                {
                    b.Property<int>("ReactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReactionId"));

                    b.Property<int>("BlogPostId")
                        .HasColumnType("int");

                    b.Property<string>("ReactionAuthorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ReactionId");

                    b.HasIndex("BlogPostId");

                    b.ToTable("PostReactions");
                });

            modelBuilder.Entity("CIS341_project.Models.Comment", b =>
                {
                    b.HasOne("CIS341_project.Models.BlogPost", "BlogPost")
                        .WithMany("Comments")
                        .HasForeignKey("BlogPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIS341_project.Models.Comment", "ParentComment")
                        .WithMany("Replies")
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("BlogPost");

                    b.Navigation("ParentComment");
                });

            modelBuilder.Entity("CIS341_project.Models.CommentReaction", b =>
                {
                    b.HasOne("CIS341_project.Models.Comment", "Comment")
                        .WithMany("CommentReactions")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("CIS341_project.Models.PostReaction", b =>
                {
                    b.HasOne("CIS341_project.Models.BlogPost", "BlogPost")
                        .WithMany("PostReactions")
                        .HasForeignKey("BlogPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlogPost");
                });

            modelBuilder.Entity("CIS341_project.Models.BlogPost", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("PostReactions");
                });

            modelBuilder.Entity("CIS341_project.Models.Comment", b =>
                {
                    b.Navigation("CommentReactions");

                    b.Navigation("Replies");
                });
#pragma warning restore 612, 618
        }
    }
}
