namespace ForumSystem.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using System.Linq;
    
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Mapping;

    public class IndexBlogPostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public int VoteValues { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Post, IndexBlogPostViewModel>()
                        .ForMember(
                        t => t.VoteValues,
                            o => o.MapFrom(b => b.Votes.Count() == 0 ? 0 : b.Votes.Select(v => v.VoteValue).Sum()));
        }
    }
}