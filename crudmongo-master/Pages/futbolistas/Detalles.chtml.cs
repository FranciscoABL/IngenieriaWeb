using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using MediatR;
using aspnetdemo2.domain.futbolistas;

namespace aspnetdemo2.Pages.futbolistas
{
    public class DetallesModel : PageModel
    {

        
        public FutbolistaDetalleModel Detalle {get; set;}
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
           var futbolista = await  mediator.Send(new LeerFutbolistaPorId(Id));
            if(futbolista == null){
                return NotFound();
            }
            Detalle = futbolista;

            return Page();
        }

    }
}
