using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineExam.Models
{
    public class QuestionResult
    {
        [Key]
        public int Id { get; set; }

        public int? ChoiceSelected { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        [Required]
        public int ExamResultId { get; set; }

        [ForeignKey("ExamResultId")]
        public ExamResult ExamResult { get; set; }
    }
}
