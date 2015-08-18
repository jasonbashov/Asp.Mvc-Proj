namespace ForumSystem.Web.Areas.Administration.Models.Admin
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Mapping;

    [Bind(Include = "Id, Title, Content")]
    public class PostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string AuthorName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool IsDeleted { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime CreatedOn { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime? ModifiedOn { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
                        .ForMember(
                        t => t.AuthorName,
                            o => o.MapFrom(b => b.Author.UserName));
        }
    }
}