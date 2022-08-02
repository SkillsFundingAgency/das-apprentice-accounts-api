
using System.Text.Json;

namespace SFA.DAS.ApprenticeAccounts.UnitTests
{
    public static class CloneExtensionMethods
    {
        public static T Clone<T>(this T self)
        {
            var serialized = JsonSerializer.Serialize(self);
            return JsonSerializer.Deserialize<T>(serialized);
        }
    }
}