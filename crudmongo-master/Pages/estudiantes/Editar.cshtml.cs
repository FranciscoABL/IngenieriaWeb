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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

namespace aspnetdemo2.Pages.estudiantes
{
    public class EditarModel : PageModel
    {

        
        [BindProperty]
        public EstudianteAEditar Detalle {get; set;}
        private readonly ILogger<EditarModel> _logger;
        private readonly IMediator mediator;
        private readonly IConfiguration configuracion;

        //private IEstudiantesRespository repo;

        [BindProperty]
        public IFormFile Foto {get; set;}

        public EditarModel(ILogger<EditarModel> logger,
         IMediator mediat,
         IConfiguration config)
        {
            _logger = logger;
            
            mediator = mediat;
            configuracion = config;
        }

        public async Task<IActionResult> OnGet(string nc)
        {
            var estudiante = await mediator.Send(new LeerEstudiantePorNumeroDeControl(nc));
          
            if(estudiante == null){
                return NotFound();
            }
            Detalle = new EstudianteAEditar() {
                NumeroControl = estudiante.NumeroControl,
                Nombre = estudiante.Nombre,
                Carrera = estudiante.Carrera
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(EditarEstudianteCommand cmd){

            if(!ModelState.IsValid){
                Detalle = new EstudianteAEditar() {
                NumeroControl = cmd.NumeroControl,
                Nombre = cmd.Nombre,
                Carrera = cmd.Carrera
                };
                return Page();
            }

            var estudiante = await mediator.Send(new LeerEstudiantePorNumeroDeControl(cmd.NumeroControl));
          
            if(estudiante == null){
                return NotFound();
            }
            
            var res = await mediator.Send(cmd);

           

        
            if(Foto != null ){
             //   var archivo = Path.Combine("wwwroot", "estudiantes", $"{estudiante.NumeroControl}.jpeg" );
                using(var archivo = Foto.OpenReadStream()){
                    var uploadCmd = new ActualizarFotoCommand(cmd.NumeroControl, archivo);
                    var res2 = await mediator.Send(uploadCmd);
                }
            }

            return RedirectToPage("./Index");
        }

        public class EstudianteAEditar {
            public string NumeroControl { get; set; }  
             public string Nombre { get; set; }
            public string Carrera { get; set; }
            
        }
    }
}
