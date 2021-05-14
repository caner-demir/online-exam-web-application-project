using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineExam.Models.ViewModels
{
    public class CoursesVM
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<CourseUser> CourseUsers { get; set; }
    }
}
