using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.futbolistas {


    public class EditarFutbolistaCommand : IRequest<bool> {
        public EditarFutbolistaCommand(string id, string nombre, string equipo, int edad, float precio)
        {
            Id = id;
            Nombre = nombre;
            Equipo = equipo;
            Edad = edad;
            Precio = precio;
        }
        public EditarFutbolistaCommand()
        {
            
        }

        public string Id { get;  set; }
        public string Nombre { get; set; }

        public string Equipo { get; set; }

        public int Edad { get; set; }

        public float Precio {get; set;}



        

    }


    public class EditarFutbolistaCommandValidator : AbstractValidator<EditarFutbolistaCommand>
    {
        public EditarFutbolistaCommandValidator()
        {
           // RuleFor(m => m.NumeroControl).MinimumLength(6).NotEmpty();
             RuleFor(m => m.Nombre).NotEmpty().MinimumLength(1);
            RuleFor(m => m.Equipo).NotEmpty();
            RuleFor(m => m.Edad).NotEmpty().Must( p => p >= 0);
            RuleFor(m => m.Precio).NotEmpty().Must( p => p >= 0);
        }
    }


    public class EditarFutbolistaCommandHandler
           : IRequestHandler<EditarFutbolistaCommand, bool>
    {
        private readonly IMongoCollection<Futbolista> Futbolistas;

        public EditarFutbolistaCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            Futbolistas = db.GetCollection<Futbolista>("Futbolistas");
        }

        public async Task<bool> Handle(EditarFutbolistaCommand request, CancellationToken cancellationToken)
        {
            var mgFutbolista = (await Futbolistas
                    .FindAsync<Futbolista>( fut => fut.Id == request.Id)
            ).FirstOrDefault();
            mgFutbolista.Nombre = request.Nombre;
            mgFutbolista.Equipo= request.Equipo;
            mgFutbolista.Edad = request.Edad;
            mgFutbolista.Precio = request.Precio;
             Futbolistas.ReplaceOne(fut => fut.Id == mgFutbolista.Id,
                mgFutbolista
              );
            
            return true;
        }
    }

    

}