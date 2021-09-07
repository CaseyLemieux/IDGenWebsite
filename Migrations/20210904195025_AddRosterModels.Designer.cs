﻿// <auto-generated />
using System;
using IDGenWebsite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IDGenWebsite.Migrations
{
    [DbContext(typeof(SchoolContext))]
    [Migration("20210904195025_AddRosterModels")]
    partial class AddRosterModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("IDGenWebsite.Models.EnrollmentsModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SchoolID")
                        .HasColumnType("longtext");

                    b.Property<string>("SectionID")
                        .HasColumnType("longtext");

                    b.Property<string>("StudentID")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("IDGenWebsite.Models.HomeroomsModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Teacher")
                        .HasColumnType("longtext");

                    b.Property<string>("TeacherEmail")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Homerooms");
                });

            modelBuilder.Entity("IDGenWebsite.Models.IDRequestModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Cost")
                        .HasColumnType("double");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DatePrinted")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("HasBeenPrinted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("StudentID")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("IDRequests");
                });

            modelBuilder.Entity("IDGenWebsite.Models.IdTemplate", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BackBottomColor")
                        .HasColumnType("longtext");

                    b.Property<string>("BackInfoLineOne")
                        .HasColumnType("longtext");

                    b.Property<string>("BackInfoLineThree")
                        .HasColumnType("longtext");

                    b.Property<string>("BackInfoLineTwo")
                        .HasColumnType("longtext");

                    b.Property<string>("BackMiddleColor")
                        .HasColumnType("longtext");

                    b.Property<string>("BackTopColor")
                        .HasColumnType("longtext");

                    b.Property<string>("BackTopLabel")
                        .HasColumnType("longtext");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FrontBottomColor")
                        .HasColumnType("longtext");

                    b.Property<string>("FrontMiddleColor")
                        .HasColumnType("longtext");

                    b.Property<string>("FrontTopColor")
                        .HasColumnType("longtext");

                    b.Property<string>("FrontTopLabel")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LogoPath")
                        .HasColumnType("longtext");

                    b.Property<string>("TemplateName")
                        .HasColumnType("longtext");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("IdTemplates");
                });

            modelBuilder.Entity("IDGenWebsite.Models.SchoolsModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SchoolID")
                        .HasColumnType("longtext");

                    b.Property<string>("SchoolName")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("IDGenWebsite.Models.SectionsModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SectionID")
                        .HasColumnType("longtext");

                    b.Property<string>("TeacherID")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("IDGenWebsite.Models.SettingModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SettingDescription")
                        .HasColumnType("longtext");

                    b.Property<string>("SettingName")
                        .HasColumnType("longtext");

                    b.Property<string>("SettingValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("IDGenWebsite.Models.StudentModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EnrollmentStartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("GradeLevel")
                        .HasColumnType("longtext");

                    b.Property<bool>("HasBeenManuallyEdited")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("HomeRoomTeacher")
                        .HasColumnType("longtext");

                    b.Property<string>("HomeRoomTeacherEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("IdPicPath")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("QrCode")
                        .HasColumnType("longtext");

                    b.Property<string>("SchoolID")
                        .HasColumnType("longtext");

                    b.Property<string>("StudentID")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
