
using System;
using System.Collections.Generic;
using System.Linq;

namespace aspnetdemo2 {
    public interface IEstudiantesRespository
    {
        void ActualizarEstudiante(string numeroControl, string nombre, string carrera);
        void BorrarEstudiante(string numeroControl);
        void CrearEstudiante(Estudiante estudiante);
        Estudiante LeerPorNC(string nc);
        List<Estudiante> LeerTodos();
    }

    public class EstudiantesRespository : IEstudiantesRespository
    {

        public EstudiantesRespository()
        {
            
        }
        private static List<Estudiante> db = new List<Estudiante>();

        static EstudiantesRespository()
        {
            db.Add(
                new Estudiante()
                {
                    NumeroControl = "12130003",
                    Nombre = "Estudiante 1",
                    Carrera = "Sistemas"
                }
            );
            db.Add(
                new Estudiante()
                {
                    NumeroControl = "12130004",
                    Nombre = "Estudiante 2",
                    Carrera = "Sistemas"
                }
            );
            db.Add(
                new Estudiante()
                {
                    NumeroControl = "12130005",
                    Nombre = "Estudiante 3",
                    Carrera = "Sistemas"
                }
            );
        }

        public Estudiante LeerPorNC(string nc) =>
                 db.FirstOrDefault(e => e.NumeroControl == nc);

        public List<Estudiante> LeerTodos()
        {
            // var q = from e in db
            //         select e;
            //     return q.ToList();
            return db.Select(e => e).ToList();
        }

        public void CrearEstudiante(Estudiante estudiante)
        {
            db.Add(estudiante);
        }

        public void BorrarEstudiante(string numeroControl)
        {
            var e = LeerPorNC(numeroControl);
            if (e == null)
            {
                return;
            }

            db.Remove(e);
        }

        public void ActualizarEstudiante(string numeroControl, string nombre, string carrera)
        {
            var e = LeerPorNC(numeroControl);
            if (e == null)
            {
                return;
            }

            e.Carrera = carrera;
            e.Nombre = nombre;
        }
    }

    public class Estudiante {
        public string NumeroControl { get; set; }  
        public string Nombre { get; set; }
        public string Carrera { get; set; }
        public float Promedio  { get; set; }
    }
}