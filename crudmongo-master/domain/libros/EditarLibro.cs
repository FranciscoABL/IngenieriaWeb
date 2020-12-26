using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.libros {


    public class EditarLibroCommand : IRequest<bool> {
        public EditarLibroCommand(string id, string titulo, string genero, float precio)
        {
            Id = id;
            Titulo = titulo;
            Genero = genero;
            Precio = precio;
        }
        public EditarLibroCommand()
        {
            
        }

        public string Id { get;  set; }
        public string Titulo{ get; set; }

        public string Genero { get; set; }

        public float Precio {get; set;}



        

    }


    public class EditarLibroCommandValidator : AbstractValidator<EditarLibroCommand>
    {
        public EditarLibroCommandValidator()
        {
           // RuleFor(m => m.NumeroControl).MinimumLength(6).NotEmpty();
             RuleFor(m => m.Titulo).NotEmpty().MinimumLength(5);
            RuleFor(m => m.Genero).NotEmpty();
            RuleFor(m => m.Precio).NotEmpty().Must( p => p >= 0);
        }
    }


    public class EditarLibroCommandHandler
           : IRequestHandler<EditarLibroCommand, bool>
    {
        private readonly IMongoCollection<Libro> libros;

        public EditarLibroCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            libros = db.GetCollection<Libro>("Libros");
        }

        public async Task<bool> Handle(EditarLibroCommand request, CancellationToken cancellationToken)
        {
            var mgLibro = (await libros
                    .FindAsync<Libro>( fut => fut.Id == request.Id)
            ).FirstOrDefault();
            mgLibro.Titulo = request.Titulo;
            mgLibro.Genero= request.Genero;
            mgLibro.Precio = request.Precio;
             libros.ReplaceOne(fut => fut.Id == mgLibro.Id,
                mgLibro
              );
            
            return true;
        }
    }

    

}