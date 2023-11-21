using System.Reflection;

namespace Web.Controllers.Helpers
{
    internal static class Mapper
    {
        public static T? MappIt<T>(object source)
        {
            var output = Activator.CreateInstance(typeof(T));

            if (output == null) return default;

            var sourceType = source.GetType();

            foreach (var outputProperty in typeof(T).GetProperties())
            {
                if (sourceType.GetProperty(outputProperty.Name) is PropertyInfo sourceProperty)
                {
                    var value = sourceProperty.GetValue(source);
                    outputProperty.SetValue(output, value);
                }
            }

            return (T)output;
        }
    }
}
