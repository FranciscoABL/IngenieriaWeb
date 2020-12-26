using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.sillas {


    public class BorrarSillaCommand : IRequest<bool> {
        public BorrarSillaCommand(string id)
        {
            Id = id;
        }
        public BorrarSillaCommand()
        {
            
        }

        public string Id { get;  set;}
        

    }


    public class BorrarSillaCommandValidator : AbstractValidator<BorrarSillaCommand>
    {
        public BorrarSillaCommandValidator()
        {
           
        }
    }


    public class BorrarSillaCommandHandler
           : IRequestHandler<BorrarSillaCommand, bool>
    {
        private readonly IMongoCollection<Silla> sillas;

        public BorrarSillaCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            sillas = db.GetCollection<Silla>("Sillas");
        }

        public async Task<bool> Handle(BorrarSillaCommand request, CancellationToken cancellationToken)
        {
           await sillas
                    .DeleteOneAsync( fut => fut.Id == request.Id);
            
            return true;
        }
    }
}