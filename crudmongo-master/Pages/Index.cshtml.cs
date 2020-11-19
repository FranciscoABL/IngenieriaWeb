using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace aspnetdemo2.Pages
{
    public class IndexModel : PageModel
    {

        public DateTime Hora;
        public string Nombre;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Hora = DateTime.Now;
            if(User.Identity.IsAuthenticated){
                Nombre = User.Identity.Name;
            }else{
                Nombre = "Anonimo";
            }
        }
    }
}
