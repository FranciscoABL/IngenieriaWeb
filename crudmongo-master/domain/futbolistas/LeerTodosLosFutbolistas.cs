
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.futbolistas {


    public class LeerTodosLosFutbolistas : IRequest<List<FutbolistaIndexModel>> {

     }

     public class FutbolistaIndexModel {
         public string Id {get; set;} 
        public string   Nombre { get; set; }
        public string Equipo { get; set; }
        public int Edad { get; set; }
        public float Precio {get; set;}

     }

    public class LeerTodosLosFutbolistasHandler : IRequestHandler<LeerTodosLosFutbolistas,List<FutbolistaIndexModel>>
    {
        private readonly IMongoCollection<Futbolista> futbolistas;

        public LeerTodosLosFutbolistasHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            futbolistas = db.GetCollection<Futbolista>("Futbolistas");
        }

        public async Task<List<FutbolistaIndexModel>> Handle(LeerTodosLosFutbolistas request, CancellationToken cancellationToken)
        {
            var resultado = await futbolistas.FindAsync<Futbolista>(fut => true);
            var res = resultado.ToList().Select(fut => 
                    new FutbolistaIndexModel() {
                        Id=fut.Id,
                        Nombre = fut.Nombre,
                        Equipo = fut.Equipo,
                        Edad = fut.Edad,
                        Precio = fut.Precio
                    }
            );

            return res.ToList();
        }
    }

}