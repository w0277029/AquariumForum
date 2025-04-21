using AquariumForum.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AquariumForum.Models
{
    public class Discussion
    {
        // Primary Key
        public int DiscussionId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string ImageFilename { get; set; } = string.Empty;

        // Property for file upload, not mapped in EF
        [NotMapped]
        [Display(Name = "Photo")]
        public IFormFile? ImageFile { get; set; } // Nullable

        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Navigation Property
        public List<Comment> Comments { get; set; } = new(); // Non-nullable with default value (source: ChatGPT)

        // Foreign key (AspNetUsers table)
        public string ApplicationUserId { get; set; } = string.Empty;

        // Navigation property
        public ApplicationUser? ApplicationUser { get; set; } // nullable!!!
    }
}
