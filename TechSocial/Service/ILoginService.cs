using System;
using System.Threading.Tasks;

namespace TechSocial
{
    public interface ILoginService
    {
        Task<JsonObject> ExecutarLogin(string user, string pass);
    }
}

