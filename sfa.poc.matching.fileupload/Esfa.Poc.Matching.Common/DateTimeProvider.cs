using System;
using Esfa.Poc.Matching.Common.Interfaces;

namespace Esfa.Poc.Matching.Common
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now() => DateTime.Now;
    }
}