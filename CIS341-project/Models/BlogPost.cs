﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CIS341_project.Models
{
    public class BlogPost 
    {
        [BindNever]
        [Key]
        public int BlogPostId { get; set; }

        [Display(Name = "Author")]
        public string PostAuthor { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Post Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Publication Date")]
        [DataType(DataType.Date)]
        public DateTime DatePublished { get; set; }

        public virtual ICollection<PostReaction> PostReactions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }

}
