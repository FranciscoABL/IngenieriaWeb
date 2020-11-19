using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.estudiantes {


    public class EditarEstudianteCommand : IRequest<bool> {
        public EditarEstudianteCommand(string numeroControl, string nombre, string carrera)
        {
            NumeroControl = numeroControl;
            Nombre = nombre;
            Carrera = carrera;
        }
        public EditarEstudianteCommand()
        {
            
        }

        public string NumeroControl { get;  set; }
        public string Nombre { get; set; }
        public string Carrera { get; set; }
        

    }


    public class EditarEstudianteCommandValidator : AbstractValidator<EditarEstudianteCommand>
    {
        public EditarEstudianteCommandValidator()
        {
            RuleFor(m => m.NumeroControl).MinimumLength(6).NotEmpty();
            RuleFor(m => m.Nombre).NotEmpty().MinimumLength(3);
        }
    }


    public class EditarEstudianteCommandHandler
           : IRequestHandler<EditarEstudianteCommand, bool>
    {
        private readonly IMongoCollection<MongoEstudiantesRespository.MongoEstudiante> estudiantes;

        public EditarEstudianteCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            estudiantes = db.GetCollection<MongoEstudiantesRespository.MongoEstudiante>(settings.CollectionName);
        }

        public async Task<bool> Handle(EditarEstudianteCommand request, CancellationToken cancellationToken)
        {
            var mgest = (await estudiantes
                    .FindAsync<MongoEstudiantesRespository.MongoEstudiante>( est => est.NumeroControl == request.NumeroControl)
            ).FirstOrDefault();
            mgest.Nombre = request.Nombre;
            mgest.Carrera = request.Carrera;
             estudiantes.ReplaceOne(est => est.Id == mgest.Id,
                mgest
              );
            
            return true;
        }
    }

    

}