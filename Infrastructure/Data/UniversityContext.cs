using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UniversityContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAddress> EmployeesAddresses { get; set; }
        public DbSet<EmployeeConsent> EmployeesConsents { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleCourse> ModulesCourses { get; set; }
        public DbSet<DegreeProgram> Programs { get; set; }
        public DbSet<DegreeProgramCourse> ProgramsCourses { get; set; }
        public DbSet<DegreePath> DegreePaths { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentAddress> StudentsAddresses { get; set; }
        public DbSet<StudentConsent> StudentsConsents { get; set; }
        public DbSet<UserAccount> UsersAccounts { get; set; }
        public DbSet<UserAccountRole> UsersAccountsRoles { get; set; }

        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define table names
            modelBuilder.Entity<Course>().ToTable("Subjects");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<EmployeeAddress>().ToTable("Employees_Addresses");
            modelBuilder.Entity<EmployeeConsent>().ToTable("Employees_Consents");
            modelBuilder.Entity<Module>().ToTable("Modules");
            modelBuilder.Entity<ModuleCourse>().ToTable("Modules_Subjects");
            modelBuilder.Entity<DegreeProgram>().ToTable("Degree_Courses");
            modelBuilder.Entity<DegreeProgramCourse>().ToTable("Degree_Courses_Subjects");
            modelBuilder.Entity<DegreePath>().ToTable("Degree_Paths");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<StudentAddress>().ToTable("Students_Addresses");
            modelBuilder.Entity<StudentConsent>().ToTable("Students_Consents");
            modelBuilder.Entity<UserAccount>().ToTable("Users_Accounts");
            modelBuilder.Entity<UserAccountRole>().ToTable("Users_Accounts_Roles");

            // Define Employee properties

            // Many-to-many relationship between DegreeProgram and Course
            modelBuilder.Entity<DegreeProgramCourse>()
                .HasKey(pc => new { pc.ProgramId, pc.CourseId });

            modelBuilder.Entity<DegreeProgramCourse>()
                .HasOne(pc => pc.Program)
                .WithMany(p => p.ProgramCourses)
                .HasForeignKey(pc => pc.ProgramId);

            modelBuilder.Entity<DegreeProgramCourse>()
                .HasOne(pc => pc.Course)
                .WithMany(c => c.ProgramCourses)
                .HasForeignKey(pc => pc.CourseId);

            // One-to-many relationship between DegreePath and Module
            modelBuilder.Entity<Module>()
                .HasOne(m => m.Path)
                .WithMany(p => p.Modules)
                .HasForeignKey(m => m.DegreePathId);

            // One-to-many relationship between DegreePath and DegreeProgram
            modelBuilder.Entity<DegreePath>()
                .HasOne(dp => dp.Program)
                .WithMany(p => p.Paths)
                .HasForeignKey(dp => dp.ProgramId);

            // Many-to-many relationship between Module and Course
            modelBuilder.Entity<ModuleCourse>()
                .HasKey(mc => new { mc.ModuleId, mc.CourseId });

            modelBuilder.Entity<ModuleCourse>()
                .HasOne(mc => mc.Module)
                .WithMany(m => m.ModuleCourses)
                .HasForeignKey(mc => mc.ModuleId);

            modelBuilder.Entity<ModuleCourse>()
                .HasOne(mc => mc.Course)
                .WithMany(c => c.ModuleCourses)
                .HasForeignKey(mc => mc.CourseId);

            // Many-to-many relationship between UserAccount and Role
            modelBuilder.Entity<UserAccountRole>()
                .HasKey(uar => new { uar.AccountId, uar.RoleId });

            modelBuilder.Entity<UserAccountRole>()
                .HasOne(uar => uar.Account)
                .WithMany(a => a.UserAccountRoles)
                .HasForeignKey(uar => uar.AccountId);

            modelBuilder.Entity<UserAccountRole>()
                .HasOne(uar => uar.Role)
                .WithMany(r => r.UserAccountRoles)
                .HasForeignKey(uar => uar.RoleId);

            // One-to-one relationship between Employee and EmployeeAddress
            modelBuilder.Entity<EmployeeAddress>()
                .HasOne(ea => ea.Employee)
                .WithOne(e => e.Address)
                .HasForeignKey<Employee>(e => e.AddressId);

            // One-to-one relationship between Employee and EmployeeConsent
            modelBuilder.Entity<EmployeeConsent>()
                .HasOne(ec => ec.Employee)
                .WithOne(e => e.Consent)
                .HasForeignKey<Employee>(e => e.ConsentId);

            // One-to-one relationship between Student and StudentAddress
            modelBuilder.Entity<StudentAddress>()
                .HasOne(sa => sa.Student)
                .WithOne(s => s.Address)
                .HasForeignKey<Student>(s => s.AddressId);

            // One-to-one relationship between Student and StudentConsent
            modelBuilder.Entity<StudentConsent>()
                .HasOne(sc => sc.Student)
                .WithOne(s => s.Consent)
                .HasForeignKey<Student>(s => s.ConsentId);

            // One-to-one relationship between UserAccount and Employee
            modelBuilder.Entity<UserAccount>()
                .HasOne(ua => ua.Employee)
                .WithOne(e => e.Account)
                .HasForeignKey<Employee>(e => e.AccountId);

            // One-to-one relationship between UserAccount and Student
            modelBuilder.Entity<UserAccount>()
                .HasOne(ua => ua.Student)
                .WithOne(s => s.Account)
                .HasForeignKey<Student>(s => s.AccountId);
        }
    }
}
