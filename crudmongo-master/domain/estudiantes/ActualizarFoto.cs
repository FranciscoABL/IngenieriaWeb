using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace aspnetdemo2.domain.estudiantes {


    public class ActualizarFotoCommand : IRequest<bool> {
        public ActualizarFotoCommand(string numeroControl, Stream archivo)
        {
            NumeroControl = numeroControl;
            Archivo = archivo;
        }
        public ActualizarFotoCommand()
        {
            
        }

        public string NumeroControl { get;  set; }
        public Stream Archivo { get; set; }
    }


    public class ActualizarFotoCommandValidator : AbstractValidator<ActualizarFotoCommand>
    {
        public ActualizarFotoCommandValidator()
        {
            RuleFor(m => m.NumeroControl).MinimumLength(6).NotEmpty();
            RuleFor(m => m.Archivo).NotNull();
        }
    }


    public class ActualizarFotoCommandHandler
           : IRequestHandler<ActualizarFotoCommand, bool>
    {
        private readonly IMongoCollection<MongoEstudiantesRespository.MongoEstudiante> estudiantes;
        private BlobContainerClient containerClient;

        public ActualizarFotoCommandHandler(IEstudiantesDatabaseSettings settings
            ,IConfiguration config
        )
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            estudiantes = db.GetCollection<MongoEstudiantesRespository.MongoEstudiante>(settings.CollectionName);

            var connectionString = config["AzStorageAccount"];
            var blobServiceClient = new BlobServiceClient(connectionString);
                        
            //var containerClient = await blobServiceClient.CreateBlobContainerAsync("estudiantes");
            containerClient = blobServiceClient.GetBlobContainerClient("estudiantes");


        }

       
        public async Task<bool> Handle(ActualizarFotoCommand request, CancellationToken cancellationToken)
        {
            var mgest = (await estudiantes
                    .FindAsync<MongoEstudiantesRespository.MongoEstudiante>( est => est.NumeroControl == request.NumeroControl)
            ).FirstOrDefault();

            //Subir a Azure. 
            var fecha = DateTime.UtcNow.ToString("yyMMddmmss");
            //yyyy-MM-ddTHH:mm:ss.sss+00
            
            var archivo = $"{request.NumeroControl}.{fecha}.jpeg";
            var blobClient = containerClient.GetBlobClient(archivo);
           
           var res =  await blobClient.UploadAsync(request.Archivo, true);
           
           //if(res.Value.)

           mgest.FotoURL = blobClient.Uri.AbsoluteUri;

             estudiantes.ReplaceOne(est => est.Id == mgest.Id,
                mgest
              );

            return true;
        }
    }

    

}