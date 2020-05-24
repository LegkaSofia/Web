using Cover.Domain.Models;
using Cover.Domain.Services;
using Cover.Domain.Utilites;
using Cover.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cover.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private readonly PostService _postService;
        private readonly LikeSevice _likeSevice;
        private readonly UserService _userService;

        public HomeController(PostService postService, UserService userService)
        {
            _postService = postService;
            _likeSevice = new LikeSevice(_postService);
            _userService = userService;
        }

        [HttpGet]
        [Route("/")]
        [Route("/News")]
        public async Task<IActionResult> News()
        {
            var user = await _userService.GetByLoginAsync(User.Identity.Name);
            var posts = await _postService.GetAsync();
            if (user.Friends == null)
            {
                user.Friends = new List<Friend>();
            }
            var names = user.Friends.Select(u => u.FriendLogin);
            var friendsPosts = posts.Where(p => names.Contains(p.OwnerUserName) || p.OwnerUserName == user.Login);
            return View(friendsPosts);
        }

        [HttpGet]
        [Route("/MyPage")]
        public async Task<IActionResult> MyPage()
        {
            var model = new UserPageViewModel()
            {
                User = await _userService.GetByLoginAsync(User.Identity.Name)

            };
            model.Posts = (await _postService.GetAsync());
            return View(model);
        }

        [HttpPost]
        [Route("/LikePressed/{postId}")]
        public async Task<bool> OnLikePressed(string postId)
        {
            var post = await _postService.GetAsync(postId);
            var newLike = new Like()
            {
                Liked = DateTime.Now,
                UserLogin = User.Identity.Name,
            };
            var result = await _likeSevice.AddLikeAsync(post, newLike);
            return result;
        }

        [HttpGet]
        [Route("/Send")]
        public async Task<PartialViewResult> Send(string body, string userName, string userSurname, string userLogin, string postId)
        {
            var post = await _postService.GetAsync(postId);
            var comment = new Comment()
            {
                Body = body,
                UserLogin = userLogin,
                UserName = userName,
                UserSurname = userSurname
            };
            if (post.Comments == null)
            {
                post.Comments = new List<Comment>();
            }
            post.Comments.Add(comment);
            await _postService.UpdateAsync(post);
            return PartialView("Post/Comment", comment);
        }
    }
}
