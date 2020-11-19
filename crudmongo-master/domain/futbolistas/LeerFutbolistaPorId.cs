using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
 namespace aspnetdemo2.domain.futbolistas{
 public class LeerFutbolistaPorId : IRequest<FutbolistaDetalleModel> {
        public LeerFutbolistaPorId(string id)
        {
           Id=id;
        }

        public string Id { get; }

     }

     public class FutbolistaDetalleModel {
         public string   Id { get; set; }
        public string   Nombre { get; set; }
        public string Equipo { get; set; }
        public int Edad { get; set; }
        public float Precio { get; set; }

     }

    public class LeerFutbolistaPorIdHandler
           : IRequestHandler<LeerFutbolistaPorId, FutbolistaDetalleModel>
    {
        private readonly IMongoCollection<Futbolista> futbolistas;

        public LeerFutbolistaPorIdHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            futbolistas = db.GetCollection<Futbolista>("Futbolistas");
        }

        public async Task<FutbolistaDetalleModel> Handle(LeerFutbolistaPorId request, CancellationToken cancellationToken)
        {
            
            var mgest = (await futbolistas
                    .FindAsync<Futbolista>( 
                            est => est.Id == request.Id
                            )
            ).FirstOrDefault();
            
           
           if(mgest is Futbolista m ){
            return new FutbolistaDetalleModel() {
                Id=m.Id,
                Nombre = m.Nombre,
                Equipo = m.Equipo,
                Edad = m.Edad,
                Precio=m.Precio
            };
           }else{
               return null;
           }
        }
    }

}