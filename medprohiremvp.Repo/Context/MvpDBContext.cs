using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using medprohiremvp.DATA.Entity;

using medprohiremvp.DATA.IdentityModels;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;

namespace medprohiremvp.Repo.Context
{
    public class MvpDBContext : DbContext 
    {

        public MvpDBContext(DbContextOptions<MvpDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Applicants> Applicants { get; set; }
        public DbSet<ApplicantCertificates> ApplicantCertificates { get; set; }
        public DbSet<ApplicantSpecialities> ApplicantSpecialities { get; set; }
        public DbSet<ApplicantWorkHistories> ApplicantWorkHistories { get; set; }
        public DbSet<Availabilities> Availabilities { get; set; }
        public DbSet<ClinicalInstitutions> ClinicalInstitutions { get; set; }
        public DbSet<ClinicalInstitutionBranches> ClinicalInstitutionBranches { get; set; }
        public DbSet<DrugscreenStatuses> DrugscreenStatuses { get; set; }
        public DbSet<InstitutionTypes> InstitutionTypes { get; set; }
        public DbSet<Specialities> Specialities { get; set; }
        public DbSet<VisaStatuses> VisaStatuses { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<CertificateTypes> CertificateTypes { get; set; }
        public DbSet<ApplicantReferences> ApplicantReferences { get; set; }
        public DbSet<ClientShifts> ClientShifts { get; set; }
        public DbSet<ShiftSpecialities> ShiftSpecialities { get; set; }
        public DbSet<SignSent> SignSent { get; set; }
        public DbSet<ShiftLabels> ShiftLabels { get; set; }
        public DbSet<PhoneVerify> PhoneVerify { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<NotificationTemplates> NotificationTemplates { get; set; }

        public DbSet<ShiftCategory> ShiftCategories { get; set; }
        public DbSet<ApplicantAppliedShifts> ApplicantAppliedShifts { get; set; }
        public DbSet<ApplicantClockInClockOutTime> ApplicantClockInClockOutTime { get; set; }
        public DbSet<Administrators> Administrators { get; set; }
        public DbSet<AdminChanges> AdminChanges { get; set; }
        public DbSet<PayChecks> PayChecks { get; set; }
        public DbSet<EmailTemplates> EmailTemplates { get; set; }
        public DbSet<EmploymentAgreements> EmploymentAgreements { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<TicketCategories> TicketCategories { get; set; }
        public DbSet<TicketContents> TicketContents { get; set; }
        public DbSet<ApplicantAvailableTypes> ApplicantAvailableTypes { get; set; }
        public DbSet <ApplicantAvailables> ApplicantAvailables { get; set; }
        public DbSet <ApiKeys> ApiKeys { get; set; }
        public DbSet<ClientSpecialties> ClientSpecialties { get; set; }
        public DbSet<ClientSpecialtiesCosts> ClientSpecialtiesCosts { get; set; }
        public DbSet<ClientCostChanges> ClientCostChanges { get; set; }
        public DbSet<ApplicantAppliedShiftsDays> ApplicantAppliedShiftsDays { get; set; }
        public DbSet<ՕperatingApplicants> ՕperatingApplicants { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //    // configures  relationship
            //modelBuilder.Entity<ApplicantCertificates>()
            //    .HasOne<Applicant>(c => c.Applicant)
            //    .WithMany(a => a.Certificates)
            //    .HasForeignKey(ac => ac.Applicant_ID)
            //    .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            //modelBuilder.Entity<ApplicantSpecialities>()
            //        .HasOne<Applicant>(c => c.Applicant)
            //        .WithMany(a => a.Specialities)
            //        .HasForeignKey(ac => ac.Applicant_ID)
            //        .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            //modelBuilder.Entity<ApplicantWorkHistory>()
            //        .HasOne<Applicant>(c => c.Applicant)
            //        .WithMany(a => a.WorkHistories)
            //        .HasForeignKey(ac => ac.Applicant_ID)
            //      .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            //    modelBuilder.Entity<Applicant>()
            //      .HasOne<ApplicationUser>(c => c.User)
            //      .WithOne(a => a.Applicant)
            //      .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            //    modelBuilder.Entity<ClinicalInstitution>()
            //       .HasOne<ApplicationUser>(cb => cb.User)
            //       .WithOne(c => c.ClinicalInstitution)
            //       .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            //    modelBuilder.Entity<ClinicalInstitution>()
            //        .HasMany<ClinicalInstitutionBranches>(cb => cb.Branches)
            //        .WithOne(c => c.Institution)
            //        .HasForeignKey(ccb => ccb.Institution_ID)
            //        .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            //    modelBuilder.Entity<ClinicalInstitution>()
            //        .HasOne<InstitutionTypes>(c => c.InstitutionType)
            //        .WithMany(t => t.ClinicalInstitutions)
            //        .HasForeignKey(ct => ct.InstitutionType_ID)
            //        .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            //    modelBuilder.Entity<States>()
            //        .HasOne<Countries>(c => c.country)
            //        .WithMany(t => t.States)
            //        .HasForeignKey(ct => ct.country_id)
            //        .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            //    modelBuilder.Entity<Cities>()
            //        .HasOne<Countries>(c => c.country)
            //        .WithMany(t => t.Cities)
            //        .HasForeignKey(ct => ct.country_id)
            //        .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            //    modelBuilder.Entity<Cities>()
            //        .HasOne<States>(c => c.state)
            //        .WithMany(t => t.Cities)
            //        .HasForeignKey(ct => ct.state_id)
            //        .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            //    modelBuilder.Entity<ApplicationUser>()
            //        .HasOne<Cities>(c => c.Cities)
            //        .WithMany(u => u.ApplicationUsers)
            //        .HasForeignKey(cu => cu.City_ID)
            //        .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);

          

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var attributes = property.PropertyInfo.GetCustomAttributes(typeof(EncryptedAttribute), false);
                    if (attributes.Any())
                    {
                        property.SetValueConverter(new EncryptedConverter());
                    }
                }
            }
        }
        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        //public override async Task<int> SaveChangesAsync()
        //{
           
        //    AddTimestamps();
        //    return await base.SaveChangesAsync();
        //}

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is Datefields && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    (entity.Entity as Datefields).DateCreated = DateTime.Now;
                }
                ((Datefields)entity.Entity).DateModified = DateTime.Now;
             
            }
        }

    }

}


