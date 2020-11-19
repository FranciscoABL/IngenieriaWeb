using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.estudiantes {


    public class LeerEstudiantePorNumeroDeControl : IRequest<EstudianteDetalleModel> {
        public LeerEstudiantePorNumeroDeControl(string numeroControl)
        {
            NumeroControl = numeroControl;
        }

        public string NumeroControl { get; }

     }

     public class EstudianteDetalleModel {
        public string   Nombre { get; set; }
        public string NumeroControl { get; set; }
        public string Carrera { get; set; }
        public float Promedio { get; set; }

     }

    public class LeerEstudiantePorNumeroDeControlHandler
           : IRequestHandler<LeerEstudiantePorNumeroDeControl, EstudianteDetalleModel>
    {
        private readonly IMongoCollection<MongoEstudiantesRespository.MongoEstudiante> estudiantes;

        public LeerEstudiantePorNumeroDeControlHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            estudiantes = db.GetCollection<MongoEstudiantesRespository.MongoEstudiante>(settings.CollectionName);
        }

        public async Task<EstudianteDetalleModel> Handle(LeerEstudiantePorNumeroDeControl request, CancellationToken cancellationToken)
        {
            
            var mgest = (await estudiantes
                    .FindAsync<MongoEstudiantesRespository.MongoEstudiante>( 
                            est => est.NumeroControl == request.NumeroControl
                            )
            ).FirstOrDefault();
            
           
           if(mgest is MongoEstudiantesRespository.MongoEstudiante m ){
            return new EstudianteDetalleModel() {
                NumeroControl = m.NumeroControl,
                Nombre = m.Nombre,
                Carrera = m.Carrera,
                Promedio = m.Promedio
            };
           }else{
               return null;
           }
        }
    }

}