using Microsoft.Extensions.Configuration;

namespace ArmLancer.API.Utils.Extensions
{
    public static class ConfigExtensions
    {
        public static T GetSettings<T>(this IConfiguration config)
        {
            var sectionName = typeof(T).Name;
            return config.GetSection(sectionName).Get<T>();
        }
    }
}