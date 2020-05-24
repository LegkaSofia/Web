using Cover.Domain.Models;
using Cover.Domain.Services;
using Cover.Models;
using Cover.Neo4J.Domain.Core.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cover.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class FriendController : Controller
    {
        private readonly UserService _userService;
        private readonly PostService _postService;
        private readonly UserRepository _userRepository;

        public FriendController(UserService userService, PostService postService, UserRepository userRepository)
        {
            _userService = userService;
            _postService = postService;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Friends()
        {
            var friends = (await _userService.GetByLoginAsync(User.Identity.Name)).Friends;

            return View(friends);
        }

        [HttpGet]
        [Route("/Profile/{Login}")]
        public async Task<IActionResult> Profile(string login)
        {
            if (User.Identity.Name == login)
            {
                return RedirectToAction("MyPage", "Home");
            }

            var user = await _userService.GetByLoginAsync(login);
            var currentUser = await _userService.GetByLoginAsync(User.Identity.Name);

            var model = new UserPageViewModel
            {
                User = user,
                Posts = (await _postService.GetAsync()).Where(p => p.OwnerUserName == user.Login),
                Path = _userRepository.GetPathLength(currentUser, user)
            };

            return View(model);
        }

        [HttpGet]
        [Route("/AddFriend")]
        public async Task<EmptyResult> AddFriend(string Id)
        {
            var user = await _userService.GetByLoginAsync(User.Identity.Name);
            var friendUser = await _userService.GetAsync(Id);
            var friend = new Friend()
            {
                friendId = Id,
                FriendLogin = friendUser.Login,
                FriendName = friendUser.Name,
                FriendSurname = friendUser.Surname
            };
            if (user.Friends == null)
            {
                user.Friends = new List<Friend>();
            }

            ((List<Friend>)user.Friends).Add(friend);

            await _userService.UpdateAsync(user);

            _userRepository.CreateFriendsRelations(user, friendUser);
            return new EmptyResult();
        }
    }
}