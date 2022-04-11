using System;
using System.ComponentModel.DataAnnotations;

namespace TaskApi.DTOs
{
    public class TaskDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime? Deadline { get; set; }
    }
}
