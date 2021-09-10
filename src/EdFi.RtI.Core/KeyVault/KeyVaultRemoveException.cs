using System;

namespace EdFi.RtI.Core.KeyVault
{
    public class KeyVaultRemoveException : Exception
    {
        public KeyVaultRemoveException(string key, Exception ex = null)
            : base($"Coult not delete Key Vault secret at \"{key}\"", ex)
        { }
    }
}
