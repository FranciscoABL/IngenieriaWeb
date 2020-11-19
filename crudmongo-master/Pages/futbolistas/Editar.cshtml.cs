
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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

namespace aspnetdemo2.Pages.futbolistas
{
    public class EditarModel : PageModel
    {

        
        [BindProperty]
        public FutbolistaAEditar Detalle {get; set;}
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
            var futbolista = await mediator.Send(new LeerFutbolistaPorId(id));
          
            if(futbolista == null){
                return NotFound();
            }
            Detalle = new FutbolistaAEditar() {
                Id =futbolista.Id,
                Nombre = futbolista.Nombre,
                Equipo = futbolista.Equipo,
                Edad = futbolista.Edad,
                Precio = futbolista.Precio
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(EditarFutbolistaCommand cmd){

            if(!ModelState.IsValid){
                Detalle = new FutbolistaAEditar() {
                Id = cmd.Id, 
                Nombre = cmd.Nombre,
                Equipo = cmd.Equipo,
                Edad = cmd.Edad,
                Precio = cmd.Precio
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

            public class FutbolistaAEditar {
            public string Id { get; set; }  
            public string Nombre { get; set; }
            public string Equipo { get; set; }
            public int Edad { get; set; }
            public float Precio { get; set; }
            
        }
    }
}
