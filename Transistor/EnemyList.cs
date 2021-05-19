using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class EnemyList
    {
        private class Nodo
        {
            public Enemy dato;
            public Nodo sig;
        }

        Nodo pri;

        public EnemyList()
        {
            pri = null;
        }

        public void InsertaFinal(Enemy e)
        {
            // Si la lista está vacía
            if (pri == null)
            {
                pri = new Nodo(); // Creamos un nodo en pri
                pri.dato = e;
                pri.sig = null;
            }
            else // Lista no vacía
            {
                // Recorremos la lista hasta el último nodo
                Nodo aux = pri;
                while (aux.sig != null)
                {
                    aux = aux.sig;
                }

                // aux apunta al último nodo
                aux.sig = new Nodo();
                aux = aux.sig;
                aux.dato = e;
                aux.sig = null;
            }
        }

        private Nodo nEsimoNodo(int n)
        {
            if (n < 0)
            {
                throw new Exception("Error: Intentando acceder a un índice negativo.");
            }
            else
            {
                Nodo nEsimo = pri;

                int i = 0;
                while (nEsimo != null && i < n)
                {
                    i++;
                    nEsimo = nEsimo.sig;
                }
                return nEsimo;
            }
        }

        public Enemy nEsimo(int n)
        {
            Nodo aux = nEsimoNodo(n);

            if (aux == null)
                throw new Exception("Error: Índice fuera de los límites de la lista");
            else
                return aux.dato;
        }

        public bool BorraElto(Enemy e)
        {
            Nodo aux = pri;
            Nodo prev = null;

            while (aux.dato != e && aux.sig != null)
            {
                prev = aux;
                aux = aux.sig;
            }

            if (aux.dato == e)
            {
                if (prev != null)
                    prev.sig = aux.sig;
                else
                    pri = aux.sig;
                return true;
            }
            else if (aux.sig == null)
            {
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
