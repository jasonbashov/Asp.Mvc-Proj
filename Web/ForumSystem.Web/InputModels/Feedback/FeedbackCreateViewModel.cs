namespace ForumSystem.Web.InputModels.Feedback
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Mapping;

    public class FeedbackCreateViewModel : IMapFrom<Feedback>
    {
        [Required]
        [MaxLength(20)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Content")]
        [DataType("tinymce_full")]
        [UIHint("tinymce_full")]
        public string Content { get; set; }
    }
}