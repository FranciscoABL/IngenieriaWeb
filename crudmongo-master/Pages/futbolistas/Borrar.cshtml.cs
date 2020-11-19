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
using Microsoft.AspNetCore.Authorization;
namespace aspnetdemo2.Pages.futbolistas
{
    [Authorize(Roles = "Admin")]
    public class BorrarModel : PageModel
    {

        
        [BindProperty]
        public FutbolistaABorrar Detalle {get; set;}
        private readonly ILogger<BorrarModel> _logger;
        private readonly IMediator mediator;

        public BorrarModel(ILogger<BorrarModel> logger,
         IMediator mediat)
        {
            _logger = logger;
            mediator = mediat;
            
        }

        public async Task<IActionResult> OnGet(string Id)
        {
           var futbolista = await mediator.Send(new LeerFutbolistaPorId(Id));
          
            if(futbolista == null){
                return NotFound();
            }
            Detalle = new FutbolistaABorrar() {
                Id = futbolista.Id,
                Nombre = futbolista.Nombre
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(BorrarFutbolistaCommand cmd){

            if(!ModelState.IsValid){
                Detalle = new FutbolistaABorrar() {
                Id = cmd.Id,
               
                };
                return Page();
            }

            var futbolista = await mediator.Send(new LeerFutbolistaPorId(cmd.Id));
          
            if(futbolista == null){
                return NotFound();
            }
            
            var res = await mediator.Send(cmd);

            return RedirectToPage("./Index");
        }

        public class FutbolistaABorrar {
            public string Id { get; set; }  
            public string Nombre { get; set; }
            
            
        }
    }

}

