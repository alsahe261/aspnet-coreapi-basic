using AutoMapper;
using ConceptosService2.Dto;
using ConceptosService2.Persistence;
using ConceptosService2.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            int id = await InsertarConcepto(concepto);
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

        private async Task<int> InsertarConcepto(ConceptoDto concepto)
        {
            var resultParameter = new SqlParameter("@conNumero", SqlDbType.SmallInt) 
            {
                Direction = ParameterDirection.Output
            };

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@conCodigoViejo", string.Empty),
                new SqlParameter("@conCodigo", concepto.Codigo),
                new SqlParameter("@conTipo", concepto.Tipo),
                new SqlParameter("@conNombre", concepto.Descripcion),
                new SqlParameter("@conParametro", "0"),
                new SqlParameter("@conInactivo", "0"),
                new SqlParameter("@conOrden", "0"),
                new SqlParameter("@conAbreviado", string.Empty),
                new SqlParameter("@conMascara", string.Empty),
                new SqlParameter("@conObservacion", string.Empty),
                resultParameter
            };

            await _dbContext.Database.ExecuteSqlCommandAsync($@"exec prctblConceptosGuardar_V2 @conCodigoViejo, @conCodigo, @conTipo, @conNombre, @conParametro, @conInactivo, @conOrden, @conAbreviado, @conMascara, @conObservacion, @conNumero OUTPUT", 
                                                            parameters);
                                
            return int.Parse(resultParameter.Value.ToString());
        }

        #endregion


    }
}