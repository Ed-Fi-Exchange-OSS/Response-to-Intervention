using Microsoft.AspNetCore.Hosting;

namespace EdFi.RtI.Core.Providers.Environment
{
    public interface IEnvironmentProvider
    {
        bool IsEnvironmentLocal { get; }
    }

    public class EnvironmentProvider : IEnvironmentProvider
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public EnvironmentProvider(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public bool IsEnvironmentLocal => _hostingEnvironment.EnvironmentName == "Local";
    }
}
