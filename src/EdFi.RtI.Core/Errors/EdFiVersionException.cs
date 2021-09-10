namespace EdFi.RtI.Core.Errors
{
    public class EdFiVersionException : DomainException
    {
        public EdFiVersionException(string version)
            : base($"Unsupported Ed-Fi version \"{version}\"")
        {
            StatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}
