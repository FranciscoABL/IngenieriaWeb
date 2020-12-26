using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.leches {


    public class LeerLechePorId : IRequest<LecheDetalleModel> {
        public LeerLechePorId(string id)
        {
            Id =id;
        }

        public string Id { get; }

     }

     public class LecheDetalleModel {
        public string Id { get; set; }
        public string Nombre{ get; set; }  
        public string Marca { get; set; }
        public int Etapa  { get; set; }


     }

    public class LeerLechePorIdHandler
           : IRequestHandler<LeerLechePorId, LecheDetalleModel>
    {
        private readonly IMongoCollection<Leche> leches;

        public LeerLechePorIdHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            leches = db.GetCollection<Leche>("Leches");
        }

        public async Task<LecheDetalleModel> Handle(LeerLechePorId request, CancellationToken cancellationToken)
        {
            
            var mgest = (await leches
                    .FindAsync<Leche>( 
                            est => est.Id == request.Id
                            )
            ).FirstOrDefault();
            
           
           if(mgest is Leche m ){
            return new LecheDetalleModel() {
                Id=m.Id,
                Nombre = m.Nombre,
                Marca=m.Marca,
                Etapa=m.Etapa
            };
           }else{
               return null;
           }
        }
    }

}