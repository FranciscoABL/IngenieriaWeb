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
using Microsoft.AspNetCore.Authorization;

namespace aspnetdemo2.Pages.leches
{
    [Authorize(Roles = "Admin")]
    public class BorrarModel : PageModel
    {

        
        [BindProperty]
        public LecheABorrar Detalle {get; set;}
        private readonly ILogger<BorrarModel> _logger;
        private readonly IMediator mediator;

        public BorrarModel(ILogger<BorrarModel> logger,
         IMediator mediat)
        {
            _logger = logger;
            mediator = mediat;
            
        }

        public async Task<IActionResult> OnGet(string id)
        {
           var leche = await mediator.Send(new LeerLechePorId(id));
          
            if(leche == null){
                return NotFound();
            }
            Detalle = new LecheABorrar() {
                Id = leche.Id,
                Nombre = leche.Nombre,
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(BorrarLecheCommand cmd){

            if(!ModelState.IsValid){
                Detalle = new LecheABorrar() {
                Id = cmd.Id,
               
                };
                return Page();
            }

            var leche = await mediator.Send(new LeerLechePorId(cmd.Id));
          
            if(leche == null){
                return NotFound();
            }
        
            var res = await mediator.Send(cmd);

            return RedirectToPage("./Index");
        }

        public class LecheABorrar {
            public string Id { get; set; }  
             public string Nombre { get; set; }
            
            
        }
    }
}
