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
using Microsoft.AspNetCore.Authorization;
namespace aspnetdemo2.Pages.sillas
{
    [Authorize(Roles = "Admin")]
    public class BorrarModel : PageModel
    {

        
        [BindProperty]
        public SillaABorrar Detalle {get; set;}
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
           var silla = await mediator.Send(new LeerSillaPorId(Id));
          
            if(silla == null){
                return NotFound();
            }
            Detalle = new SillaABorrar() {
                Id = silla.Id,
                TipoDeMaterial = silla.TipoDeMaterial
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(BorrarSillaCommand cmd){

            if(!ModelState.IsValid){
                Detalle = new SillaABorrar() {
                Id = cmd.Id,
               
                };
                return Page();
            }

            var silla = await mediator.Send(new LeerSillaPorId(cmd.Id));
          
            if(silla == null){
                return NotFound();
            }
            
            var res = await mediator.Send(cmd);

            return RedirectToPage("./Index");
        }

        public class SillaABorrar {
            public string Id { get; set; }  
            public string TipoDeMaterial { get; set; }
            
            
        }
    }

}

