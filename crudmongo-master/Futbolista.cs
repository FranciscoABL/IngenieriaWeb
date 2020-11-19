using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;


namespace aspnetdemo2 {


 public class Futbolista {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Nombre { get; set; }  
        public string Equipo { get; set; }
        public int Edad { get; set; }
        public float Precio  { get; set; }

       
        
        }

}