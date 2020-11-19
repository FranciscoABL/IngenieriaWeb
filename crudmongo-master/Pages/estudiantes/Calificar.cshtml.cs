using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using MediatR;
using aspnetdemo2.domain.estudiantes;

namespace aspnetdemo2.Pages.estudiantes
{
    public class CalificarModel : PageModel
    {

        
        [BindProperty]
        public EstudianteACalificar Detalle {get; set;}
        private readonly ILogger<EditarModel> _logger;
        private readonly IMediator mediator;

        //private IEstudiantesRespository repo;

        public CalificarModel(ILogger<EditarModel> logger,
         IMediator mediat)
        {
            _logger = logger;
            
            mediator = mediat;
        }

        public async Task<IActionResult> OnGet(string nc)
        {
            var estudiante = await mediator.Send(new LeerEstudiantePorNumeroDeControl(nc));
          
            if(estudiante == null){
                return NotFound();
            }
            Detalle = new EstudianteACalificar() {
                NumeroControl = estudiante.NumeroControl,
                Nombre = estudiante.Nombre,
                Carrera = estudiante.Carrera,
                Promedio = estudiante.Promedio
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(CalificarEstudianteCommand cmd){

            var estudiante = await mediator.Send(new LeerEstudiantePorNumeroDeControl(cmd.NumeroControl));
          
            if(estudiante == null){
                return NotFound();
            }

             if(!ModelState.IsValid){
                
                Detalle = new EstudianteACalificar() {
                NumeroControl = cmd.NumeroControl,
                Nombre = estudiante.Nombre,
                Carrera = estudiante.Carrera,
                Promedio = cmd.Promedio
                };
                return Page();
            }
            
            var res = await mediator.Send(cmd);

            return RedirectToPage("./Index");
        }

        public class EstudianteACalificar {
            public string NumeroControl { get; set; }  
             public string Nombre { get; set; }
            public string Carrera { get; set; }
            public float Promedio { get; set; }
            
        }
    }
}
