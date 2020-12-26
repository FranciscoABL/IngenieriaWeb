
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.libros {


    public class LeerTodosLosLibros : IRequest<List<LibroIndexModel>> {

     }

     public class LibroIndexModel {
         public string Id {get; set;} 
        public string   Titulo { get; set; }
        public string Genero { get; set; }
        public float Precio {get; set;}

     }

    public class LeerTodosLosLibrosHandler : IRequestHandler<LeerTodosLosLibros,List<LibroIndexModel>>
    {
        private readonly IMongoCollection<Libro> libros;

        public LeerTodosLosLibrosHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            libros = db.GetCollection<Libro>("Libros");
        }

        public async Task<List<LibroIndexModel>> Handle(LeerTodosLosLibros request, CancellationToken cancellationToken)
        {
            var resultado = await libros.FindAsync<Libro>(fut => true);
            var res = resultado.ToList().Select(fut => 
                    new LibroIndexModel() {
                        Id=fut.Id,
                        Titulo = fut.Titulo,
                        Genero= fut.Genero,
                        Precio = fut.Precio
                    }
            );

            return res.ToList();
        }
    }

}