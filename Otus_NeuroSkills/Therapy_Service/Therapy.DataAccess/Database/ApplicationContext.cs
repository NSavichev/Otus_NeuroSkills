using Microsoft.EntityFrameworkCore;
using Therapy_Service.Therapy.Host.Dto;
using Therapy_Service.Therapy.Host.Models;
using TherapyService.Therapy.Host.Dto;

namespace Therapy_Service.Therapy.DataAccess.Database
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated(); //Создаем БД если нет
        }
        /// <summary>
        /// Контексты таблиц - Exercise, Progress
        /// </summary>
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Progress> Progresses { get; set; }

        /// <summary>
        /// Свои DTO
        /// </summary>
        public DbSet<ExerciseDto> ExerciseDtos { get; set; }
        public DbSet<ProgressDto> ProgressDtos { get; set; }

        //Для установления своих имен в БД а не тех что сгенерит EF
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exercise>().ToTable("ExerciseTest");
            modelBuilder.Entity<Progress>().ToTable("ProgressTest");
        }
    }
}
