using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using aspnetdemo2.domain.libros;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace aspnetdemo2.Pages.libros
{

    public class CrearModel : PageModel
    {

   private readonly ILogger<CrearModel> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMediator mediator;
       
        public string Titulo { get; set; }
        public string Genero{ get;set; }
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

        public async Task<IActionResult> OnPost(CrearLibroCommand cmd ){
            //do nothing
            if(!ModelState.IsValid){
                Titulo = cmd.Titulo;
                Genero = cmd.Genero;
                Precio=cmd.Precio;
                
                return Page();
            }
            var res = await  mediator.Send(cmd);
            //Crear nuevo libro     
            return RedirectToPage("./Index");

        }
    }
}

    