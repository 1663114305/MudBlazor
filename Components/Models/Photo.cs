using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Components.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string OriginalFileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        public long FileSize { get; set; }

        [MaxLength(100)]
        public string FileType { get; set; } = string.Empty;

        public DateTime UploadTime { get; set; } = DateTime.Now;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsPublic { get; set; } = true;
    }
}