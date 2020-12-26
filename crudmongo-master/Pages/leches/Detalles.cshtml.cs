using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using MediatR;
using aspnetdemo2.domain.leches;

namespace aspnetdemo2.Pages.leches
{
    public class DetallesModels : PageModel
    {

        
        public LecheDetalleModel Detalle {get; set;}
        private readonly ILogger<CrearModel> _logger;
        private IMediator mediator;

        public DetallesModels(ILogger<CrearModel> logger
        ,IMediator media)
        {
            _logger = logger;
            
            mediator = media;
        }

        public async Task<IActionResult> OnGet(string id)
        {
           var leche = await  mediator.Send(new LeerLechePorId(id));
            if(leche == null){
                return NotFound();
            }
            Detalle = leche;

            return Page();
        }

    }
}
