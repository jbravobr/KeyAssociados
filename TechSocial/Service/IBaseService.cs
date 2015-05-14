using System;
using System.Threading.Tasks;

namespace TechSocial
{
    public interface IBaseService
    {
        Task<JsonObject> RetornarBasesLegais();
    }
}

