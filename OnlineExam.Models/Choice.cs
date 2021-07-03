using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace OnlineExam.Models
{
    public class Choice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ChoiceNumber { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [JsonIgnore]
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

    }
}
