using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetdemo2.domain.estudiantes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace aspnetdemo2.Pages.estudiantes
{
    [Authorize]
    public class IndexModel : PageModel
    {

        public List<EstudianteIndexModel> Estudiantes {get; set;}
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator mediator;

        public IndexModel(ILogger<IndexModel> logger,
        IMediator mediat)
        {
            _logger = logger;
            
            mediator = mediat;
        }

        public async Task OnGet()
        {
            var modelo = await mediator.Send( new LeerTodosLosEstudiantes());
        
            Estudiantes = modelo;
            
        }
        
    }
}
