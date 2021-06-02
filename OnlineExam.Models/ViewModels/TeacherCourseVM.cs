using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineExam.Models.ViewModels
{
    public class TeacherCourseVM
    {
        public Course Course { get; set; }
        public int Students { get; set; }
        public int Exams { get; set; }
    }
}
