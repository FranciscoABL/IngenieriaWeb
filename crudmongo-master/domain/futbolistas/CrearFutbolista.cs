using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.futbolistas{


    public class CrearFutbolistaCommand : IRequest<bool> {
        public CrearFutbolistaCommand( 
                string nombre, 
                string equipo,
                int edad, 
                float precio
        )
        {
            Nombre = nombre;
            Equipo = equipo;
            Edad = Edad;
            Precio = Precio;
            
        }
        public CrearFutbolistaCommand()
        {
            
        }


        public string Nombre { get; set; }
        public string Equipo { get;set; }
        public int Edad { get; set; }
        public float Precio { get; set; }
    }


    public class CrearFutbolistaCommandValidator : AbstractValidator<CrearFutbolistaCommand>
    {
        public CrearFutbolistaCommandValidator()
        {
            
            RuleFor(m => m.Nombre).NotEmpty().MinimumLength(3);
            RuleFor(m => m.Equipo).NotEmpty().MinimumLength(3);
            RuleFor(m => m.Edad).NotEmpty()
                        .Must( p => p >= 0);
            RuleFor(m => m.Precio).NotEmpty()
                        .Must( p => p >= 0);
                
        }
    }


    public class CrearFutbolistaCommandHandler
           : IRequestHandler<CrearFutbolistaCommand, bool>
    {
        private readonly IMongoCollection<Futbolista> futbolistas;

        public CrearFutbolistaCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            futbolistas = db.GetCollection<Futbolista>("Futbolistas");
        }

        public async Task<bool> Handle(CrearFutbolistaCommand request, CancellationToken cancellationToken)
        {
           
           var mgFutbolista = new Futbolista() {
                Nombre = request.Nombre,
                Equipo = request.Equipo,
                Edad = request.Edad,
                Precio = request.Precio
            };

            await futbolistas.InsertOneAsync(mgFutbolista);

            return true;
            
        }
    }

    

}