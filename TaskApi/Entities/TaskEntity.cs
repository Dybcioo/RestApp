using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskApi.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? Deadline { get; set; }
    }
}
