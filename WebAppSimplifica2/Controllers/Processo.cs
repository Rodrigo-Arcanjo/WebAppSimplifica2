using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Imports.Newtonsoft.Json.Linq;
using System;
using System.Net;
using WebAppSimplifica2.Helpers;
using WebAppSimplifica2.Models;

namespace WebAppSimplifica2.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class Processo : ControllerBase
    {

        /// <summary>
        /// Obter Todos os Processos
        /// </summary>
        /// <param name="UserId">ID do usuário</param>
        /// <param name="REQ_RT">Índice de responsável técnico ou requerente</param>
        /// <param name="CurrentItemmount">Qt. Itens</param>
        /// <returns>Listagem dos processos e todos os seus respectivos dados</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="500">Erro genérico do servidor</response>
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [Route("GetProcessos/{UserId}/{REQ_RT}/{CurrentItemmount}")]
        public IActionResult GetProcessos_One(string UserId, int REQ_RT, int CurrentItemmount)
        {
            Status status = null;
            List<SqlParameter> lsql = new List<SqlParameter>
            {
                new SqlParameter("@USER_ID", UserId),
                new SqlParameter("@REQ_RT", REQ_RT),
                new SqlParameter("@CURRENT_AMMOUNT", CurrentItemmount)
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(new Status("FALHA", "CONFIGURAR MENSAGEM DE RETORNO", ""));
            }

            string json = ConnectionHelper.ExecProcToJson($"SM_GetProcessosByUserId", lsql);
            //System.Exception: 'Houve um erro na Consulta: The ConnectionString property has not been initialized.'

            if (!string.IsNullOrWhiteSpace(json) && !json.Equals("[]"))
            {
                try
                {
                    //string jsonDef = JsonConvert.SerializeObject(json);

                    Newtonsoft.Json.Linq.JArray objJson = Newtonsoft.Json.Linq.JArray.Parse(json);



                    status = new Status("OK", "", JsonConvert.DeserializeObject<List<ProcessoOne>>(json));
                }
                catch(Exception ex)
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

        /// <summary>
        /// Obter um Processo apenas (pelo ID)
        /// </summary>
        /// <param name="Pro_Id">ID do Processo</param>
        /// <returns>Objeto do Processo buscado</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="404">Recurso não encontrado</response>
        /// <response code="500">Erro genérico do servidor</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [Route("GetProcessoById/{Pro_Id}")]
        public IActionResult GetProcessoById_Two(string Pro_Id)
        {

            Status status = null;
            List<SqlParameter> lsql = new List<SqlParameter>
            {
                new SqlParameter("@PRO_ID", Pro_Id)
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(new Status("FALHA", "cONFIGURAR MENSAGEM DE RETORNO", ""));
            }

            string json = ConnectionHelper.ExecProcToJson($"GetProcessosByProId", lsql);

            if (!string.IsNullOrWhiteSpace(json) && !json.Equals("[]"))
            {
                try
                {
                    status = new Status("OK", "", JsonConvert.DeserializeObject<List<ProcessoTwo>>(json));
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

        /// <summary>
        /// Obter um Processo apenas (pelo NumProc)
        /// </summary>
        /// <param name="UsuId">ID do usuário</param>
        /// <param name="NumProAmostra">Número do Processo</param>
        /// <param name="YearProAmostra">Ano do Processo</param>
        /// <returns>Objeto com as informações do Processo</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="404">Recurso não encontrado</response>
        /// <response code="500">Erro genérico do servidor</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [Route("GetProcessoByNumProc/{UsuId}/{NumProAmostra}")]
        public IActionResult GetProcessoByNumProc_Three(int UsuId, string NumProAmostra, string YearProAmostra)
        {

            //NumProAmostra = NumProAmostra.Replace("()_", "/");

            Status status = null;

            string numProAmostraEnd = NumProAmostra + "/" + YearProAmostra;

            List<SqlParameter> lsql = new List<SqlParameter>
            {
                new SqlParameter("@PRO_NUM_AMOSTRA", numProAmostraEnd),
                new SqlParameter("@USER_ID", UsuId)
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(new Status("FALHA", "cONFIGURAR MENSAGEM DE RETORNO", ""));
            }

            //string json = ConnectionHelper.ExecProcToJson($"GetProcessosByProNumero", lsql);
            string json = ConnectionHelper.ExecProcToJson($"GetProcessosByProNumeroAmostra", lsql);

            if (!string.IsNullOrWhiteSpace(json) && !json.Equals("[]"))
            {
                try
                {
                    status = new Status("OK", "", JsonConvert.DeserializeObject<List<ProcessoThree>>(json));
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

        /// <summary>
        /// Obter um Processo apenas (independete da forma de busca)
        /// </summary>
        /// <param name="NumProAmostra">Amostra no NumProc</param>
        /// <param name="YearNumProAmostra">Ano no NumProc</param>
        /// <returns>Objeto de um único processo</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="404">Recurso não encontrado</response>
        /// <response code="500">Erro genérico do servidor</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [Route("GetProcessoByIdAndNumProc/{NumProAmostra}")]
        public IActionResult GetProcessoByIdAndNumProc_Four(string NumProAmostra, string YearNumProAmostra)
        {

            //NumProAmostra = NumProAmostra.Replace("()_", "/");

            string NumProAmostraEnd = NumProAmostra + "/" + YearNumProAmostra;
            Status status = null;
            List<SqlParameter> lsql = new List<SqlParameter>
            {
                new SqlParameter("@PRO_NUM_AMOSTRA", NumProAmostraEnd)
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(new Status("FALHA", "cONFIGURAR MENSAGEM DE RETORNO", ""));
            }

            string json = ConnectionHelper.ExecProcToJson($"GetProcessoIdByProNumero", lsql);

            if (!string.IsNullOrWhiteSpace(json) && !json.Equals("[]"))
            {
                try
                {
                    status = new Status("OK", "", JsonConvert.DeserializeObject<List<ProcessoFour>>(json));
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
