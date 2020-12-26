using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
 namespace aspnetdemo2.domain.libros{
 public class LeerLibroPorId : IRequest<LibroDetalleModel> {
        public LeerLibroPorId(string id)
        {
           Id=id;
        }

        public string Id { get; }

     }

     public class LibroDetalleModel {
         public string   Id { get; set; }
        public string   Titulo { get; set; }
        public string Genero { get; set; }
        public float Precio { get; set; }

     }

    public class LeerLibroPorIdHandler
           : IRequestHandler<LeerLibroPorId, LibroDetalleModel>
    {
        private readonly IMongoCollection<Libro> libros;

        public LeerLibroPorIdHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            libros = db.GetCollection<Libro>("Libros");
        }

        public async Task<LibroDetalleModel> Handle(LeerLibroPorId request, CancellationToken cancellationToken)
        {
            
            var mgest = (await libros
                    .FindAsync<Libro>( 
                            est => est.Id == request.Id
                            )
            ).FirstOrDefault();
            
           
           if(mgest is Libro m ){
            return new LibroDetalleModel() {
                Id=m.Id,
                Titulo= m.Titulo,
                Genero = m.Genero,
                Precio=m.Precio
            };
           }else{
               return null;
           }
        }
    }

}