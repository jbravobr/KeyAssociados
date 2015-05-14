using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechSocial
{
    public interface IAuditoriaService
    {
        Task<JsonObject> RetornarAuditorias(string fornecedor);
    }
}

