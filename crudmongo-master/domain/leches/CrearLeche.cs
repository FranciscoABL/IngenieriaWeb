using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.leches {


    public class CrearLecheCommand : IRequest<bool> {
        public CrearLecheCommand(string nombre, 
                string marca, 
                int etapa
        )
        {
            Nombre = nombre;
            Marca=marca;
            Etapa=etapa;

        }
        public CrearLecheCommand()
        {
            
        }

        public string Nombre{ get; set; }  
        public string Marca { get; set; }
        public int Etapa  { get; set; }

    }


    public class CrearLecheCommandValidator : AbstractValidator<CrearLecheCommand>
    {
        public CrearLecheCommandValidator()
        {
            RuleFor(m => m.Nombre).NotEmpty().MinimumLength(3);
            RuleFor(m => m.Marca).NotEmpty().MinimumLength(3);
            RuleFor(m => m.Etapa).NotEmpty()
                        .Must( p => p >= 0);
                
        }
    }


    public class CrearLecheCommandHandler
           : IRequestHandler<CrearLecheCommand, bool>
    {
        private readonly IMongoCollection<Leche> leches;

        public CrearLecheCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            leches = db.GetCollection<Leche>("Leches");
        }

        public async Task<bool> Handle(CrearLecheCommand request, CancellationToken cancellationToken)
        {
           
           var mgLeche = new Leche() {
                Nombre = request.Nombre,
                Marca = request.Marca,
                Etapa= request.Etapa,
            };

            await leches.InsertOneAsync(mgLeche);

            return true;
            
        }
    }

    

}