using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using WebAppSimplifica2.Helpers;
using WebAppSimplifica2.Models;

namespace WebAppSimplifica2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Poligono : ControllerBase
    {

        /// <summary>
        /// Obter as Coordenadas do Polígono
        /// </summary>
        /// <param name="processoId">Id do Processo</param>
        /// <returns>Retorno das Coordenadas e ordem do Polígono</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="404">Recurso não encontrado</response>
        /// <response code="500">Erro genérico do servidor</response>
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [Route("GetCoordenadas/{processoId}")]
        public IActionResult GetCoordenadas(int processoId)
        {
            Status status = null;
            List<SqlParameter> lsql = new List<SqlParameter>
            {
                new SqlParameter("@PROID", processoId)
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(new Status("FALHA", "cONFIGURAR MENSAGEM DE RETORNO", ""));
            }

            string json = ConnectionHelper.ExecProcToJson($"SM_GetPoligonoByProId", lsql);

            if (!string.IsNullOrWhiteSpace(json) && !json.Equals("[]"))
            {
                try
                {
                    status = new Status("OK", "", JsonConvert.DeserializeObject<List<PoligonoDAO>>(json));
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Status("FALHA", "CONFIGURAR MENSAGEM DE RETORNO ->" + ex.Message, ""));
                }
            }
            else
            {
                return NotFound(new Status("NÃO LOCALIZADO", "CONFIGURAR MENSAGEM DE RETORNO", ""));
            }

            return Ok(status);
        }

    }
}
