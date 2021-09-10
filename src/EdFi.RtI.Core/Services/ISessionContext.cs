using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services
{
    public interface ISessionContext
    {
        string Email { get; }
        string TenantId { get; }
    }

    public class SessionContext : ISessionContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string TenantId
        {
            get {
                try
                {
                    var context = _httpContextAccessor.HttpContext;
                    var tenantClaim = context.User.FindFirst(claim => claim.Type.Contains("tenant"));
                    return tenantClaim.Value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public string Email
        {
            get {
                try
                {
                    var context = _httpContextAccessor.HttpContext;
                    var emailClaim = context.User.FindFirst(claim => claim.Type.Contains("email"));
                    return emailClaim.Value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
