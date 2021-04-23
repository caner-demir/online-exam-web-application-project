using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineExam.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int ExamId { get; set; }

        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }
    }
}
