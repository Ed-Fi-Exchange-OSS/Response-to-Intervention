using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Settings
{
    public class EdFiVersionNotSetException : Exception
    {
        public EdFiVersionNotSetException()
            : base("An Ed-Fi version has not been set.")
        {

        }
    }
}
