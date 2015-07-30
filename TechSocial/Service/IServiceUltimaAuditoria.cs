using System;
using System.Threading.Tasks;

namespace TechSocial
{
    public interface IServiceUltimaAuditoria
    {
        Task<JsonObjectUltimaAuditoria> GetUltimaAuditoria(string fornecedorId);
    }
}

