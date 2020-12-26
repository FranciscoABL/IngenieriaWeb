using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;


namespace aspnetdemo2 {


 public class Silla {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int NumeroDePatas { get; set; }  
        public string TipoDeMaterial { get; set; }
        public bool DescansaBrazo  { get; set; }

        }

}