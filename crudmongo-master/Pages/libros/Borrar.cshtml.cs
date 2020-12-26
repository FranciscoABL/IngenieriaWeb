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
using Microsoft.AspNetCore.Authorization;
namespace aspnetdemo2.Pages.libros
{
    [Authorize(Roles = "Admin")]
    public class BorrarModel : PageModel
    {

        
        [BindProperty]
        public LibroABorrar Detalle {get; set;}
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
           var libro = await mediator.Send(new LeerLibroPorId(Id));
          
            if(libro == null){
                return NotFound();
            }
            Detalle = new LibroABorrar() {
                Id = libro.Id,
                Titulo = libro.Titulo
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(BorrarLibroCommand cmd){

            if(!ModelState.IsValid){
                Detalle = new LibroABorrar() {
                Id = cmd.Id,
               
                };
                return Page();
            }

            var libro = await mediator.Send(new LeerLibroPorId(cmd.Id));
          
            if(libro == null){
                return NotFound();
            }
            
            var res = await mediator.Send(cmd);

            return RedirectToPage("./Index");
        }

        public class LibroABorrar {
            public string Id { get; set; }  
            public string Titulo { get; set; }
            
            
        }
    }

}

