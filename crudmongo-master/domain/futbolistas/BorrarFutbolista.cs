using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.futbolistas {


    public class BorrarFutbolistaCommand : IRequest<bool> {
        public BorrarFutbolistaCommand(string id)
        {
            Id = id;
        }
        public BorrarFutbolistaCommand()
        {
            
        }

        public string Id { get;  set;}
        

    }


    public class BorrarFutbolistaCommandValidator : AbstractValidator<BorrarFutbolistaCommand>
    {
        public BorrarFutbolistaCommandValidator()
        {
           
        }
    }


    public class BorrarFutbolistaCommandHandler
           : IRequestHandler<BorrarFutbolistaCommand, bool>
    {
        private readonly IMongoCollection<Futbolista> Futbolistas;

        public BorrarFutbolistaCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            Futbolistas = db.GetCollection<Futbolista>("Futbolistas");
        }

        public async Task<bool> Handle(BorrarFutbolistaCommand request, CancellationToken cancellationToken)
        {
           await Futbolistas
                    .DeleteOneAsync( fut => fut.Id == request.Id);
            
            return true;
        }
    }
}