using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    class Tester
    {
        public void TestBattlefieldLoadLevel()
        {
            Battlefield bf = new Battlefield("Transistor.txt");
            
        }
        public void TestBattlefieldShowNormal()
        {
            Battlefield bf = new Battlefield("Transistor.txt");
            //bf.Show(TurnMode.Normal);

        }
        public void TestList()
        {
            List<Character> lista = new List<Character>();
            foreach( Character c in lista)
            {

            }
        }
    }
}
