using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineExam.Models.ViewModels
{
    public class ExamsVM
    {
        public IEnumerable<Exam> Exams { get; set; }
        public Course Course { get; set; }
        public IEnumerable<Course> CoursesNavigation { get; set; }
        public IEnumerable<Exam> ExamsNavigation { get; set; }
        public int CountCourseUsers { get; set; }
    }
}
