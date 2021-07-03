using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineExam.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<CourseUser> CourseUsers { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<QuestionResult> QuestionResults { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CourseUser>()
                .HasKey(cu => new { cu.CourseId, cu.UserId });
            builder.Entity<CourseUser>()
                .HasOne(cu => cu.Course)
                .WithMany(cu => cu.Users)
                .HasForeignKey(cu => cu.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CourseUser>()
                .HasOne(cu => cu.User)
                .WithMany(cu => cu.Courses)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(builder);
        }
    }
}
