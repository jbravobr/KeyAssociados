using System.Threading.Tasks;

namespace TechSocial
{
    public interface IRespostaService
    {
        Task<Answers> RetornarRespostasParaAuditoria(int auditoriaId);
    }
}

