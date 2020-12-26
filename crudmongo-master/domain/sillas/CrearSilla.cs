using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.sillas{


    public class CrearSillaCommand : IRequest<bool> {
        public CrearSillaCommand( 
                int  numerodepatas, 
                string tipodematerial,
                bool descansabrazo
        )
        {
            NumeroDePatas = numerodepatas;
            TipoDeMaterial = tipodematerial;
            DescansaBrazo=descansabrazo;
            
        }
        public CrearSillaCommand()
        {
            
        }


        public int NumeroDePatas { get; set; }
        public string TipoDeMaterial { get;set; }
        public bool DescansaBrazo { get; set; }
    }


    public class CrearSillaCommandValidator : AbstractValidator<CrearSillaCommand>
    {
        public CrearSillaCommandValidator()
        {
            
            RuleFor(m => m.NumeroDePatas).NotEmpty().Must( p => p >= 0);
            RuleFor(m => m.TipoDeMaterial).NotEmpty().MinimumLength(3);                        
                
        }
    }


    public class CrearSillaCommandHandler
           : IRequestHandler<CrearSillaCommand, bool>
    {
        private readonly IMongoCollection<Silla> sillas;

        public CrearSillaCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            sillas = db.GetCollection<Silla>("Sillas");
        }

        public async Task<bool> Handle(CrearSillaCommand request, CancellationToken cancellationToken)
        {
           
           var mgSilla = new Silla() {
                NumeroDePatas = request.NumeroDePatas,
                TipoDeMaterial = request.TipoDeMaterial,
                DescansaBrazo = request.DescansaBrazo
            };

            await sillas.InsertOneAsync(mgSilla);

            return true;
            
        }
    }

    

}