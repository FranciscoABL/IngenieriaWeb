using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.estudiantes {


    public class CrearEstudianteCommand : IRequest<bool> {
        public CrearEstudianteCommand(string numeroControl, 
                string nombre, 
                string carrera, 
                float promedio,
                string password,
                string confirmarPassword)
        {
            NumeroControl = numeroControl;
            Nombre = nombre;
            Carrera = carrera;
            Promedio = promedio;
            Password = password;
            ConfirmarPassword = confirmarPassword;
        }
        public CrearEstudianteCommand()
        {
            
        }

        public string NumeroControl { get;  set; }
        public string Nombre { get; set; }
        public string Carrera { get;set; }
        public float Promedio { get; set; }
        public string Password { get; set; }
        public string ConfirmarPassword { get; set; }
    }


    public class CrearEstudianteCommandValidator : AbstractValidator<CrearEstudianteCommand>
    {
        public CrearEstudianteCommandValidator()
        {
            RuleFor(m => m.NumeroControl).MinimumLength(6).NotEmpty();
            RuleFor(m => m.Nombre).NotEmpty().MinimumLength(3);
            RuleFor(m => m.Promedio).NotEmpty()
                        .Must( p => p >= 0);
            RuleFor(m => m.Password).NotEmpty().MinimumLength(4);
            RuleFor(m => m.ConfirmarPassword).NotEmpty().MinimumLength(4);
            RuleFor(m => m.ConfirmarPassword)
                .Equal(m => m.Password);
                
        }
    }


    public class CrearEstudianteCommandHandler
           : IRequestHandler<CrearEstudianteCommand, bool>
    {
        private readonly IMongoCollection<MongoEstudiantesRespository.MongoEstudiante> estudiantes;

        public CrearEstudianteCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            estudiantes = db.GetCollection<MongoEstudiantesRespository.MongoEstudiante>(settings.CollectionName);
        }

        public async Task<bool> Handle(CrearEstudianteCommand request, CancellationToken cancellationToken)
        {
           
           var mgEstudiante = new MongoEstudiantesRespository.MongoEstudiante() {
                Nombre = request.Nombre,
                NumeroControl = request.NumeroControl,
                Promedio = request.Promedio,
                Carrera = request.Carrera
            };

            await estudiantes.InsertOneAsync(mgEstudiante);

            return true;
            
        }
    }

    

}