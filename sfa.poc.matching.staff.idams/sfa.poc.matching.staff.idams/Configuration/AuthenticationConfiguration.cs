
using System.Diagnostics;

namespace sfa.poc.matching.staff.idams.Configuration
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ", nq}")]
    public class AuthenticationConfiguration
    {
        public string WtRealm { get; set; }
        public string MetaDataEndpoint { get; set; }
        private string DebuggerDisplay => $"AuthenticationConfiguration: { WtRealm }";

    }
}
