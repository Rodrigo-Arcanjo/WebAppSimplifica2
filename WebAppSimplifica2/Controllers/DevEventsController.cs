using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppSimplifica2.Entities;
using WebAppSimplifica2.Persistence;

namespace WebAppSimplifica2.Controllers
{
    [Route("api/dev-events")]
    [ApiController]

    public class DevEventsController : ControllerBase
    {

        private readonly DevEventsDbContext _context;

        public DevEventsController(DevEventsDbContext context)
        {
            _context = context;
        }

        // api/dev-events       GET
        /// <summary>
        /// Obter Todos os Eventos
        /// </summary>
        /// <returns>Coleção de eventos</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult GetAll()
        {
            var devEvents = _context.DevEvents.Where(d => !d.IsDeleted).ToList();

            if (!ModelState.IsValid) //Utilizar essa condicional para fazer as validações e tratamento de exceções
            {
                return NotFound();
            }

            return Ok(devEvents);
        }

        // api/dev-events/3fa85f64-5717-4562-b3fc-2c963f66afa6       GET
        /// <summary>
        /// Obter um Evento
        /// </summary>
        /// <param name="id">Identificador do Evento</param>
        /// <returns>Dados do Evento</returns>
        /// <response code ="200">Sucesso</response>
        /// <respose code="404">Não encontrado!</respose>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public IActionResult GetById(Guid id)
        {
            var devEvent = _context.DevEvents
                .Include(de => de.Speakers)
                .SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
            {
                return NotFound();
            }

            return Ok(devEvent);
        }

        // api/dev-events/      POST
        /// <summary>
        /// Cadastrar  um Evento
        /// </summary>
        /// <remarks>
        /// {"title":"string", "description":"string", "startDate":"2023-07-20T15:30:23.512Z", "endDate":"2023-07-20T15:30:23.512Z"}
        /// </remarks>
        /// <param name="devEvent">Dados do Evento</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public IActionResult Post(DevEvent devEvent)
        {
            _context.DevEvents.Add(devEvent);
            _context.SaveChanges(); 

            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);
        }

        // api/dev-events/3fa85f64-5717-4562-b3fc-2c963f66afa6       PUT
        /// <summary>
        /// Atualizar um Evento
        /// </summary>
        /// <remarks>
        /// {"title":"string", "description":"string", "startDate":"2023-07-20T15:30:23.512Z", "endDate":"2023-07-20T15:30:23.512Z"}
        /// </remarks>
        /// <param name="id">Identificador do Evento</param>
        /// <param name="input">Dados do Evento</param>
        /// <returns>Sem retorno de objeto</returns>
        /// <response code="404">Não encontrado!</response>
        /// <response code="204">Sucesso</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public IActionResult Update(Guid id, DevEvent input)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
            {
                return NotFound();
            }

            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);

            _context.DevEvents.Update(devEvent);
            _context.SaveChanges();

            return NoContent();
        }

        // api/dev-events/3fa85f64-5717-4562-b3fc-2c963f66afa6       DELETE
        /// <summary>
        /// Deletar um Evento
        /// </summary>
        /// <param name="id">Identificador do Evento</param>
        /// <returns>sem Objeto de retorno</returns>
        /// <response code="404">Não encontrado!</response>
        /// <response code ="204">Sucesso</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
            {
                return NotFound();
            }

            devEvent.Delete();

            _context.SaveChanges();

            return NoContent();
        }

        // api/dev-events/3fa85f64-5717-4562-b3fc-2c963f66afa6/speakers
        /// <summary>
        /// Cadastrar Palestrante
        /// </summary>
        /// <remarks>
        /// {"name":"string", "talkTitle":"string", "talkDescription":"string", "linkedInProfile":"string"}
        /// </remarks>
        /// <param name="id">Identificador do Evento</param>
        /// <param name="speaker">Dados do Palestrante</param>
        /// <returns>Sem objeto de retorno</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Evento não encontrado!</response>
        [HttpPost("{id}/speakers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public IActionResult PostSpeaker(Guid id, DevEventSpeaker speaker)
        {
            speaker.DevEventId = id;

            var devEvent = _context.DevEvents.Any(d => d.Id == id);

            if (!devEvent)
            {
                return NotFound();
            } 

            _context.DevEventsSpeakers.Add(speaker);
            _context.SaveChanges();

            return NoContent();

        }

    }
}
