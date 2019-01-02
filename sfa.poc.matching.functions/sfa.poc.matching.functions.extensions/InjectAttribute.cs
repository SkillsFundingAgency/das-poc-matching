using System;
using Microsoft.Azure.WebJobs.Description;

namespace sfa.poc.matching.functions.extensions
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
    }
}
