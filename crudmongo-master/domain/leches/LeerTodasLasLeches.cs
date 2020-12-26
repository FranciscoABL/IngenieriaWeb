using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.leches {


    public class LeerTodasLasLeches : IRequest<List<LecheIndexModel>> {

     }

     public class LecheIndexModel {
         public string Id{ get; set; } 
        public string Nombre{ get; set; }  
        public string Marca { get; set; }
        public int Etapa  { get; set; }
     }

    public class LeerTodasLasLechesHandler : IRequestHandler<LeerTodasLasLeches,List<LecheIndexModel>>
    {
        private readonly IMongoCollection<Leche> leches;

        public LeerTodasLasLechesHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            leches = db.GetCollection<Leche>("Leches");
        }

        public async Task<List<LecheIndexModel>> Handle(LeerTodasLasLeches request, CancellationToken cancellationToken)
        {
            var resultado = await leches.FindAsync<Leche>(est => true);
            var res = resultado.ToList().Select(est => 
                    new LecheIndexModel() {
                        Id=est.Id,
                        Nombre = est.Nombre,
                        Marca = est.Marca,
                        Etapa = est.Etapa
                    }
            );

            return res.ToList();
        }
    }

}