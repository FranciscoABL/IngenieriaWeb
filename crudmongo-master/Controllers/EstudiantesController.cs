using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetdemo2.domain.estudiantes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aspnetdemo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EstudiantesController : ControllerBase
    {
        private readonly IMediator mediator;

        public EstudiantesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        // GET: api/Estudiantes
        [HttpGet]
        public async Task<ActionResult<List<EstudianteIndexModel>>> Get()
        {
            var modelo = await mediator.Send( new LeerTodosLosEstudiantes());
            return modelo;
        }

        // GET: api/Estudiantes/5
        [HttpGet("{nc}", Name = "Get")]
        public async Task<ActionResult<EstudianteDetalleModel>> Get(string nc)
        {
            var estudiante = await  mediator.Send(new LeerEstudiantePorNumeroDeControl(nc));
            if(estudiante == null){
                return NotFound();
            }
           
            return estudiante;
        }

        // POST: api/Estudiantes
        
        [HttpPut("{nc}")]
        public async Task<ActionResult> Put(string nc,[FromBody] EditarEstudianteCommand cmd){

            if(!ModelState.IsValid){
              
                return BadRequest();
            }

            var estudiante = await mediator.Send(new LeerEstudiantePorNumeroDeControl(cmd.NumeroControl));
          
            if(estudiante == null){
                return NotFound();
            }
            
            var res = await mediator.Send(cmd);

            return Ok();
        }

        [HttpPut("{nc}/Calificar")]
        public async Task<ActionResult> Put(string nc,[FromBody] float promedio){

            if(!ModelState.IsValid){
              
                return BadRequest();
            }

            var estudiante = await mediator.Send(new LeerEstudiantePorNumeroDeControl(nc));
          
            if(estudiante == null){
                return NotFound();
            }
            
            var res = await mediator.Send(new CalificarEstudianteCommand(nc, promedio));

            return Ok();
        }

        // PUT: api/Estudiantes/5
        [HttpPost]
        public async Task<ActionResult> Post( [FromBody] CrearEstudianteCommand cmd ){
            //do nothing

            if(!ModelState.IsValid){
              
                return BadRequest();
            }
            
            var res = await  mediator.Send(cmd);
            //Crear nuevo alumno


            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{nc}")]
        [Authorize(Roles= "Admin")]
        public async Task<ActionResult> Delete(string nc)
        {
           
            var estudiante = await mediator.Send(new LeerEstudiantePorNumeroDeControl(nc));
          
            if(estudiante == null){
                return NotFound();
            }
            
            var res = await mediator.Send(new BorrarEstudianteCommand(nc));

            return Ok();
        }
    }
}
