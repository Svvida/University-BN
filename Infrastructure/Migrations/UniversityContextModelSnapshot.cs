﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(UniversityContext))]
    partial class UniversityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.AccountEntities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.AccountEntities.UserAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Users_Accounts", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.AccountEntities.UserAccountRole", b =>
                {
                    b.Property<Guid>("AccountId")
                        .HasColumnType("char(36)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("AccountId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users_Accounts_Roles", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.DegreeCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Degree_Courses", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.DegreeCourseSubject", b =>
                {
                    b.Property<int>("DegreeCourseId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("DegreeCourseId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Degree_Courses_Subjects", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.DegreePath", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DegreeCourseId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("DegreeCourseId");

                    b.ToTable("Degree_Paths", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DegreePathId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("DegreePathId");

                    b.ToTable("Modules", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.ModuleSubject", b =>
                {
                    b.Property<int>("ModuleId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("ModuleId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Modules_Subjects", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Subjects", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.EmployeeEntities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ConsentId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("ContactPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("DateOfAddmission")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("ConsentId")
                        .IsUnique();

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.EmployeeEntities.EmployeeAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ApartmentNumber")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("BuildingNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Employees_Addresses", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.EmployeeEntities.EmployeeConsent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("PermissionForDataProcessing")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("PermissionForPhoto")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Employees_Consents", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ConsentId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("ContactPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("DateOfAddmission")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("ConsentId")
                        .IsUnique();

                    b.ToTable("Students", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.StudentAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ApartmentNumber")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("BuildingNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Students_Addresses", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.StudentConsent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("PermissionForDataProcessing")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("PermissionForPhoto")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Students_Consents", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.StudentDegreeCourse", b =>
                {
                    b.Property<Guid>("StudentId")
                        .HasColumnType("char(36)");

                    b.Property<int>("DegreeCourseId")
                        .HasColumnType("int");

                    b.HasKey("StudentId", "DegreeCourseId");

                    b.HasIndex("DegreeCourseId");

                    b.ToTable("StudentDegreeCourses");
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.StudentDegreePath", b =>
                {
                    b.Property<Guid>("StudentId")
                        .HasColumnType("char(36)");

                    b.Property<int>("DegreePathId")
                        .HasColumnType("int");

                    b.HasKey("StudentId", "DegreePathId");

                    b.HasIndex("DegreePathId");

                    b.ToTable("StudentDegreePaths");
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.StudentModule", b =>
                {
                    b.Property<Guid>("StudentId")
                        .HasColumnType("char(36)");

                    b.Property<int>("ModuleId")
                        .HasColumnType("int");

                    b.HasKey("StudentId", "ModuleId");

                    b.HasIndex("ModuleId");

                    b.ToTable("StudentModules");
                });

            modelBuilder.Entity("Domain.Entities.AccountEntities.UserAccountRole", b =>
                {
                    b.HasOne("Domain.Entities.AccountEntities.UserAccount", "Account")
                        .WithMany("UserAccountRoles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.AccountEntities.Role", "Role")
                        .WithMany("UserAccountRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.DegreeCourseSubject", b =>
                {
                    b.HasOne("Domain.Entities.EducationEntities.DegreeCourse", "DegreeCourse")
                        .WithMany("DegreeCourseSubjects")
                        .HasForeignKey("DegreeCourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.EducationEntities.Subject", "Subject")
                        .WithMany("DegreeCourseSubjects")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DegreeCourse");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.DegreePath", b =>
                {
                    b.HasOne("Domain.Entities.EducationEntities.DegreeCourse", "DegreeCourse")
                        .WithMany("Paths")
                        .HasForeignKey("DegreeCourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DegreeCourse");
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.Module", b =>
                {
                    b.HasOne("Domain.Entities.EducationEntities.DegreePath", "Path")
                        .WithMany("Modules")
                        .HasForeignKey("DegreePathId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Path");
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.ModuleSubject", b =>
                {
                    b.HasOne("Domain.Entities.EducationEntities.Module", "Module")
                        .WithMany("ModuleSubjects")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.EducationEntities.Subject", "Subject")
                        .WithMany("ModuleSubjects")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Domain.Entities.EmployeeEntities.Employee", b =>
                {
                    b.HasOne("Domain.Entities.AccountEntities.UserAccount", "Account")
                        .WithOne("Employee")
                        .HasForeignKey("Domain.Entities.EmployeeEntities.Employee", "AccountId");

                    b.HasOne("Domain.Entities.EmployeeEntities.EmployeeAddress", "Address")
                        .WithOne("Employee")
                        .HasForeignKey("Domain.Entities.EmployeeEntities.Employee", "AddressId");

                    b.HasOne("Domain.Entities.EmployeeEntities.EmployeeConsent", "Consent")
                        .WithOne("Employee")
                        .HasForeignKey("Domain.Entities.EmployeeEntities.Employee", "ConsentId");

                    b.Navigation("Account");

                    b.Navigation("Address");

                    b.Navigation("Consent");
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.Student", b =>
                {
                    b.HasOne("Domain.Entities.AccountEntities.UserAccount", "Account")
                        .WithOne("Student")
                        .HasForeignKey("Domain.Entities.StudentEntities.Student", "AccountId");

                    b.HasOne("Domain.Entities.StudentEntities.StudentAddress", "Address")
                        .WithOne("Student")
                        .HasForeignKey("Domain.Entities.StudentEntities.Student", "AddressId");

                    b.HasOne("Domain.Entities.StudentEntities.StudentConsent", "Consent")
                        .WithOne("Student")
                        .HasForeignKey("Domain.Entities.StudentEntities.Student", "ConsentId");

                    b.Navigation("Account");

                    b.Navigation("Address");

                    b.Navigation("Consent");
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.StudentDegreeCourse", b =>
                {
                    b.HasOne("Domain.Entities.EducationEntities.DegreeCourse", "DegreeCourse")
                        .WithMany("StudentDegreeCourses")
                        .HasForeignKey("DegreeCourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.StudentEntities.Student", "Student")
                        .WithMany("StudentDegreeCourses")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DegreeCourse");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.StudentDegreePath", b =>
                {
                    b.HasOne("Domain.Entities.EducationEntities.DegreePath", "DegreePath")
                        .WithMany("StudentDegreePaths")
                        .HasForeignKey("DegreePathId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.StudentEntities.Student", "Student")
                        .WithMany("studentDegreePaths")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DegreePath");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.StudentModule", b =>
                {
                    b.HasOne("Domain.Entities.EducationEntities.Module", "Module")
                        .WithMany("StudentModules")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.StudentEntities.Student", "Student")
                        .WithMany("StudentModules")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Domain.Entities.AccountEntities.Role", b =>
                {
                    b.Navigation("UserAccountRoles");
                });

            modelBuilder.Entity("Domain.Entities.AccountEntities.UserAccount", b =>
                {
                    b.Navigation("Employee");

                    b.Navigation("Student");

                    b.Navigation("UserAccountRoles");
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.DegreeCourse", b =>
                {
                    b.Navigation("DegreeCourseSubjects");

                    b.Navigation("Paths");

                    b.Navigation("StudentDegreeCourses");
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.DegreePath", b =>
                {
                    b.Navigation("Modules");

                    b.Navigation("StudentDegreePaths");
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.Module", b =>
                {
                    b.Navigation("ModuleSubjects");

                    b.Navigation("StudentModules");
                });

            modelBuilder.Entity("Domain.Entities.EducationEntities.Subject", b =>
                {
                    b.Navigation("DegreeCourseSubjects");

                    b.Navigation("ModuleSubjects");
                });

            modelBuilder.Entity("Domain.Entities.EmployeeEntities.EmployeeAddress", b =>
                {
                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Domain.Entities.EmployeeEntities.EmployeeConsent", b =>
                {
                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.Student", b =>
                {
                    b.Navigation("StudentDegreeCourses");

                    b.Navigation("StudentModules");

                    b.Navigation("studentDegreePaths");
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.StudentAddress", b =>
                {
                    b.Navigation("Student");
                });

            modelBuilder.Entity("Domain.Entities.StudentEntities.StudentConsent", b =>
                {
                    b.Navigation("Student");
                });
#pragma warning restore 612, 618
        }
    }
}
