using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineExam.Models
{
    public class CourseUser
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
