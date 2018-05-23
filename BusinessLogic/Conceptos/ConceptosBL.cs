using AutoMapper;
using ConceptosService2.Dto;
using ConceptosService2.Persistence;
using ConceptosService2.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConceptosService2.BusinessLogic.Conceptos
{
    public class ConceptosBL : IConceptosBL
    {
        private readonly SiamContext _dbContext;
        private readonly IMapper _mapper;

        public ConceptosBL(SiamContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        public async Task<int> AddConcepto(ConceptoDto concepto)
        {
            int id = await InsertarConcepto(concepto.Codigo, concepto.Tipo, concepto.Descripcion);
            return id;
        }

        public async Task<List<ConceptoDto>> GetAll(string type)
        {
            List<prcConceptos_Model> conceptos = await CargarConceptos(type);

            return _mapper.Map<List<prcConceptos_Model>, List<ConceptoDto>>(conceptos);
        }


        public async Task<bool> UpdateConcepto(ConceptoDto concepto)
        {
            throw new System.NotImplementedException();
        }


        #region "Data Access Methods"

        private async Task<List<prcConceptos_Model>> CargarConceptos(string type)
        {
            List<prcConceptos_Model> conceptos;

            if (string.IsNullOrEmpty(type))
            {
                conceptos = await _dbContext.Conceptos.FromSql("EXECUTE prcConceptos").ToListAsync();
            }
            else
            {
                conceptos = await _dbContext.Conceptos.FromSql($"EXECUTE prcConceptos @cTipoConcepto={type}").ToListAsync();
            }

            return conceptos;
        }

        private async Task<int> InsertarConcepto(string codigo, string tipo, string nombre)
        {
            int results = 0;
            results = await _dbContext.Database.ExecuteSqlCommandAsync($"exec prctblConceptosGuardar @conCodigoViejo={""}, @conCodigo={codigo}, @conTipo={tipo}, @conNombre={nombre}, @conParametro=0, @conInactivo=0, @conOrden=0, @conAbreviado={""}, @conMascara={""}");
            return results;
        }

        #endregion


    }
}