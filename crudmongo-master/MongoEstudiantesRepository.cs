using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace aspnetdemo2 {


    public class MongoEstudiantesRespository : IEstudiantesRespository
    {
        private readonly IMongoCollection<MongoEstudiante> estudiantes;

        public MongoEstudiantesRespository(IEstudiantesDatabaseSettings settings)
        {
            var cliente = new MongoClient(settings.ConnectionString);
            var db = cliente.GetDatabase(settings.DatabaseName);

            estudiantes = db.GetCollection<MongoEstudiante>(settings.CollectionName);
        }

        public void ActualizarEstudiante(string numeroControl, string nombre, string carrera)
        {
            var mgest = estudiantes
                    .Find<MongoEstudiante>( est => est.NumeroControl == numeroControl)
                    .FirstOrDefault();
            mgest.Nombre = nombre;
            mgest.Carrera = carrera;
             estudiantes.ReplaceOne(est => est.Id == mgest.Id,
                mgest
              );
            
        }

        public void BorrarEstudiante(string numeroControl)
        {
            estudiantes
                    .DeleteOne( est => est.NumeroControl == numeroControl);
        }

        public void CrearEstudiante(Estudiante estudiante)
        {
            var mgEstudiante = new MongoEstudiante() {
                Nombre = estudiante.Nombre,
                NumeroControl = estudiante.NumeroControl,
                Promedio = estudiante.Promedio,
                Carrera = estudiante.Carrera
            };

            estudiantes.InsertOne(mgEstudiante);
        }

        public Estudiante LeerPorNC(string nc)
        {
            var mgest = estudiantes
                    .Find<MongoEstudiante>( est => est.NumeroControl == nc)
                    .FirstOrDefault();
            return new Estudiante() {
                NumeroControl = mgest.NumeroControl,
                Nombre = mgest.Nombre,
                Carrera = mgest.Carrera,
                Promedio = mgest.Promedio
            };
        }

        public List<Estudiante> LeerTodos()
        {
            var resultado = estudiantes.Find<MongoEstudiante>(est => true);
            var res = resultado.ToList().Select(est => 
                    new Estudiante() {
                        NumeroControl = est.NumeroControl,
                        Nombre = est.Nombre,
                        Carrera = est.Carrera,
                        Promedio = est.Promedio
                    }
            );

            return res.ToList();
            
        }

        public class MongoEstudiante {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string NumeroControl { get; set; }  
        public string Nombre { get; set; }
        public string Carrera { get; set; }
        public float Promedio  { get; set; }

        public string FotoURL { get; set; }
        
        }
    }

    public class EstudiantesDatabaseSettings  : IEstudiantesDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IEstudiantesDatabaseSettings {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }



}