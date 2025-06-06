﻿using AquariumForum.Data;

namespace AquariumForum.Models
{
    public class Comment
    {
        // Primary Key
        public int CommentId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Foreign Key
        public int DiscussionId { get; set; }

        // Navigation Property
        public Discussion? Discussion { get; set; } // Nullable

        // Foreign key (AspNetUsers table)
        public string ApplicationUserId { get; set; } = string.Empty;

        // Navigation property
        public ApplicationUser? ApplicationUser { get; set; } // nullable!!!
    }
}
