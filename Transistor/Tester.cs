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
            bf.Show(TurnMode.normal);

        }
    }
}
