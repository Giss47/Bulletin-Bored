using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbAdapter
{
    public class User
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; private set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        public bool Administrator { get; set; }

        public ICollection<Post> Posts { get; set; }

        //Moving validation to data layer, a partialy implementation of DDD patter
        public void SetUserName(string name)
        {
            if (name.Contains("@"))
            {
                throw new InvalidOperationException("Name can't contain @");
            }

            UserName = name;
        }
    }
}
