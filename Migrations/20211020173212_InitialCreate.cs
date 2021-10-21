using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IDGenWebsite.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicSessions",
                columns: table => new
                {
                    SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolYear = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSessions", x => x.SourcedId);
                    table.ForeignKey(
                        name: "FK_AcademicSessions_AcademicSessions_ParentSourcedId",
                        column: x => x.ParentSourcedId,
                        principalTable: "AcademicSessions",
                        principalColumn: "SourcedId",
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
                    SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Users", x => x.SourcedId);
                });

            migrationBuilder.CreateTable(
                name: "Orgs",
                columns: table => new
                {
                    SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsersSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orgs", x => x.SourcedId);
                    table.ForeignKey(
                        name: "FK_Orgs_Orgs_ParentSourcedId",
                        column: x => x.ParentSourcedId,
                        principalTable: "Orgs",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orgs_Users_UsersSourcedId",
                        column: x => x.UsersSourcedId,
                        principalTable: "Users",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolYearSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CourseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.SourcedId);
                    table.ForeignKey(
                        name: "FK_Courses_AcademicSessions_SchoolYearSourcedId",
                        column: x => x.SchoolYearSourcedId,
                        principalTable: "AcademicSessions",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Orgs_OrganizationSourcedId",
                        column: x => x.OrganizationSourcedId,
                        principalTable: "Orgs",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.SourcedId);
                    table.ForeignKey(
                        name: "FK_Classes_Courses_CourseSourcedId",
                        column: x => x.CourseSourcedId,
                        principalTable: "Courses",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Orgs_SchoolSourcedId",
                        column: x => x.SchoolSourcedId,
                        principalTable: "Orgs",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AcademicSessionsClasses",
                columns: table => new
                {
                    AcademicSessionsSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassesSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSessionsClasses", x => new { x.AcademicSessionsSourcedId, x.ClassesSourcedId });
                    table.ForeignKey(
                        name: "FK_AcademicSessionsClasses_AcademicSessions_AcademicSessionsSourcedId",
                        column: x => x.AcademicSessionsSourcedId,
                        principalTable: "AcademicSessions",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicSessionsClasses_Classes_ClassesSourcedId",
                        column: x => x.ClassesSourcedId,
                        principalTable: "Classes",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SchoolSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Primary = table.Column<bool>(type: "bit", nullable: false),
                    BeginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.SourcedId);
                    table.ForeignKey(
                        name: "FK_Enrollments_Classes_ClassSourcedId",
                        column: x => x.ClassSourcedId,
                        principalTable: "Classes",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Orgs_SchoolSourcedId",
                        column: x => x.SchoolSourcedId,
                        principalTable: "Orgs",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Users_UserSourcedId",
                        column: x => x.UserSourcedId,
                        principalTable: "Users",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoursesSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClassesSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UsersSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.SourcedId);
                    table.ForeignKey(
                        name: "FK_Grades_Classes_ClassesSourcedId",
                        column: x => x.ClassesSourcedId,
                        principalTable: "Classes",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_Courses_CoursesSourcedId",
                        column: x => x.CoursesSourcedId,
                        principalTable: "Courses",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_Users_UsersSourcedId",
                        column: x => x.UsersSourcedId,
                        principalTable: "Users",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassesSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.SourcedId);
                    table.ForeignKey(
                        name: "FK_Periods_Classes_ClassesSourcedId",
                        column: x => x.ClassesSourcedId,
                        principalTable: "Classes",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Importance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorResourceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassesSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoursesSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.SourcedId);
                    table.ForeignKey(
                        name: "FK_Resources_Classes_ClassesSourcedId",
                        column: x => x.ClassesSourcedId,
                        principalTable: "Classes",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resources_Courses_CoursesSourcedId",
                        column: x => x.CoursesSourcedId,
                        principalTable: "Courses",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectCodes",
                columns: table => new
                {
                    SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassesSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoursesSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectCodes", x => x.SourcedId);
                    table.ForeignKey(
                        name: "FK_SubjectCodes_Classes_ClassesSourcedId",
                        column: x => x.ClassesSourcedId,
                        principalTable: "Classes",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectCodes_Courses_CoursesSourcedId",
                        column: x => x.CoursesSourcedId,
                        principalTable: "Courses",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassesSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoursesSourcedId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SourcedId);
                    table.ForeignKey(
                        name: "FK_Subjects_Classes_ClassesSourcedId",
                        column: x => x.ClassesSourcedId,
                        principalTable: "Classes",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subjects_Courses_CoursesSourcedId",
                        column: x => x.CoursesSourcedId,
                        principalTable: "Courses",
                        principalColumn: "SourcedId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessions_ParentSourcedId",
                table: "AcademicSessions",
                column: "ParentSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessionsClasses_ClassesSourcedId",
                table: "AcademicSessionsClasses",
                column: "ClassesSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CourseSourcedId",
                table: "Classes",
                column: "CourseSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolSourcedId",
                table: "Classes",
                column: "SchoolSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_OrganizationSourcedId",
                table: "Courses",
                column: "OrganizationSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SchoolYearSourcedId",
                table: "Courses",
                column: "SchoolYearSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_ClassSourcedId",
                table: "Enrollments",
                column: "ClassSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SchoolSourcedId",
                table: "Enrollments",
                column: "SchoolSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_UserSourcedId",
                table: "Enrollments",
                column: "UserSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ClassesSourcedId",
                table: "Grades",
                column: "ClassesSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_CoursesSourcedId",
                table: "Grades",
                column: "CoursesSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_UsersSourcedId",
                table: "Grades",
                column: "UsersSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Orgs_ParentSourcedId",
                table: "Orgs",
                column: "ParentSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Orgs_UsersSourcedId",
                table: "Orgs",
                column: "UsersSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_ClassesSourcedId",
                table: "Periods",
                column: "ClassesSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ClassesSourcedId",
                table: "Resources",
                column: "ClassesSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_CoursesSourcedId",
                table: "Resources",
                column: "CoursesSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectCodes_ClassesSourcedId",
                table: "SubjectCodes",
                column: "ClassesSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectCodes_CoursesSourcedId",
                table: "SubjectCodes",
                column: "CoursesSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ClassesSourcedId",
                table: "Subjects",
                column: "ClassesSourcedId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CoursesSourcedId",
                table: "Subjects",
                column: "CoursesSourcedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicSessionsClasses");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "IDRequests");

            migrationBuilder.DropTable(
                name: "IdTemplates");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SubjectCodes");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "AcademicSessions");

            migrationBuilder.DropTable(
                name: "Orgs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
