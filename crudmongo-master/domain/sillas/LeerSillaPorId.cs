using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
 namespace aspnetdemo2.domain.sillas{
 public class LeerSillaPorId : IRequest<SillaDetalleModel> {
        public LeerSillaPorId(string id)
        {
           Id=id;
        }

        public string Id { get; }

     }

     public class SillaDetalleModel {
         public string   Id { get; set; }
         public int NumeroDePatas { get; set; }
        public string TipoDeMaterial { get;set; }
        public bool DescansaBrazo { get; set; }

     }

    public class LeerSillaPorIdHandler
           : IRequestHandler<LeerSillaPorId, SillaDetalleModel>
    {
        private readonly IMongoCollection<Silla> sillas;

        public LeerSillaPorIdHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            sillas = db.GetCollection<Silla>("Sillas");
        }

        public async Task<SillaDetalleModel> Handle(LeerSillaPorId request, CancellationToken cancellationToken)
        {
            
            var mgest = (await sillas
                    .FindAsync<Silla>( 
                            est => est.Id == request.Id
                            )
            ).FirstOrDefault();
            
           
           if(mgest is Silla m ){
            return new SillaDetalleModel() {
                Id=m.Id,
                NumeroDePatas = m.NumeroDePatas,
                TipoDeMaterial= m.TipoDeMaterial,
                DescansaBrazo = m.DescansaBrazo
            };
           }else{
               return null;
           }
        }
    }

}