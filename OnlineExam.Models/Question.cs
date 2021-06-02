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

        public int? CorrectChoice { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public int Points { get; set; }

        public ICollection<Choice> Choices { get; set; }

        [Required]
        public int ExamId { get; set; }

        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }
    }
}
