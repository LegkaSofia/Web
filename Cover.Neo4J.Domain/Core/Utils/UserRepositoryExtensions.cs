using Cover.Domain.Models;
using Cover.Neo4J.Domain.Core.Repository;

namespace Cover.Neo4J.Domain.Core.Utils
{
    public static class UserRepositoryExtensions
    {
        public static UserRepository Add(this UserRepository self, User user)
        {
            self.Create(user);
            return self;
        }
    }
}
