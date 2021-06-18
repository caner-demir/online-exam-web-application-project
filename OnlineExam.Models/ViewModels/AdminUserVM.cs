using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineExam.Models.ViewModels
{
    public class AdminUserVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public int CourseCount { get; set; }
        public int StudentCount { get; set; }
    }
}
