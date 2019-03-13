using System.ComponentModel.DataAnnotations;

namespace DbAdapter
{
    public class User
    {
        
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}
