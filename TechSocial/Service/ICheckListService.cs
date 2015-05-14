using System;
using System.Threading.Tasks;

namespace TechSocial
{
    public interface ICheckListService
    {
        Task<JsonObjectModulo> RetornaChecklist(string checklist);
    }
}

