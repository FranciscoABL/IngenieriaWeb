using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using aspnetdemo2.domain.sillas;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace aspnetdemo2.Pages.sillas
{

    public class CrearModel : PageModel
    {

   private readonly ILogger<CrearModel> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMediator mediator;
       
        public int NumeroDePatas { get; set; }
        public string TipoDeMaterial{ get;set; }
        public bool DescansaBrazo { get; set; }

        public CrearModel(ILogger<CrearModel> logger,
        UserManager<IdentityUser> userManager,
         IMediator med)
        {
            _logger = logger;
            this.userManager = userManager;
            mediator = med;
        }

        public void OnGet()
        {
           
            
        }

        public async Task<IActionResult> OnPost(CrearSillaCommand cmd ){
            //do nothing
            if(!ModelState.IsValid){
                NumeroDePatas = cmd.NumeroDePatas;
                TipoDeMaterial = cmd.TipoDeMaterial;                
                return Page();
            }
            var res = await  mediator.Send(cmd);
            //Crear nuevo Silla     
            return RedirectToPage("./Index");

        }
    }
}

    