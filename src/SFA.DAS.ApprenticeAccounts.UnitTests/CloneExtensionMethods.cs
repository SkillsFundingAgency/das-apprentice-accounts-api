using Newtonsoft.Json;

namespace SFA.DAS.ApprenticeAccounts.UnitTests
{
    public static class CloneExtensionMethods
    {
        public static T Clone<T>(this T self)
        {
            var serialized = JsonConvert.SerializeObject(self);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}