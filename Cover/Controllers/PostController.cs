using Cover.Domain.Models;
using Cover.Domain.Services;
using Cover.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cover.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class PostController : Controller
    {
        private readonly PostService _postService;
        private readonly UserService _userService;

        public PostController(PostService postService, UserService userService)
        {
            _postService = postService;
            _userService = userService;
        }



        [HttpGet]
        [Route("/Add")]
        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        [Route("/Add")]
        public async Task<IActionResult> AddPost(CreatePostViewModel model)
        {
            var u = _userService.GetByLogin(User.Identity.Name);
            var post = new Post()
            {
                Body = model.Body,
                Category = model.Category,
                Comments = null,
                CreatedDate = DateTime.Now,
                Likes = null,
                OwnerUserName = User.Identity.Name,
                CreateBy = $"{u.Name} {u.Surname}",
            };
            await _postService.CreateAsync(post);
            return RedirectToAction("News", "Home");
        }

        [HttpGet]
        public async Task<PartialViewResult> Comment(string postId)
        {
            var post = await _postService.GetAsync(postId);
            var comments = post.Comments;
            return PartialView(comments);
        }



        [HttpGet]
        [Route("Details/{postId}")]
        public async Task<IActionResult> Details(string postId)
        {
            var model = new PostViewModel()
            {
                Post = await _postService.GetAsync(postId),
                User = await _userService.GetByLoginAsync(User.Identity.Name)
            };
            return View(model);
        }
    }
}
