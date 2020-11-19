using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.estudiantes {


    public class CalificarEstudianteCommand : IRequest<bool> {
      

        public CalificarEstudianteCommand(string numeroControl, float promedio)
        {
            NumeroControl = numeroControl;
           Promedio = promedio;
        }
        public CalificarEstudianteCommand()
        {
            
        }

        public string NumeroControl { get;  set; }
        public float Promedio { get; set; }
    }


    public class CalificarEstudianteCommandValidator : AbstractValidator<CalificarEstudianteCommand>
    {
        public CalificarEstudianteCommandValidator()
        {
            RuleFor(m => m.NumeroControl).MinimumLength(6).NotEmpty();
            RuleFor(m => m.Promedio).NotEmpty()
                        .Must( p => p >= 0).WithMessage("Promedio debe de ser positivo.");
        }
    }


    public class CalificarEstudianteCommandHandler
           : IRequestHandler<CalificarEstudianteCommand, bool>
    {
        private readonly IMongoCollection<MongoEstudiantesRespository.MongoEstudiante> estudiantes;

        public CalificarEstudianteCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            estudiantes = db.GetCollection<MongoEstudiantesRespository.MongoEstudiante>(settings.CollectionName);
        }

        public async Task<bool> Handle(CalificarEstudianteCommand request, CancellationToken cancellationToken)
        {
            var mgest = (await estudiantes
                    .FindAsync<MongoEstudiantesRespository.MongoEstudiante>( est => est.NumeroControl == request.NumeroControl)
            ).FirstOrDefault();
            mgest.Promedio = request.Promedio;
            estudiantes.ReplaceOne(est => est.Id == mgest.Id,
                mgest
              );
            
            return true;
        }
    }

    

}