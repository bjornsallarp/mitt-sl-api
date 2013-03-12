namespace MySlApi.Core.Tests.Helpers
{
    using System;
    using System.IO;
    using System.Reflection;

    public static class ResourceFileHelper
    {
        public static string GetHtmlFile(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("MySlApi.Core.Tests.Html." + name))
            {
                if (stream == null)
                {
                    throw new Exception(string.Format("Resource '{0}' could not be read!", name));
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }                
            }
        }
    }
}
