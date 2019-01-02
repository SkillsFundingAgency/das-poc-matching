
using System;
using System.Diagnostics;

namespace sfa.poc.matching.search.azure.application.Entities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ", nq}")]
    public class Course
    {
        public long Id { get; set; }

       public string Name { get; set; }

        public string Description { get; set; }

        private string DebuggerDisplay => $"Course: {Id} {Name}";
    }
}
