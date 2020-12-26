using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace aspnetdemo2.domain.libros{


    public class CrearLibroCommand : IRequest<bool> {
        public CrearLibroCommand( 
                string titulo, 
                string genero,
                float precio
        )
        {
            Titulo = titulo;
            Genero = genero;
            Precio = Precio;
            
        }
        public CrearLibroCommand()
        {
            
        }


        public string Titulo { get; set; }
        public string Genero { get;set; }
        public float Precio { get; set; }
    }


    public class CrearLibroCommandValidator : AbstractValidator<CrearLibroCommand>
    {
        public CrearLibroCommandValidator()
        {
            
            RuleFor(m => m.Titulo).NotEmpty().MinimumLength(3);
            RuleFor(m => m.Genero).NotEmpty().MinimumLength(3);
            RuleFor(m => m.Precio).NotEmpty()
                        .Must( p => p >= 0);
                
        }
    }


    public class CrearLibroCommandHandler
           : IRequestHandler<CrearLibroCommand, bool>
    {
        private readonly IMongoCollection<Libro> libros;

        public CrearLibroCommandHandler(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            libros = db.GetCollection<Libro>("Libros");
        }

        public async Task<bool> Handle(CrearLibroCommand request, CancellationToken cancellationToken)
        {
           
           var mgLibro = new Libro() {
                Titulo = request.Titulo,
                Genero = request.Genero,
                Precio = request.Precio
            };

            await libros.InsertOneAsync(mgLibro);

            return true;
            
        }
    }

    

}