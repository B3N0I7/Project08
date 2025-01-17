﻿// <auto-generated />
using System;
using ConsultantCalendarApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConsultantCalendarApi.Migrations
{
    [DbContext(typeof(ConsultantCalendarDbContext))]
    [Migration("20240407070209_customizeConsultantCalendarDb")]
    partial class customizeConsultantCalendarDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ConsultantCalendarApi.Models.ConsultantCalendarModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ConsultantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsAppointment")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ConsultantId");

                    b.ToTable("ConsultantCalendars");
                });

            modelBuilder.Entity("ConsultantCalendarApi.Models.ConsultantModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConsultantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConsultantSpecialty")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Consultants");
                });

            modelBuilder.Entity("ConsultantCalendarApi.Models.ConsultantCalendarModel", b =>
                {
                    b.HasOne("ConsultantCalendarApi.Models.ConsultantModel", "Consultant")
                        .WithMany("ConsultantCalendars")
                        .HasForeignKey("ConsultantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consultant");
                });

            modelBuilder.Entity("ConsultantCalendarApi.Models.ConsultantModel", b =>
                {
                    b.Navigation("ConsultantCalendars");
                });
#pragma warning restore 612, 618
        }
    }
}
