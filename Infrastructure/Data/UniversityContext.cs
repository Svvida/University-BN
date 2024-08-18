using Domain.Entities.AccountEntities;
using Domain.Entities.EducationEntities;
using Domain.Entities.EmployeeEntities;
using Domain.Entities.EventEntities;
using Domain.Entities.ExternalEntities;
using Domain.Entities.StudentEntities;
using Domain.EntitiesBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UniversityContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAddress> EmployeesAddresses { get; set; }
        public DbSet<EmployeeConsent> EmployeesConsents { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleSubject> ModulesSubjects { get; set; }
        public DbSet<DegreeCourse> DegreeCourses { get; set; }
        public DbSet<DegreeCourseSubject> DegreeCourseSubjects { get; set; }
        public DbSet<DegreePath> DegreePaths { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentAddress> StudentsAddresses { get; set; }
        public DbSet<StudentConsent> StudentsConsents { get; set; }
        public DbSet<StudentDegreeCourse> StudentDegreeCourses { get; set; }
        public DbSet<StudentDegreePath> StudentDegreePaths { get; set; }
        public DbSet<StudentModule> StudentModules { get; set; }
        public DbSet<UserAccount> UsersAccounts { get; set; }
        public DbSet<UserAccountRole> UsersAccountsRoles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventOrganizer> EventOrganizers { get; set; }
        public DbSet<EventOrganizerEvents> EventOrganizerEvents { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ExternalParticipant> ExternalParticipants { get; set; }
        public DbSet<ExternalParticipantComanies> ExternalParticipantComanies { get; set; }

        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define table names
            modelBuilder.Entity<Subject>().ToTable("Subjects");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<EmployeeAddress>().ToTable("Employees_Addresses");
            modelBuilder.Entity<EmployeeConsent>().ToTable("Employees_Consents");
            modelBuilder.Entity<Module>().ToTable("Modules");
            modelBuilder.Entity<ModuleSubject>().ToTable("Modules_Subjects");
            modelBuilder.Entity<DegreeCourse>().ToTable("Degree_Courses");
            modelBuilder.Entity<DegreeCourseSubject>().ToTable("Degree_Courses_Subjects");
            modelBuilder.Entity<DegreePath>().ToTable("Degree_Paths");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<StudentAddress>().ToTable("Students_Addresses");
            modelBuilder.Entity<StudentConsent>().ToTable("Students_Consents");
            modelBuilder.Entity<UserAccount>().ToTable("Users_Accounts");
            modelBuilder.Entity<UserAccountRole>().ToTable("Users_Accounts_Roles");
            modelBuilder.Entity<Event>().ToTable("Events");
            modelBuilder.Entity<EventOrganizer>().ToTable("Events_Organizers");
            modelBuilder.Entity<EventOrganizerEvents>().ToTable("Events_Organizers_Events");
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<ExternalParticipant>().ToTable("External_Participants");
            modelBuilder.Entity<ExternalParticipantComanies>().ToTable("External_Participants_Companies");

            // Many-to-many relationship between Student and DegreeCourse
            modelBuilder.Entity<StudentDegreeCourse>()
                .HasKey(sdc => new { sdc.StudentId, sdc.DegreeCourseId });

            modelBuilder.Entity<StudentDegreeCourse>()
                .HasOne(sdc => sdc.Student)
                .WithMany(s => s.StudentDegreeCourses)
                .HasForeignKey(sdc => sdc.StudentId);

            modelBuilder.Entity<StudentDegreeCourse>()
                .HasOne(sdc => sdc.DegreeCourse)
                .WithMany(dc => dc.StudentDegreeCourses)
                .HasForeignKey(sdc => sdc.DegreeCourseId);

            // Many-to-many relationship between Student and DegreePath
            modelBuilder.Entity<StudentDegreePath>()
                .HasKey(sdp => new { sdp.StudentId, sdp.DegreePathId });

            modelBuilder.Entity<StudentDegreePath>()
                .HasOne(sdp => sdp.Student)
                .WithMany(s => s.studentDegreePaths)
                .HasForeignKey(sdp => sdp.StudentId);

            modelBuilder.Entity<StudentDegreePath>()
                .HasOne(sdp => sdp.DegreePath)
                .WithMany(dp => dp.StudentDegreePaths)
                .HasForeignKey(sdp => sdp.DegreePathId);

            // Many-to-many relationship between Student and Module
            modelBuilder.Entity<StudentModule>()
                .HasKey(sm => new { sm.StudentId, sm.ModuleId });

            modelBuilder.Entity<StudentModule>()
                .HasOne(sm => sm.Student)
                .WithMany(s => s.StudentModules)
                .HasForeignKey(sm => sm.StudentId);

            modelBuilder.Entity<StudentModule>()
                .HasOne(sm => sm.Module)
                .WithMany(m => m.StudentModules)
                .HasForeignKey(sm => sm.ModuleId);

            // Many-to-many relationship between DegreeProgram and Course
            modelBuilder.Entity<DegreeCourseSubject>()
                .HasKey(pc => new { pc.DegreeCourseId, pc.SubjectId });

            modelBuilder.Entity<DegreeCourseSubject>()
                .HasOne(pc => pc.DegreeCourse)
                .WithMany(p => p.DegreeCourseSubjects)
                .HasForeignKey(pc => pc.DegreeCourseId);

            modelBuilder.Entity<DegreeCourseSubject>()
                .HasOne(pc => pc.Subject)
                .WithMany(c => c.DegreeCourseSubjects)
                .HasForeignKey(pc => pc.SubjectId);

            // One-to-many relationship between DegreePath and Module
            modelBuilder.Entity<Module>()
                .HasOne(m => m.Path)
                .WithMany(p => p.Modules)
                .HasForeignKey(m => m.DegreePathId);

            // One-to-many relationship between DegreePath and DegreeProgram
            modelBuilder.Entity<DegreePath>()
                .HasOne(dp => dp.DegreeCourse)
                .WithMany(p => p.Paths)
                .HasForeignKey(dp => dp.DegreeCourseId);

            // Many-to-many relationship between Module and Course
            modelBuilder.Entity<ModuleSubject>()
                .HasKey(mc => new { mc.ModuleId, mc.SubjectId });

            modelBuilder.Entity<ModuleSubject>()
                .HasOne(mc => mc.Module)
                .WithMany(m => m.ModuleSubjects)
                .HasForeignKey(mc => mc.ModuleId);

            modelBuilder.Entity<ModuleSubject>()
                .HasOne(mc => mc.Subject)
                .WithMany(c => c.ModuleSubjects)
                .HasForeignKey(mc => mc.SubjectId);

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

            // Many-to-many relationship between Event and EventOrganizer
            modelBuilder.Entity<EventOrganizerEvents>()
                .HasKey(eoe => new { eoe.EventId, eoe.EventOrganizerId });

            modelBuilder.Entity<EventOrganizerEvents>()
                .HasOne(eoe => eoe.Event)
                .WithMany(e => e.EventOrganizersEvents)
                .HasForeignKey(eoe => eoe.EventId);

            modelBuilder.Entity<EventOrganizerEvents>()
                .HasOne(eoe => eoe.EventOrganizer)
                .WithMany(eo => eo.EventOrganizersEvents)
                .HasForeignKey(eoe => eoe.EventOrganizerId);

            // Many-to-many relationship between Company and ExternalParticipant
            modelBuilder.Entity<ExternalParticipantComanies>()
                .HasKey(epc => new { epc.CompanyId, epc.ExternalParticipantId });

            modelBuilder.Entity<ExternalParticipantComanies>()
                .HasOne(epc => epc.Company)
                .WithMany(c => c.ExternalParticipantComanies)
                .HasForeignKey(epc => epc.CompanyId);

            modelBuilder.Entity<ExternalParticipantComanies>()
                .HasOne(epc => epc.ExternalParticipant)
                .WithMany(ep => ep.ExternalParticipantComanies)
                .HasForeignKey(epc => epc.ExternalParticipantId);

            // One-to-one relationship between EventOrganizer and UserAccount
            modelBuilder.Entity<UserAccount>()
                .HasOne(ua => ua.EventOrganizer)
                .WithOne(eo => eo.Account)
                .HasForeignKey<EventOrganizer>(eo => eo.AccountId);

            // One-to-one relationship between Company and UserAccount
            modelBuilder.Entity<UserAccount>()
                .HasOne(ua => ua.Company)
                .WithOne(c => c.Account)
                .HasForeignKey<Company>(c => c.AccountId);

            // One-to-one relationship between ExternalParticipant and UserAccount
            modelBuilder.Entity<UserAccount>()
                .HasOne(ua => ua.ExternalParticipant)
                .WithOne(ep => ep.Account)
                .HasForeignKey<ExternalParticipant>(ep => ep.AccountId);
        }

        // Override the SaveChanges method
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        // Override the SaveChangesAsymc method
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker
                .Entries<AuditableEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
