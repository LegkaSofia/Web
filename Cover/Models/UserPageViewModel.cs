using Cover.Domain.Models;
using System.Collections.Generic;

namespace Cover.Models
{
    public class UserPageViewModel
    {
        public User User { get; set; }

        public IEnumerable<Post> Posts { get; set; }

        public int Path { get; set; }
    }
}
