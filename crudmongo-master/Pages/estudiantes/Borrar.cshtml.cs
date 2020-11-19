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
using Microsoft.AspNetCore.Authorization;

namespace aspnetdemo2.Pages.estudiantes
{
    [Authorize(Roles = "Admin")]
    public class BorrarModel : PageModel
    {

        
        [BindProperty]
        public EstudianteABorrar Detalle {get; set;}
        private readonly ILogger<BorrarModel> _logger;
        private readonly IMediator mediator;

        public BorrarModel(ILogger<BorrarModel> logger,
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
            Detalle = new EstudianteABorrar() {
                NumeroControl = estudiante.NumeroControl,
                Nombre = estudiante.Nombre,
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(BorrarEstudianteCommand cmd){

            if(!ModelState.IsValid){
                Detalle = new EstudianteABorrar() {
                NumeroControl = cmd.NumeroControl,
               
                };
                return Page();
            }

            var estudiante = await mediator.Send(new LeerEstudiantePorNumeroDeControl(cmd.NumeroControl));
          
            if(estudiante == null){
                return NotFound();
            }
            
            var res = await mediator.Send(cmd);

            return RedirectToPage("./Index");
        }

        public class EstudianteABorrar {
            public string NumeroControl { get; set; }  
             public string Nombre { get; set; }
            
            
        }
    }
}
