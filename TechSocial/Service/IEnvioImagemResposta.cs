using System;
using System.Threading.Tasks;

namespace TechSocial
{
	public interface IEnvioImagemResposta
	{
		Task<bool> Enviar(string imageArray, string auditoria, string questao);

		Task<bool> EnviarAssinatura(string imageArray, string audi);
	}
}

