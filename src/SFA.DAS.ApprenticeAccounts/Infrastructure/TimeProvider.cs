using System;

namespace SFA.DAS.ApprenticeCommitments.Infrastructure
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
    }

    public class UtcTimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }

    public class SpecifiedTimeProvider : ITimeProvider
    {
        public SpecifiedTimeProvider(DateTime time) => Now = time;

        public DateTime Now { get; set; }

        public void Advance(TimeSpan timeSpan) => Now = Now.Add(timeSpan);

        public override string ToString() => Now.ToString();
    }
}