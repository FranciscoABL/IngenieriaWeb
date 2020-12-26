
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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

namespace aspnetdemo2.Pages.libros
{
    public class EditarModel : PageModel
    {

        
        [BindProperty]
        public LibroAEditar Detalle {get; set;}
        private readonly ILogger<EditarModel> _logger;
        private readonly IMediator mediator;
        private readonly IConfiguration configuracion;

        //private IEstudiantesRespository repo;

       

        public EditarModel(ILogger<EditarModel> logger,
         IMediator mediat,
         IConfiguration config)
        {
            _logger = logger;
            
            mediator = mediat;
            configuracion = config;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            var libro = await mediator.Send(new LeerLibroPorId(id));
          
            if(libro == null){
                return NotFound();
            }
            Detalle = new LibroAEditar() {
                Id =libro.Id,
                Titulo = libro.Titulo,
                Genero = libro.Genero,
                Precio = libro.Precio
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(EditarLibroCommand cmd){

            if(!ModelState.IsValid){
                Detalle = new LibroAEditar() {
                Id = cmd.Id, 
                Titulo= cmd.Titulo,
                Genero = cmd.Genero,
                Precio = cmd.Precio
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

            public class LibroAEditar {
            public string Id { get; set; }  
            public string Titulo { get; set; }
            public string Genero { get; set; }
            public float Precio { get; set; }
            
        }
    }
}
