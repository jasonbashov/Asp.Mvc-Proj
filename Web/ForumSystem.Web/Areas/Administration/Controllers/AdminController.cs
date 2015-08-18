namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Areas.Administration.Models.Admin;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    
    using Microsoft.AspNet.Identity;

    [Authorize]
    public class AdminController : Controller
    {
        private readonly IDeletableEntityRepository<Post> posts;

        public AdminController(IDeletableEntityRepository<Post> posts)
        {
            this.posts = posts;
        }

        // GET: Administration/Admin
        public ActionResult Index()
        {
            return this.View();
        }

        // Kendo UI's grid gets data from here
        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var posts = this.posts
                            .All()
                            .Project().To<PostViewModel>()
                            .ToDataSourceResult(request);
            return this.Json(posts);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, PostViewModel model)
        {
            var post = this.posts
                            .GetById(model.Id);
            this.posts.Delete(post);
            this.posts.SaveChanges();
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, PostViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var postDbModel = this.posts.GetById(model.Id);
                postDbModel.Title = model.Title;
                postDbModel.Content = model.Content;
                this.posts.SaveChanges();
            }

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, PostViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var postDbModel = new Post();
                postDbModel.Title = model.Title;
                postDbModel.Content = model.Content;
                postDbModel.AuthorId = this.User.Identity.GetUserId();
                this.posts.Add(postDbModel);
                this.posts.SaveChanges();
            }

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }
    }
}