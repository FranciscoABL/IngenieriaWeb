using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.estudiantes {


    public class BorrarEstudianteCommand : IRequest<bool> {
        public BorrarEstudianteCommand(string numeroControl)
        {
            NumeroControl = numeroControl;
        }
        public BorrarEstudianteCommand()
        {
            
        }

        public string NumeroControl { get;  set; }
        

    }


    public class BorrarEstudianteCommandValidator : AbstractValidator<BorrarEstudianteCommand>
    {
        public BorrarEstudianteCommandValidator()
        {
            RuleFor(m => m.NumeroControl).MinimumLength(6).NotEmpty();
        }
    }


    public class BorrarEstudianteCommandHandler
           : IRequestHandler<BorrarEstudianteCommand, bool>
    {
        private readonly IMongoCollection<MongoEstudiantesRespository.MongoEstudiante> estudiantes;

        public BorrarEstudianteCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            estudiantes = db.GetCollection<MongoEstudiantesRespository.MongoEstudiante>(settings.CollectionName);
        }

        public async Task<bool> Handle(BorrarEstudianteCommand request, CancellationToken cancellationToken)
        {
           await estudiantes
                    .DeleteOneAsync( est => est.NumeroControl == request.NumeroControl);
            
            return true;
        }
    }

    

}