namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.ViewModels.PageableFeedbackList;

    [Authorize]
    public class PageableFeedbackListController : Controller
    {
        private const int DefaultPageSize = 4;

        private readonly IDeletableEntityRepository<Feedback> feedback;

        public PageableFeedbackListController(IDeletableEntityRepository<Feedback> feedback)
        {
            this.feedback = feedback;
        }

        // GET: PageableFeedbackList
        //[OutputCache(Duration = 60 * 60)]
        public ActionResult Index(int? page)
        {
            if (page == null)
            {
                page = 1;
            }

            if (page <= 0)
            {
                page = 1;
            }

            int currentPage = (int)page;

            // The content has already been sanitized in Feedback controller
            var model = this.feedback
                        .All()
                        .OrderByDescending(f => f.CreatedOn)
                        .Skip((currentPage - 1) * DefaultPageSize)
                        .Take(DefaultPageSize)
                        .Project().To<FeedbackViewModel>();

            // Paging
            int allFeedbackCount = this.feedback.All().Count();
            int allPagesCount = (int)Math.Ceiling((double)allFeedbackCount / DefaultPageSize);

            ViewBag.AllPagesCount = allPagesCount;
            ViewBag.ShowPrevious = currentPage != 1;
            ViewBag.ShowNext = currentPage != allPagesCount;
            ViewBag.CurrentPage = currentPage;
            return this.View(model);
        }
    }
}