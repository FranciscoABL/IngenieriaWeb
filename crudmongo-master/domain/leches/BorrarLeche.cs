using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.leches {


    public class BorrarLecheCommand : IRequest<bool> {
        public BorrarLecheCommand(string id)
        {
            Id = id;
        }
        public BorrarLecheCommand()
        {
            
        }

        public string Id { get;  set; }
        

    }



    public class BorrarLecheCommandHandler
           : IRequestHandler<BorrarLecheCommand, bool>
    {
        private readonly IMongoCollection<Leche> leches;

        public BorrarLecheCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            leches = db.GetCollection<Leche>("Leches");
        }

        public async Task<bool> Handle(BorrarLecheCommand request, CancellationToken cancellationToken)
        {
           await leches
                    .DeleteOneAsync( est => est.Id == request.Id);
            
            return true;
        }
    }

    

}