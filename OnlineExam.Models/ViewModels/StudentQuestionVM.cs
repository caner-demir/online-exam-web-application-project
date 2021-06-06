using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineExam.Models.ViewModels
{
    public class StudentQuestionVM
    {
        public IEnumerable<int> QuestionIds { get; set; }
        public int CourseId { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
