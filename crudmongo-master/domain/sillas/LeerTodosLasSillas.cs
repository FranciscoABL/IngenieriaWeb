
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.sillas {


    public class LeerTodosLasSillas : IRequest<List<SillaIndexModel>> {

     }

     public class SillaIndexModel {
         public string Id {get; set;} 
        public int NumeroDePatas { get; set; }
        public string TipoDeMaterial { get;set; }
        public bool DescansaBrazo { get; set; }

     }

    public class LeerTodosLasSillasHandler : IRequestHandler<LeerTodosLasSillas,List<SillaIndexModel>>
    {
        private readonly IMongoCollection<Silla> sillas;

        public LeerTodosLasSillasHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            sillas = db.GetCollection<Silla>("Sillas");
        }

        public async Task<List<SillaIndexModel>> Handle(LeerTodosLasSillas request, CancellationToken cancellationToken)
        {
            var resultado = await sillas.FindAsync<Silla>(fut => true);
            var res = resultado.ToList().Select(fut => 
                    new SillaIndexModel() {
                        Id=fut.Id,
                        NumeroDePatas = fut.NumeroDePatas,
                        TipoDeMaterial= fut.TipoDeMaterial,
                        DescansaBrazo = fut.DescansaBrazo
                    }
            );

            return res.ToList();
        }
    }

}