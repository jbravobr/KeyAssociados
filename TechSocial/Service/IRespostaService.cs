using System.Threading.Tasks;
using System.Collections.Generic;

namespace TechSocial
{
	public interface IRespostaService
	{
		Task<Answers> RetornarRespostasParaAuditoria(int auditoriaId);

		Task<bool> EnviarResposta(ICollection<Respostas> resp);
	}
}

