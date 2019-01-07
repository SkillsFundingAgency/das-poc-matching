using System;
using System.Diagnostics;

namespace sfa.poc.matching.search.azure.application.Entities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ", nq}")]
    public class IndexedCourse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        private string DebuggerDisplay => $"Course: {Id} {Name}";

        public static IndexedCourse FromCourse(Course course)
        {
            return new IndexedCourse
            {
                Id = course.Id.ToString(),
                Name = course.Name,
                Description = course.Description,
            };
        }

        public Course ToCourse()
        {
            return new Course
            {
                Id = Convert.ToInt64(Id),
                Name = Name,
                Description = Description
            };
        }
    }
}
