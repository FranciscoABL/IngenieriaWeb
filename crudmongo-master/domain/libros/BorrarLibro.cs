using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.libros {


    public class BorrarLibroCommand : IRequest<bool> {
        public BorrarLibroCommand(string id)
        {
            Id = id;
        }
        public BorrarLibroCommand()
        {
            
        }

        public string Id { get;  set;}
        

    }


    public class BorrarLibroCommandValidator : AbstractValidator<BorrarLibroCommand>
    {
        public BorrarLibroCommandValidator()
        {
           
        }
    }


    public class BorrarLibroCommandHandler
           : IRequestHandler<BorrarLibroCommand, bool>
    {
        private readonly IMongoCollection<Libro> libros;

        public BorrarLibroCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            libros = db.GetCollection<Libro>("Libros");
        }

        public async Task<bool> Handle(BorrarLibroCommand request, CancellationToken cancellationToken)
        {
           await libros
                    .DeleteOneAsync( fut => fut.Id == request.Id);
            
            return true;
        }
    }
}