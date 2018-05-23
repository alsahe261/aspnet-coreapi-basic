using ConceptosService2.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConceptosService2.BusinessLogic.Conceptos
{
    public interface IConceptosBL
    {
        Task<List<ConceptoDto>> GetAll(string type);
        
        Task<int> AddConcepto(ConceptoDto concepto);

        Task<bool> UpdateConcepto(ConceptoDto concepto);
    }
}