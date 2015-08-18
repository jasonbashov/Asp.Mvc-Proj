namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure;
    using ForumSystem.Web.InputModels.Feedback;
    using Microsoft.AspNet.Identity;
    
    public class FeedbackController : Controller
    {
        private readonly IDeletableEntityRepository<Feedback> feedback;
        
        private readonly ISanitizer sanitizer;

        public FeedbackController(IDeletableEntityRepository<Feedback> feedback, ISanitizer sanitizer)
        {
            this.feedback = feedback;
            this.sanitizer = sanitizer;
        }

        // GET: Create
        public ActionResult Create()
        {
            var model = new FeedbackCreateViewModel();
            return this.View(model);
        }

        // Post: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(FeedbackCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var feedbackDbModel = Mapper.Map<Feedback>(model);
                
                // Sanitize dangerous content
                feedbackDbModel.Content = this.sanitizer.Sanitize(feedbackDbModel.Content);

                if (this.User.Identity.IsAuthenticated)
                {
                    var currentUserId = this.User.Identity.GetUserId();
                    feedbackDbModel.AuthorId = currentUserId;
                }

                this.feedback.Add(feedbackDbModel);
                this.feedback.SaveChanges();
                this.TempData["SuccessMessage"] = "Thank you for providing feedback!";
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(model);
        }
    }
}