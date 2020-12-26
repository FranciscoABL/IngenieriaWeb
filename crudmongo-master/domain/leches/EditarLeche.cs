using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.leches {


    public class EditarLecheCommand : IRequest<bool> {
        public EditarLecheCommand(string nombre, string marca,int etapa)
        {
            Nombre = nombre;
            Marca = marca;
            Etapa=etapa;
        }
        public EditarLecheCommand()
        {
            
        }

        public string Id { get; set; }
        public string Nombre{ get; set; }  
        public string Marca { get; set; }
        public int Etapa  { get; set; }

        

    }


    public class EditarLecheCommandValidator : AbstractValidator<EditarLecheCommand>
    {
        public EditarLecheCommandValidator()
        {
            RuleFor(m => m.Nombre).NotEmpty().MinimumLength(3);
            RuleFor(m => m.Marca).NotEmpty().MinimumLength(3);
            RuleFor(m => m.Etapa).NotEmpty()
                        .Must( p => p >= 0);
        }
    }


    public class EditarLecheCommandHandler
           : IRequestHandler<EditarLecheCommand, bool>
    {
        private readonly IMongoCollection<Leche> leches;

        public EditarLecheCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            leches = db.GetCollection<Leche>("Leches");
        }

        public async Task<bool> Handle(EditarLecheCommand request, CancellationToken cancellationToken)
        {
            var mgest = (await leches
                    .FindAsync<Leche>( est => est.Id == request.Id)
            ).FirstOrDefault();
             mgest.Id = request.Id;
            mgest.Nombre = request.Nombre;
            mgest.Marca=request.Marca;
            mgest.Etapa=request.Etapa;
             leches.ReplaceOne(est => est.Id == mgest.Id,
                mgest
              );
            
            return true;
        }
    }

    

}