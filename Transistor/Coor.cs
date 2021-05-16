using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Coor
    {
        // fila y columna (como propiedades)
        public int fil { get; set; }
        public int col { get; set; }

        public Coor(int _fil = 0, int _col = 0)
        {
            fil = _fil;
            col = _col;
        }

        // sobrecarga de + y - para hacer "desplazamientos" con coordenadas
        public static Coor operator +(Coor c1, Coor c2)
        {
            return new Coor(c1.fil + c2.fil, c1.col + c2.col);
        }

        public static Coor operator -(Coor c)
        {
            return new Coor(-c.fil, -c.col);
        }

        // sobrecarga de los operadores == y != para comparar coordenadas mediante fil y col
        public static bool operator ==(Coor c1, Coor c2)
        {
            // Añadimos igualación a null para PersonajeValido()
            if (c1 is null || c2 is null)
                return c1 is null && c2 is null;
            else
                return c1.fil == c2.fil && c1.col == c2.col;
        }

        public static bool operator !=(Coor c1, Coor c2)
        {
            //public bool Equals(Coor c){
            return !(c1 == c2);
        }

        public override bool Equals(object c)
        {
            return (c is Coor) && this == (Coor)c;
        }

        // It was giving me an error 
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
