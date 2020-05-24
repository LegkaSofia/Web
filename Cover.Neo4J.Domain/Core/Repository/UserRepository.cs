using Cover.Domain.Models;
using Cover.Neo4J.Domain.Core.Entities;
using Cover.Neo4J.Domain.Core.Settings.Factory;
using Neo4jClient;
using System.Linq;

namespace Cover.Neo4J.Domain.Core.Repository
{
    public class UserRepository
    {
        private readonly GraphClient _client;

        public UserRepository(CoverGraphDatabaseClientFactory factory)
        {
            _client = factory.GetClient();
            _client.Connect();
            var count = _client.Cypher
                .Match("(n)")
                .Return<int>("count(n)").Results
                .FirstOrDefault();
            if (count == 0)
            {
                _client.Cypher
                    .CreateUniqueConstraint("n:User", "n.login");
            }
        }

        #region Create
        public void Create(User user)
        {
            UserDto u = new UserDto()
            {
                FullName = $"{user.Name} {user.Surname}",
                Login = user.Login
            };

            _client.Cypher
                .Create("(u:User {user})")
                .WithParam("user", u)
                .ExecuteWithoutResults();
        }

        public void CreateFriendsRelations(User user, User friend)
        {
            var userInDb = Get(user.Login);
            if (userInDb == null)
            {
                Create(user);
            }

            if (Get(friend.Login) == null)
            {
                Create(friend);
            }

            _client.Cypher
                .Match("(u:User {login: {userLogin}}), (f:User {login: {friendLogin}})")
                .WithParam("userLogin", user.Login)
                .WithParam("friendLogin", friend.Login)
                .Create("(u)-[r:Friended_With]->(f)")
                .ExecuteWithoutResults();
        }
        #endregion
        #region Read
        public UserDto Get(string login)
        {
            return _client.Cypher
                .Match("(u:User {login: {userLogin}})")
                .WithParam("userLogin", login)
                .Return(u => u.As<UserDto>()).Results.FirstOrDefault();
        }

        public UserDto GetFriendByLogin(User user, string login)
        {
            return _client.Cypher
                .Match("(u:User {login: {userLogin}}),(f:User {login: {friendLogin}}), (u)-[Friended_With]->(f)")
                .WithParam("userLogin", user.Login)
                .WithParam("friendLogin", login)
                .Return<UserDto>("f").Results.FirstOrDefault();
        }

        public int GetPathLength(User user, User destination)
        {
            var u1 = Get(user.Login);
            var u2 = Get(destination.Login);
            bool isNull = false;
            if (u1 == null)
            {
                isNull = true;
                Create(user);
            }
            if (u2 == null)
            {
                isNull = true;
                Create(destination);
            }
            if (!isNull)
            {
                return _client.Cypher
                .Match("(u:User {login: {userLogin}}), (d:User {login: {destLogin}}), p=shortestPath((u)-[*..15]-(d))")
                .WithParam("userLogin", user.Login)
                .WithParam("destLogin", destination.Login)
                .Return<int>("length(p)").Results.FirstOrDefault();
            }
            return default;
        }
        #endregion

        #region Helper Methods
        private UserDto AddAndGet(User user)
        {
            Create(user);
            return Get(user.Login);
        }
        #endregion
    }
}