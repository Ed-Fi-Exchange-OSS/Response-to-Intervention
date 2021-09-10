namespace EdFi.RtI.Core.Infrastructure
{
    public class AppSettings
    {
        public string StartupMode { get; set; }

        public bool IsStartupModeHosted => StartupMode == "Hosted";
        public bool IsStartupModeStandalone => StartupMode == "Standalone";
    }
}
