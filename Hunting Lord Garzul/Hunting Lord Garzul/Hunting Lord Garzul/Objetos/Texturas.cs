using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Hunting_Lord_Garzul.Objetos
{
    public class Texturas
    {
        // La textura a utilizar
        private Texture2D Textura;
        public Texture2D textura
        {
            get { return Textura; }
            set { Textura = value; }
        }

        // Nombre completo de la textura
        private String Nombre_textura;
        public String nombre_textura
        {
            get { return Nombre_textura; }
            set { Nombre_textura = value; }
        }
        
        // Armadura a la que pertenece esta pieza
        private String Armadura;
        public String armadura
        {
            get { return Armadura; }
        }

        // Accion que realiza esta pieza
        private String Accion;
        public String accion
        {
            get { return Accion; }
        }

        // Pieza que representa la textura
        private String Pieza;
        public String pieza
        {
            get { return Pieza; }
        }

        // Constructor del objeto
        public Texturas(Texture2D textura, String nombre)
        {
            Textura = textura;
            Nombre_textura = nombre;

            string[] separador = nombre.Split('_');
            Armadura = separador[0];
            Accion = separador[1];
            Pieza = separador[2];
        }
    }
}
