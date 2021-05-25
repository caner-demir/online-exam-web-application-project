using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineExam.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        [MaxLength(120)]
        [Display(Name = "Description for Homepage")]
        public string DescriptionShort { get; set; }

        [Required]
        [Display(Name = "Description for Course Page")]
        public string DescriptionDetailed { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DateCreated { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<CourseUser> Users { get; set; }
    }
}
