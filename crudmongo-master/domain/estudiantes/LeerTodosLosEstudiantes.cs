using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.estudiantes {


    public class LeerTodosLosEstudiantes : IRequest<List<EstudianteIndexModel>> {

     }

     public class EstudianteIndexModel {
        public string   Nombre { get; set; }
        public string NumeroControl { get; set; }
        public string Carrera { get; set; }

        public string FotoURL {get; set;}

     }

    public class LeerTodosLosEstudiantesHandler : IRequestHandler<LeerTodosLosEstudiantes,List<EstudianteIndexModel>>
    {
        private readonly IMongoCollection<MongoEstudiantesRespository.MongoEstudiante> estudiantes;

        public LeerTodosLosEstudiantesHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            estudiantes = db.GetCollection<MongoEstudiantesRespository.MongoEstudiante>(settings.CollectionName);
        }

        public async Task<List<EstudianteIndexModel>> Handle(LeerTodosLosEstudiantes request, CancellationToken cancellationToken)
        {
            var resultado = await estudiantes.FindAsync<MongoEstudiantesRespository.MongoEstudiante>(est => true);
            var res = resultado.ToList().Select(est => 
                    new EstudianteIndexModel() {
                        NumeroControl = est.NumeroControl,
                        Nombre = est.Nombre,
                        Carrera = est.Carrera,
                        FotoURL = est.FotoURL
                    }
            );

            return res.ToList();
        }
    }

}