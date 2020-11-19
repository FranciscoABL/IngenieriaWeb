using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using aspnetdemo2.domain.futbolistas;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace aspnetdemo2.Pages.futbolistas
{

    public class CrearModel : PageModel
    {

   private readonly ILogger<CrearModel> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMediator mediator;
       
        
        public string Nombre { get; set; }
        public string Equipo{ get;set; }
        public int Edad { get; set; }
        public float Precio { get; set; }

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

        public async Task<IActionResult> OnPost(CrearFutbolistaCommand cmd ){
            //do nothing
            var res = await  mediator.Send(cmd);
            //Crear nuevo alumno       
            return RedirectToPage("./Index");

        }
    }
}

    