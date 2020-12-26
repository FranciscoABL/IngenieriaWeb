using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.sillas {


    public class EditarSillaCommand : IRequest<bool> {
        public EditarSillaCommand(string id, int numerodepatas, string tipodematerial, bool descansabrazo)
        {
            Id = id;
            NumeroDePatas = numerodepatas;
            TipoDeMaterial = tipodematerial;
            DescansaBrazo = descansabrazo;
        }
        public EditarSillaCommand()
        {
            
        }

        public string Id { get;  set; }
        public int NumeroDePatas { get; set; }
        public string TipoDeMaterial { get;set; }
        public bool DescansaBrazo { get; set; }

    }


    public class EditarSillaCommandValidator : AbstractValidator<EditarSillaCommand>
    {
        public EditarSillaCommandValidator()
        {
           // RuleFor(m => m.NumeroControl).MinimumLength(6).NotEmpty();
             RuleFor(m => m.NumeroDePatas).NotEmpty().Must( p => p >= 0);
            RuleFor(m => m.TipoDeMaterial).NotEmpty().MinimumLength(3);
        }
    }


    public class EditarSillaCommandHandler
           : IRequestHandler<EditarSillaCommand, bool>
    {
        private readonly IMongoCollection<Silla> sillas;

        public EditarSillaCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            sillas = db.GetCollection<Silla>("Sillas");
        }

        public async Task<bool> Handle(EditarSillaCommand request, CancellationToken cancellationToken)
        {
            var mgSilla = (await sillas
                    .FindAsync<Silla>( fut => fut.Id == request.Id)
            ).FirstOrDefault();
            mgSilla.NumeroDePatas = request.NumeroDePatas;
            mgSilla.TipoDeMaterial= request.TipoDeMaterial;
            mgSilla.DescansaBrazo = request.DescansaBrazo;
             sillas.ReplaceOne(fut => fut.Id == mgSilla.Id,
                mgSilla
              );
            
            return true;
        }
    }

    

}