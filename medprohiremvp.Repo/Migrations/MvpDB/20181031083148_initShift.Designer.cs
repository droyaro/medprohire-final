﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using medprohiremvp.Repo.Context;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    [DbContext(typeof(MvpDBContext))]
    [Migration("20181031083148_initShift")]
    partial class initShift
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("medprohiremvp.DATA.Entity.Applicant", b =>
                {
                    b.Property<int>("Applicant_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Availability_ID");

                    b.Property<bool>("BackgroundCheck");

                    b.Property<int>("BoardingProcess");

                    b.Property<bool>("CEU");

                    b.Property<string>("Contract");

                    b.Property<bool>("Drugscreen");

                    b.Property<int?>("DrugscreenStatus_ID");

                    b.Property<bool>("E_verify");

                    b.Property<string>("Employemen_agreement");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("I_9");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("PreferredID")
                        .IsRequired();

                    b.Property<string>("ProfileImage");

                    b.Property<string>("SSN")
                        .IsRequired();

                    b.Property<int>("Status_ID");

                    b.Property<string>("Sub_specialities");

                    b.Property<string>("TIN")
                        .IsRequired();

                    b.Property<Guid>("User_ID");

                    b.Property<int>("VisaStatus_ID");

                    b.Property<string>("W_4");

                    b.Property<bool>("WorkAuth");

                    b.HasKey("Applicant_ID");

                    b.HasIndex("Availability_ID");

                    b.HasIndex("DrugscreenStatus_ID");

                    b.HasIndex("User_ID")
                        .IsUnique();

                    b.HasIndex("VisaStatus_ID");

                    b.ToTable("Applicant");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ApplicantCertificates", b =>
                {
                    b.Property<int>("Ceritification_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Applicant_ID");

                    b.Property<string>("CeritifcationImg")
                        .IsRequired();

                    b.Property<int>("CertificateType_ID");

                    b.HasKey("Ceritification_ID");

                    b.HasIndex("Applicant_ID");

                    b.HasIndex("CertificateType_ID");

                    b.ToTable("ApplicantCertificates");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ApplicantReferences", b =>
                {
                    b.Property<int>("Reference_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Applicant_ID");

                    b.Property<string>("Company")
                        .IsRequired();

                    b.Property<string>("ContactName")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<string>("Position")
                        .IsRequired();

                    b.HasKey("Reference_ID");

                    b.HasIndex("Applicant_ID");

                    b.ToTable("ApplicantReferences");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ApplicantSpecialities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Applicant_ID");

                    b.Property<string>("License")
                        .IsRequired();

                    b.Property<int>("Speciality_ID");

                    b.HasKey("Id");

                    b.HasIndex("Applicant_ID");

                    b.HasIndex("Speciality_ID");

                    b.ToTable("ApplicantSpecialities");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ApplicantWorkHistory", b =>
                {
                    b.Property<int>("WorkHistory_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Applicant_ID");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("PlaceName")
                        .IsRequired();

                    b.Property<int>("Speciality_ID");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("WorkHistory_ID");

                    b.HasIndex("Applicant_ID");

                    b.HasIndex("Speciality_ID");

                    b.ToTable("ApplicantWorkHistory");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.Availabilities", b =>
                {
                    b.Property<int>("Availability_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Availability")
                        .IsRequired();

                    b.HasKey("Availability_ID");

                    b.ToTable("Availabilities");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.CertificateType", b =>
                {
                    b.Property<int>("Certificate_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Certificate_TypeName")
                        .IsRequired();

                    b.HasKey("Certificate_ID");

                    b.ToTable("CertificateType");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.Cities", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("city_name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("country_id");

                    b.Property<int>("state_id");

                    b.HasKey("id");

                    b.HasIndex("country_id");

                    b.HasIndex("state_id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ClientShift", b =>
                {
                    b.Property<int>("ClientShift_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ClockInTime");

                    b.Property<DateTime>("ClockOutTime");

                    b.Property<int>("ContractorCount");

                    b.Property<DateTime>("DateOfShift");

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("HourlyRate");

                    b.Property<int>("Institution_ID");

                    b.Property<string>("Responsibility")
                        .IsRequired();

                    b.Property<string>("ShiftDescription")
                        .IsRequired();

                    b.Property<DateTime>("ShiftExpirationDate");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("ClientShift_ID");

                    b.HasIndex("Institution_ID");

                    b.ToTable("ClientShift");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ClinicalInstitution", b =>
                {
                    b.Property<int>("Institution_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContactPerson")
                        .IsRequired();

                    b.Property<string>("ContactTitle")
                        .IsRequired();

                    b.Property<string>("InstitutionDescription");

                    b.Property<string>("InstitutionName")
                        .IsRequired();

                    b.Property<string>("InstitutionTaxId")
                        .IsRequired();

                    b.Property<int>("InstitutionType_ID");

                    b.Property<string>("Logo");

                    b.Property<bool>("Status");

                    b.Property<Guid>("User_ID");

                    b.HasKey("Institution_ID");

                    b.HasIndex("InstitutionType_ID");

                    b.HasIndex("User_ID")
                        .IsUnique();

                    b.ToTable("ClinicalInstitution");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ClinicalInstitutionBranches", b =>
                {
                    b.Property<int>("Branch_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("CityId")
                        .IsRequired();

                    b.Property<int>("Institution_ID");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("ZipCode")
                        .IsRequired();

                    b.HasKey("Branch_ID");

                    b.HasIndex("Institution_ID");

                    b.ToTable("ClinicalInstitutionBranches");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.Countries", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("shortname")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.HasKey("id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.DrugscreenStatuses", b =>
                {
                    b.Property<int>("DrugscreenStatus_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DrugscreenStatus")
                        .IsRequired();

                    b.HasKey("DrugscreenStatus_ID");

                    b.ToTable("DrugscreenStatuses");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.InstitutionTypes", b =>
                {
                    b.Property<int>("InstitutionType_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("InstitutionTypeNames")
                        .IsRequired();

                    b.HasKey("InstitutionType_ID");

                    b.ToTable("InstitutionTypes");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ShiftSpecialities", b =>
                {
                    b.Property<int>("ShiftSpeciality_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientShift_ID");

                    b.Property<int>("Speciality_ID");

                    b.HasKey("ShiftSpeciality_ID");

                    b.HasIndex("ClientShift_ID");

                    b.HasIndex("Speciality_ID");

                    b.ToTable("ShiftSpecialities");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.Speciality", b =>
                {
                    b.Property<int>("Speciality_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Speciality_Name")
                        .IsRequired();

                    b.HasKey("Speciality_ID");

                    b.ToTable("Speciality");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.States", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("country_id");

                    b.Property<string>("state_name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("id");

                    b.HasIndex("country_id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.VisaStatuses", b =>
                {
                    b.Property<int>("VisaStatus_ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("VisaStatus")
                        .IsRequired();

                    b.HasKey("VisaStatus_ID");

                    b.ToTable("VisaStatuses");
                });

            modelBuilder.Entity("medprohiremvp.DATA.IdentityModels.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address");

                    b.Property<int>("City_ID");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.Property<string>("ZipCode")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("City_ID");

                    b.ToTable("ApplicationUser");
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.Applicant", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.Availabilities", "Availability")
                        .WithMany()
                        .HasForeignKey("Availability_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("medprohiremvp.DATA.Entity.DrugscreenStatuses", "DrugscreenStatus")
                        .WithMany()
                        .HasForeignKey("DrugscreenStatus_ID");

                    b.HasOne("medprohiremvp.DATA.IdentityModels.ApplicationUser", "User")
                        .WithOne("Applicant")
                        .HasForeignKey("medprohiremvp.DATA.Entity.Applicant", "User_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("medprohiremvp.DATA.Entity.VisaStatuses", "VisaStatus")
                        .WithMany()
                        .HasForeignKey("VisaStatus_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ApplicantCertificates", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.Applicant", "Applicant")
                        .WithMany("Certificates")
                        .HasForeignKey("Applicant_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("medprohiremvp.DATA.Entity.CertificateType", "CertificateType")
                        .WithMany()
                        .HasForeignKey("CertificateType_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ApplicantReferences", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.Applicant", "applicant")
                        .WithMany()
                        .HasForeignKey("Applicant_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ApplicantSpecialities", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.Applicant", "Applicant")
                        .WithMany("Specialities")
                        .HasForeignKey("Applicant_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("medprohiremvp.DATA.Entity.Speciality", "Speciality")
                        .WithMany()
                        .HasForeignKey("Speciality_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ApplicantWorkHistory", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.Applicant", "Applicant")
                        .WithMany("WorkHistories")
                        .HasForeignKey("Applicant_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("medprohiremvp.DATA.Entity.Speciality", "Speciality")
                        .WithMany()
                        .HasForeignKey("Speciality_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.Cities", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.Countries", "country")
                        .WithMany("Cities")
                        .HasForeignKey("country_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("medprohiremvp.DATA.Entity.States", "state")
                        .WithMany("Cities")
                        .HasForeignKey("state_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ClientShift", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.ClinicalInstitution", "institution")
                        .WithMany("ClientShifts")
                        .HasForeignKey("Institution_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ClinicalInstitution", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.InstitutionTypes", "InstitutionType")
                        .WithMany("ClinicalInstitutions")
                        .HasForeignKey("InstitutionType_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("medprohiremvp.DATA.IdentityModels.ApplicationUser", "User")
                        .WithOne("ClinicalInstitution")
                        .HasForeignKey("medprohiremvp.DATA.Entity.ClinicalInstitution", "User_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ClinicalInstitutionBranches", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.ClinicalInstitution", "Institution")
                        .WithMany("Branches")
                        .HasForeignKey("Institution_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.ShiftSpecialities", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.ClientShift", "ClientShift")
                        .WithMany("ShiftSpecialities")
                        .HasForeignKey("ClientShift_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("medprohiremvp.DATA.Entity.Speciality", "Speciality")
                        .WithMany()
                        .HasForeignKey("Speciality_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("medprohiremvp.DATA.Entity.States", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.Countries", "country")
                        .WithMany("States")
                        .HasForeignKey("country_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("medprohiremvp.DATA.IdentityModels.ApplicationUser", b =>
                {
                    b.HasOne("medprohiremvp.DATA.Entity.Cities", "Cities")
                        .WithMany()
                        .HasForeignKey("City_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
