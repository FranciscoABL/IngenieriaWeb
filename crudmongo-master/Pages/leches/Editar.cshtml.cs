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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

namespace aspnetdemo2.Pages.leches
{
    public class EditarModel : PageModel
    {

        
        [BindProperty]
        public LecheAEditar Detalle {get; set;}
        private readonly ILogger<EditarModel> _logger;
        private readonly IMediator mediator;
        private readonly IConfiguration configuracion;

        //private ILechesRespository repo;

        [BindProperty]
        public IFormFile Foto {get; set;}

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
            var leche = await mediator.Send(new LeerLechePorId(id));
          
            if(leche == null){
                return NotFound();
            }
            Detalle = new LecheAEditar() {
                Id=leche.Id,
                Nombre = leche.Nombre,
                Marca=leche.Marca,
                Etapa=leche.Etapa
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(EditarLecheCommand cmd){

            if(!ModelState.IsValid){
                Detalle = new LecheAEditar() {
                Nombre = cmd.Nombre,
                Marca=cmd.Marca,
                Etapa=cmd.Etapa
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

        public class LecheAEditar {
        public string Id { get; set; }
        public string Nombre{ get; set; }  
        public string Marca { get; set; }
        public int Etapa  { get; set; }

            
        }
    }
}
