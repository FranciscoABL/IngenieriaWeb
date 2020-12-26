
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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

namespace aspnetdemo2.Pages.sillas
{
    public class EditarModel : PageModel
    {

        
        [BindProperty]
        public SillaAEditar Detalle {get; set;}
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
            var silla = await mediator.Send(new LeerSillaPorId(id));
          
            if(silla == null){
                return NotFound();
            }
            Detalle = new SillaAEditar() {
                Id =silla.Id,
                NumeroDePatas = silla.NumeroDePatas,
                TipoDeMaterial = silla.TipoDeMaterial,
                DescansaBrazo = silla.DescansaBrazo
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(EditarSillaCommand cmd){

            if(!ModelState.IsValid){
                Detalle = new SillaAEditar() {
                Id = cmd.Id, 
                NumeroDePatas= cmd.NumeroDePatas,
                TipoDeMaterial = cmd.TipoDeMaterial,
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

            public class SillaAEditar {
            public string Id { get; set; }  
            public int NumeroDePatas { get; set; }  
            public string TipoDeMaterial { get; set; }
            public bool DescansaBrazo  { get; set; }
            
        }
    }
}
