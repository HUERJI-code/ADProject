using ADProject.Models;
using ADProject.Services;
using Microsoft.EntityFrameworkCore;

namespace ADProject.Repositories
{
    public class TagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Tag> GetAllTags() => _context.Tags.ToList();

        public Tag GetById(int id) => _context.Tags.Find(id);

        public IEnumerable<Tag> GetTagsByUserProfileId(int profileId)
        {
            var profile = _context.UserProfiles
                .Include(p => p.Tags)
                .FirstOrDefault(p => p.Id == profileId);

            return profile?.Tags ?? Enumerable.Empty<Tag>();
        }

        public IEnumerable<Tag> GetTagsByActivityId(int activityId)
        {
            var activity = _context.Activities
                .Include(a => a.Tags)
                .FirstOrDefault(a => a.ActivityId == activityId);

            return activity?.Tags ?? Enumerable.Empty<Tag>();
        }

        public void AddTag(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();                   
        }

        public bool TagExists(string name)
        {
            return _context.Tags.Any(t => t.Name == name);
        }

        public List<int> AddTagIfNotExists(List<string> tagNames)
        {
            foreach (var name in tagNames)
            {
                if (!TagExists(name))
                {
                    var newTag = new Tag { Name = name, Description = "1" };
                    AddTag(newTag);
                }
            }
            var tagIds = new List<int>();
            foreach (var name in tagNames)
            {
                var tag = _context.Tags.FirstOrDefault(t => t.Name == name);
                if (tag != null)
                {
                    tagIds.Add(tag.TagId);
                }
            }
            return tagIds;
        }


    }
}
