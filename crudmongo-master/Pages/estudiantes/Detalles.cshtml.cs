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
    public class DetallesModel : PageModel
    {

        
        public EstudianteDetalleModel Detalle {get; set;}
        private readonly ILogger<CrearModel> _logger;
        private IMediator mediator;

        public DetallesModel(ILogger<CrearModel> logger
        ,IMediator media)
        {
            _logger = logger;
            
            mediator = media;
        }

        public async Task<IActionResult> OnGet(string nc)
        {
           var estudiante = await  mediator.Send(new LeerEstudiantePorNumeroDeControl(nc));
            if(estudiante == null){
                return NotFound();
            }
            Detalle = estudiante;

            return Page();
        }

    }
}
