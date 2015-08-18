namespace ForumSystem.Web.ViewModels.PageableFeedbackList
{
    using System;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Mapping;
    
    public class FeedbackViewModel : IMapFrom<Feedback>, IHaveCustomMappings
    {
        public string Author { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Feedback, FeedbackViewModel>()
                        .ForMember(
                        t => t.Author,
                            o => o.MapFrom(b => b.Author.UserName));
        }
    }
}