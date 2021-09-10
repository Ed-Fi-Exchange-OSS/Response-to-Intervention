using System;

namespace EdFi.RtI.Core.KeyVault
{
    public class KeyVaultSetException : Exception
    {
        public KeyVaultSetException(string key, Exception innerException = null)
            : base($"Could not store the Key Vault secret at \"{key}\"", innerException)
        { }
    }
}
