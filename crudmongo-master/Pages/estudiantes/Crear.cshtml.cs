using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using aspnetdemo2.domain.estudiantes;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace aspnetdemo2.Pages.estudiantes
{
    public class CrearModel : PageModel
    {

        private readonly ILogger<CrearModel> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMediator mediator;
       
        public string NumeroControl { get;  set; }
        public string Nombre { get; set; }
        public string Carrera { get;set; }
        public float Promedio { get; set; }

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

        public async Task<IActionResult> OnPost(CrearEstudianteCommand cmd ){
            //do nothing

             var correo = $"{cmd.NumeroControl}@culiacan.tecnm.mx";

            var existe = await userManager.FindByEmailAsync(correo);

            if(existe != null){
                 ModelState.AddModelError("Numero Control", "Ese usuario ya existe ");
            }
          

            if(!ModelState.IsValid){
                NumeroControl = cmd.NumeroControl;
                Nombre = cmd.Nombre;
                Carrera = cmd.Carrera;
                Promedio = cmd.Promedio;
                
                return Page();
            }
 var user = new IdentityUser { UserName = correo, Email = correo };
              foreach(var val in userManager.PasswordValidators){
                var valRes = await val.ValidateAsync(userManager, user, cmd.Password);
                if(!valRes.Succeeded){
                    foreach(var er in valRes.Errors){
                        ModelState.AddModelError(er.Code, er.Description);
                    }
                }
            }
           
            var res = await  mediator.Send(cmd);
            //Crear nuevo alumno
            
            
                var result = await userManager.CreateAsync(user, cmd.Password);
            if (!result.Succeeded)
            {
                foreach(var er in result.Errors){
                    ModelState.AddModelError(er.Code, er.Description);
                }

                NumeroControl = cmd.NumeroControl;
                Nombre = cmd.Nombre;
                Carrera = cmd.Carrera;
                Promedio = cmd.Promedio;
                
                return Page();
            }


            return RedirectToPage("./Index");

        }


    }
}
