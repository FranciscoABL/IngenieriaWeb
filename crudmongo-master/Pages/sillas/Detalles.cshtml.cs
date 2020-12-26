using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using MediatR;
using aspnetdemo2.domain.sillas;

namespace aspnetdemo2.Pages.sillas
{
    public class DetallesModel : PageModel
    {

        
        public SillaDetalleModel Detalle {get; set;}
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
           var silla = await  mediator.Send(new LeerSillaPorId(Id));
            if(silla == null){
                return NotFound();
            }
            Detalle = silla;

            return Page();
        }

    }
}
