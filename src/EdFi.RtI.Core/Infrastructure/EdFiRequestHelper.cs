namespace EdFi.RtI.Core.Infrastructure
{
    public static class EdFiRequestHelper
    {
        public static readonly int DefaultRecordCount = 50;
        public static readonly int MinRecordCount = 1;
        public static readonly int MaxRecordCount = 250;

        public static readonly int MaxRequestAttempts = 10;

        public static int GetOffset(int pageNumber, int pageSize) => (pageNumber - 1) * pageSize;

        public static int GetLimit(int pageSize) => pageSize;
    }
}
