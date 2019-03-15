using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbAdapter
{
    public enum CategoryType
    {
        funny,
        art,
        food,
        travel,
        adult
    }
    public class Category
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public CategoryType Type { get; set; }

        public ICollection<PostCategory> PostCategory { get; set; }
    }
}
