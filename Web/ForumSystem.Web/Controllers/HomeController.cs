namespace ForumSystem.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.ViewModels.Home;
    using Microsoft.AspNet.Identity;

    public class HomeController : Controller
    {
        private readonly IDeletableEntityRepository<Post> posts;

        private readonly IRepository<Vote> votes;

        public HomeController(IDeletableEntityRepository<Post> posts, IRepository<Vote> votes)
        {
            this.posts = posts;
            this.votes = votes;
        }

        public ActionResult Index()
        {
            var model = this.posts.All().Project().To<IndexBlogPostViewModel>();

            return this.View(model);
        }

        // TODO: Abstract Up- and DownVote
        [Authorize]
        [HttpPost]
        public ActionResult UpVote(int id, int currentVoteValues)
        {
            if (Request.IsAjaxRequest())
            {
                var currentUserId = this.User.Identity.GetUserId();
                var voteInDb = this.votes.All().FirstOrDefault(v => v.PostId == id && v.UserId == currentUserId);
                if (voteInDb == null)
                {
                    voteInDb = new Vote()
                    {
                        PostId = id,
                        UserId = currentUserId
                    };
                    this.votes.Add(voteInDb);
                }

                voteInDb.VoteValue = 1;
                this.votes.SaveChanges();

                return this.Content((currentVoteValues + 1).ToString());
            }

            return this.RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DownVote(int id, int currentVoteValues)
        {
            if (Request.IsAjaxRequest())
            {
                var currentUserId = this.User.Identity.GetUserId();
                var voteInDb = this.votes.All().FirstOrDefault(v => v.PostId == id && v.UserId == currentUserId);
                if (voteInDb == null)
                {
                    voteInDb = new Vote()
                    {
                        PostId = id,
                        UserId = currentUserId
                    };
                    this.votes.Add(voteInDb);
                }

                voteInDb.VoteValue = -1;
                this.votes.SaveChanges();

                return this.Content((currentVoteValues - 1).ToString());
            }

            return this.RedirectToAction("Index");
        }
    }
}