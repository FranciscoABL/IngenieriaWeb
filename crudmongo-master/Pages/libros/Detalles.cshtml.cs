using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using MediatR;
using aspnetdemo2.domain.libros;

namespace aspnetdemo2.Pages.libros
{
    public class DetallesModel : PageModel
    {

        
        public LibroDetalleModel Detalle {get; set;}
        private readonly ILogger<CrearModel> _logger;
        private IMediator mediator;

        public DetallesModel(ILogger<CrearModel> logger
        ,IMediator media)
        {
            _logger = logger;
            
            mediator = media;
        }

        public async Task<IActionResult> OnGet(string Id)
        {
           var libro = await  mediator.Send(new LeerLibroPorId(Id));
            if(libro == null){
                return NotFound();
            }
            Detalle = libro;

            return Page();
        }

    }
}
