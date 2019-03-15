using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbAdapter
{
    public class Post
    {
        public int Id { get; set; }

        [MaxLength(300)]
        public string Text { get; set; }

        public int Like { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Date { get; private set; }

        [Required]
        public User User { get; set; }

        public ICollection<PostCategory> postCategory { get; set; }
    }
}
