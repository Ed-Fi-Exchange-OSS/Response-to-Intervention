using System;

namespace EdFi.RtI.Core.KeyVault
{
    public class KeyVaultGetException : Exception
    {
        public KeyVaultGetException(string key, Exception innerException = null)
            : base($"Could not get Key Vault secret at \"{key}\"", innerException)
        { }
    }
}
