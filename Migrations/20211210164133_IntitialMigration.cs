using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IDGenWebsite.Migrations
{
    public partial class IntitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicSessions",
                columns: table => new
                {
                    SessionSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parent_SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSessions", x => x.SessionSourcedId);
                    table.ForeignKey(
                        name: "FK_AcademicSessions_AcademicSessions_Parent_SourcedId",
                        column: x => x.Parent_SourcedId,
                        principalTable: "AcademicSessions",
                        principalColumn: "SessionSourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IDRequests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatePrinted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    HasBeenPrinted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDRequests", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IdTemplates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontTopColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontMiddleColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontBottomColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackTopColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackMiddleColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackBottomColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontTopLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackTopLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackInfoLineOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackInfoLineTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackInfoLineThree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdTemplates", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Orgs",
                columns: table => new
                {
                    OrgSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parent_SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orgs", x => x.OrgSourcedId);
                    table.ForeignKey(
                        name: "FK_Orgs_Orgs_Parent_SourcedId",
                        column: x => x.Parent_SourcedId,
                        principalTable: "Orgs",
                        principalColumn: "OrgSourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SettingDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnabledUser = table.Column<bool>(type: "bit", nullable: false),
                    GivenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdPicPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserSourcedId);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolYearSessionSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationOrgSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SessionSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseSourcedId);
                    table.ForeignKey(
                        name: "FK_Courses_AcademicSessions_SchoolYearSessionSourcedId",
                        column: x => x.SchoolYearSessionSourcedId,
                        principalTable: "AcademicSessions",
                        principalColumn: "SessionSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Orgs_OrganizationOrgSourcedId",
                        column: x => x.OrganizationOrgSourcedId,
                        principalTable: "Orgs",
                        principalColumn: "OrgSourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationsUsers",
                columns: table => new
                {
                    OrganizationsOrgSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationsUsers", x => new { x.OrganizationsOrgSourcedId, x.UserSourcedId });
                    table.ForeignKey(
                        name: "FK_OrganizationsUsers_Orgs_OrganizationsOrgSourcedId",
                        column: x => x.OrganizationsOrgSourcedId,
                        principalTable: "Orgs",
                        principalColumn: "OrgSourcedId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationsUsers_Users_UserSourcedId",
                        column: x => x.UserSourcedId,
                        principalTable: "Users",
                        principalColumn: "UserSourcedId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseSourcedId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolOrgSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrgSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassSourcedId);
                    table.ForeignKey(
                        name: "FK_Classes_Courses_CourseSourcedId1",
                        column: x => x.CourseSourcedId1,
                        principalTable: "Courses",
                        principalColumn: "CourseSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Orgs_SchoolOrgSourcedId",
                        column: x => x.SchoolOrgSourcedId,
                        principalTable: "Orgs",
                        principalColumn: "OrgSourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AcademicSessionsClasses",
                columns: table => new
                {
                    AcademicSessionsSessionSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassesClassSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSessionsClasses", x => new { x.AcademicSessionsSessionSourcedId, x.ClassesClassSourcedId });
                    table.ForeignKey(
                        name: "FK_AcademicSessionsClasses_AcademicSessions_AcademicSessionsSessionSourcedId",
                        column: x => x.AcademicSessionsSessionSourcedId,
                        principalTable: "AcademicSessions",
                        principalColumn: "SessionSourcedId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicSessionsClasses_Classes_ClassesClassSourcedId",
                        column: x => x.ClassesClassSourcedId,
                        principalTable: "Classes",
                        principalColumn: "ClassSourcedId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EnrollmentSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserSourcedId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassSourcedId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolOrgSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Primary = table.Column<bool>(type: "bit", nullable: false),
                    BeginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EnrollmentSourcedId);
                    table.ForeignKey(
                        name: "FK_Enrollments_Classes_ClassSourcedId1",
                        column: x => x.ClassSourcedId1,
                        principalTable: "Classes",
                        principalColumn: "ClassSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Orgs_SchoolOrgSourcedId",
                        column: x => x.SchoolOrgSourcedId,
                        principalTable: "Orgs",
                        principalColumn: "OrgSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Users_UserSourcedId1",
                        column: x => x.UserSourcedId1,
                        principalTable: "Users",
                        principalColumn: "UserSourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    GradeSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassesClassSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.GradeSourcedId);
                    table.ForeignKey(
                        name: "FK_Grades_Classes_ClassesClassSourcedId",
                        column: x => x.ClassesClassSourcedId,
                        principalTable: "Classes",
                        principalColumn: "ClassSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_Courses_CourseSourcedId",
                        column: x => x.CourseSourcedId,
                        principalTable: "Courses",
                        principalColumn: "CourseSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_Users_UserSourcedId",
                        column: x => x.UserSourcedId,
                        principalTable: "Users",
                        principalColumn: "UserSourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    PeriodSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassesClassSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.PeriodSourcedId);
                    table.ForeignKey(
                        name: "FK_Periods_Classes_ClassesClassSourcedId",
                        column: x => x.ClassesClassSourcedId,
                        principalTable: "Classes",
                        principalColumn: "ClassSourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    ResourceSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Importance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorResourceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassesClassSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.ResourceSourcedId);
                    table.ForeignKey(
                        name: "FK_Resources_Classes_ClassesClassSourcedId",
                        column: x => x.ClassesClassSourcedId,
                        principalTable: "Classes",
                        principalColumn: "ClassSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resources_Courses_CourseSourcedId",
                        column: x => x.CourseSourcedId,
                        principalTable: "Courses",
                        principalColumn: "CourseSourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectCodes",
                columns: table => new
                {
                    SubjectCodeSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassesClassSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectCodes", x => x.SubjectCodeSourcedId);
                    table.ForeignKey(
                        name: "FK_SubjectCodes_Classes_ClassesClassSourcedId",
                        column: x => x.ClassesClassSourcedId,
                        principalTable: "Classes",
                        principalColumn: "ClassSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectCodes_Courses_CourseSourcedId",
                        column: x => x.CourseSourcedId,
                        principalTable: "Courses",
                        principalColumn: "CourseSourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassesClassSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectSourcedId);
                    table.ForeignKey(
                        name: "FK_Subjects_Classes_ClassesClassSourcedId",
                        column: x => x.ClassesClassSourcedId,
                        principalTable: "Classes",
                        principalColumn: "ClassSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subjects_Courses_CourseSourcedId",
                        column: x => x.CourseSourcedId,
                        principalTable: "Courses",
                        principalColumn: "CourseSourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Metadatas",
                columns: table => new
                {
                    MetadataSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicSessionsSessionSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassesClassSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnrollmentSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganizationsOrgSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResourceSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadatas", x => x.MetadataSourcedId);
                    table.ForeignKey(
                        name: "FK_Metadatas_AcademicSessions_AcademicSessionsSessionSourcedId",
                        column: x => x.AcademicSessionsSessionSourcedId,
                        principalTable: "AcademicSessions",
                        principalColumn: "SessionSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Metadatas_Classes_ClassesClassSourcedId",
                        column: x => x.ClassesClassSourcedId,
                        principalTable: "Classes",
                        principalColumn: "ClassSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Metadatas_Courses_CourseSourcedId",
                        column: x => x.CourseSourcedId,
                        principalTable: "Courses",
                        principalColumn: "CourseSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Metadatas_Enrollments_EnrollmentSourcedId",
                        column: x => x.EnrollmentSourcedId,
                        principalTable: "Enrollments",
                        principalColumn: "EnrollmentSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Metadatas_Orgs_OrganizationsOrgSourcedId",
                        column: x => x.OrganizationsOrgSourcedId,
                        principalTable: "Orgs",
                        principalColumn: "OrgSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Metadatas_Resources_ResourceSourcedId",
                        column: x => x.ResourceSourcedId,
                        principalTable: "Resources",
                        principalColumn: "ResourceSourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Metadatas_Users_UserSourcedId",
                        column: x => x.UserSourcedId,
                        principalTable: "Users",
                        principalColumn: "UserSourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessions_Parent_SourcedId",
                table: "AcademicSessions",
                column: "Parent_SourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessionsClasses_ClassesClassSourcedId",
                table: "AcademicSessionsClasses",
                column: "ClassesClassSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CourseSourcedId1",
                table: "Classes",
                column: "CourseSourcedId1");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolOrgSourcedId",
                table: "Classes",
                column: "SchoolOrgSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_OrganizationOrgSourcedId",
                table: "Courses",
                column: "OrganizationOrgSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SchoolYearSessionSourcedId",
                table: "Courses",
                column: "SchoolYearSessionSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_ClassSourcedId1",
                table: "Enrollments",
                column: "ClassSourcedId1");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SchoolOrgSourcedId",
                table: "Enrollments",
                column: "SchoolOrgSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_UserSourcedId1",
                table: "Enrollments",
                column: "UserSourcedId1");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ClassesClassSourcedId",
                table: "Grades",
                column: "ClassesClassSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_CourseSourcedId",
                table: "Grades",
                column: "CourseSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_UserSourcedId",
                table: "Grades",
                column: "UserSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Metadatas_AcademicSessionsSessionSourcedId",
                table: "Metadatas",
                column: "AcademicSessionsSessionSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Metadatas_ClassesClassSourcedId",
                table: "Metadatas",
                column: "ClassesClassSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Metadatas_CourseSourcedId",
                table: "Metadatas",
                column: "CourseSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Metadatas_EnrollmentSourcedId",
                table: "Metadatas",
                column: "EnrollmentSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Metadatas_OrganizationsOrgSourcedId",
                table: "Metadatas",
                column: "OrganizationsOrgSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Metadatas_ResourceSourcedId",
                table: "Metadatas",
                column: "ResourceSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Metadatas_UserSourcedId",
                table: "Metadatas",
                column: "UserSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationsUsers_UserSourcedId",
                table: "OrganizationsUsers",
                column: "UserSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Orgs_Parent_SourcedId",
                table: "Orgs",
                column: "Parent_SourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_ClassesClassSourcedId",
                table: "Periods",
                column: "ClassesClassSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ClassesClassSourcedId",
                table: "Resources",
                column: "ClassesClassSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_CourseSourcedId",
                table: "Resources",
                column: "CourseSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectCodes_ClassesClassSourcedId",
                table: "SubjectCodes",
                column: "ClassesClassSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectCodes_CourseSourcedId",
                table: "SubjectCodes",
                column: "CourseSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ClassesClassSourcedId",
                table: "Subjects",
                column: "ClassesClassSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CourseSourcedId",
                table: "Subjects",
                column: "CourseSourcedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicSessionsClasses");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "IDRequests");

            migrationBuilder.DropTable(
                name: "IdTemplates");

            migrationBuilder.DropTable(
                name: "Metadatas");

            migrationBuilder.DropTable(
                name: "OrganizationsUsers");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SubjectCodes");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "AcademicSessions");

            migrationBuilder.DropTable(
                name: "Orgs");
        }
    }
}
