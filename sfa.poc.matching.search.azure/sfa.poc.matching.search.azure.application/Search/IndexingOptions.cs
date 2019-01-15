using System;

namespace sfa.poc.matching.search.azure.application.Search
{
    [Flags]
    public enum IndexingOptions
    {
        Default = 0,
        UseSynonyms = 1,
    }
}
