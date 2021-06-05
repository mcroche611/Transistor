﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class EnemyList
    {
        private class Node
        {
            public Enemy data;
            public Node next;
        }

        Node first;

        public EnemyList()
        {
            first = null;
        }

        public void Append(Enemy e)
        {
            // Si la lista está vacía
            if (first == null)
            {
                first = new Node(); // Creamos un nodo en pri
                first.data = e;
                first.next = null;
            }
            else // Lista no vacía
            {
                // Recorremos la lista hasta el último nodo
                Node aux = first;
                while (aux.next != null)
                {
                    aux = aux.next;
                }

                // aux apunta al último nodo
                aux.next = new Node();
                aux = aux.next;
                aux.data = e;
                aux.next = null;
            }
        }

        private Node nEsimoNodo(int n)
        {
            if (n < 0)
            {
                throw new Exception("Error: Intentando acceder a un índice negativo.");
            }
            else
            {
                Node nEsimo = first;

                int i = 0;
                while (nEsimo != null && i < n)
                {
                    i++;
                    nEsimo = nEsimo.next;
                }
                return nEsimo;
            }
        }

        public Enemy nEsimo(int n)
        {
            Node aux = nEsimoNodo(n);

            if (aux == null)
                throw new Exception("Error: Índice fuera de los límites de la lista");
            else
                return aux.data;
        }

        public bool Delete(Enemy e)
        {
            Node aux = first;
            Node ant = null;

            while (aux.data != e && aux.next != null)
            {
                ant = aux;
                aux = aux.next;
            }

            if (aux.data == e)
            {
                if (ant != null)
                    ant.next = aux.next;
                else
                    first = aux.next;
                return true;
            }
            else if (aux.next == null)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        public void BorraEliminados()
        {
            Node aux = first;
            Node prev = null;

            while (aux != null)
            {
                if (aux.data.Destroyed)
                {
                    if (prev != null)
                    {
                        prev.next = aux.next;
                    }
                    else
                    {
                        first = aux.next;
                    }
                }
                else
                {
                    prev = aux;
                }
                aux = aux.next;
            }
        }

        public int Count()
        {
            int numElts = 0;

            if (first != null)
            {
                //Primer nodo
                numElts++;

                //Recorre el resto de nodos
                Node aux = first;
                while (aux.next != null)
                {
                    aux = aux.next;
                    numElts++;
                }
            }

            return numElts;
        }

        public bool IsEnemy(Coor pos)
        {
            Node aux = first;
            bool found = false;

            while (aux != null && !found)
            {
                if (aux.data.Pos != pos)
                    aux = aux.next;
                else
                    found = true;
            }

            return found;
        }

        public Enemy GetEnemyInPos(Coor pos)
        {
            Node aux = first;
            Enemy enemy = null;

            while (aux != null && enemy == null)
            {
                if (aux.data.Pos != pos)
                    aux = aux.next;
                else
                    enemy = aux.data;
            }

            return enemy;
        }
    }
}
