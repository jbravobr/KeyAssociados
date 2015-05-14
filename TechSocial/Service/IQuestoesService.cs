using System;
using System.Threading.Tasks;

namespace TechSocial
{
    public interface IQuestoesService
    {
        Task<JsonObjectQuestoes> RetornarQuestoes(int modulo);
    }
}

