using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Coor
    {
        public static Coor ZERO = new Coor(0, 0);
        public static Coor LEFT = new Coor(0, -1);
        public static Coor RIGHT = new Coor(0, 1);
        public static Coor UP = new Coor(-1, 0);
        public static Coor DOWN = new Coor(1, 0);

        // fila y columna (como propiedades)
        public int row { get; set; }
        public int col { get; set; }

        public Coor(int _row = 0, int _col = 0)
        {
            row = _row;
            col = _col;
        }

        public int X_Distance(Coor c)
        {
            return Math.Abs(this.col - c.col);
        }
        
        public int Y_Distance(Coor c)
        {
            return Math.Abs(this.row - c.row);
        }

        public int Distance(Coor c)
        {
            return X_Distance(c) + Y_Distance(c);
        }

        public bool Aligned(Coor c)
        {
            return col == c.col || row == c.row;
        }


        // sobrecarga de + y - para hacer "desplazamientos" con coordenadas
        public static Coor operator +(Coor c1, Coor c2)
        {
            if (c1 is null)
                return c2;
            else if (c2 is null)
                return c1;
            else
                return new Coor(c1.row + c2.row, c1.col + c2.col);
        }

        public static Coor operator -(Coor c1, Coor c2)
        {
            if (c1 is null)
                return -c2;
            else if (c2 is null)
                return c1;
            else
                return new Coor(c1.row - c2.row, c1.col - c2.col);
        }

        public static Coor operator -(Coor c)
        {
            return new Coor(-c.row, -c.col);
        }

        // sobrecarga de los operadores == y != para comparar coordenadas mediante fil y col
        public static bool operator ==(Coor c1, Coor c2)
        {
            // Añadimos igualación a null para PersonajeValido()
            if (c1 is null || c2 is null)
                return c1 is null && c2 is null;
            else
                return c1.row == c2.row && c1.col == c2.col;
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
