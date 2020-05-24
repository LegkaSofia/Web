using Cover.Domain.Models;
using Cover.Neo4J.Domain.Core.Entities;
using Cover.Neo4J.Domain.Core.Repository;
using Cover.Neo4J.Domain.Core.Settings;
using Cover.Neo4J.Domain.Core.Settings.Factory;
using Cover.Neo4J.Domain.Core.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
namespace Cover.Tests
{
    [TestClass]
    public class Neo4JTests
    {
        private UserRepository _userRepository;

        [TestMethod]
        public void AddUserTest()
        {
            User user = GetUser();
            _userRepository.Add(user);

            var userFromDb = _userRepository.Get(user.Login);

            Assert.IsNotNull(userFromDb);
            Assert.AreEqual(user.Login, userFromDb.Login);
            Assert.AreEqual($"{user.Name} {user.Surname}", userFromDb.FullName);
        }

        [TestMethod]
        public void CreateFriendsRelationsTest()
        {
            var user = GetUser();
            var friend = GetUser("Sofia", "Legka", "sofa");
            AddFriend(user, friend);
            _userRepository.Add(user).Add(friend);

            _userRepository.CreateFriendsRelations(user);

            UserDto friendFromDb = _userRepository.GetFriendByLogin(user, friend.Login);

            Assert.IsNotNull(friendFromDb);
            Assert.AreEqual(friend.Login, friendFromDb.Login);
        }

        [TestMethod]
        public void GetPathLengthTest()
        {
            var user1 = GetUser();
            var user2 = GetUser("Sofa", "Sofa", "sofa");
            _userRepository.Add(user1).Add(user2);
            AddFriend(user1, user2);

            _userRepository.CreateFriendsRelations(user1);
            var length = _userRepository.GetPathLength(user1, user2);

            Assert.AreEqual(1, length);
        }
        #region Helper Methods

        [TestInitialize]
        public void Init()
        {
            CoverGraphDatabaseSettings settings = new CoverGraphDatabaseSettings()
            {
                DatabaseLogin = "neo4j",
                DatabasePassword = "network",
                Url = "http://localhost:7474/db/data"
            };
            CoverGraphDatabaseClientFactory factory = new CoverGraphDatabaseClientFactory(settings);

            _userRepository = new UserRepository(factory);
        }
        private User GetUser(string name = "Taras", string surname = "Sharko", string login = "login")
        {
            return new User()
            {
                Name = name,
                Surname = surname,
                Login = login
            };
        }

        private void AddFriend(User user, User friend)
        {
            if (user.Friends == null)
            {
                user.Friends = new List<Friend>();
            }
            (user.Friends as List<Friend>).Add(
                new Friend
                {
                    FriendLogin = friend.Login,
                    FriendName = friend.Name,
                    FriendSurname = friend.Surname
                });
        }
        #endregion
    }
}
