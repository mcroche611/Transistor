using System;
using System.Collections.Generic;
using System.Text;

namespace Transistor
{
    enum TurnMode { normal, plan, run }

    class Transistor
    {
        TurnMode mode;

        EnemyList enemies;
        Battlefield field;
        Player red;

        void Run()
        {
            field = new Battlefield("nivel1");
            red = new Player(field, new Coor());
            enemies = new EnemyList();

        }

        void ReadInput(TurnMode mode)
        {

        }

        private void ReadInputBattle()
        {

        }

        private void ReadInputTurn()
        {

        }

        void ProcessInput()
        {

        }

        private void ProcessInputBattle()
        {

        }

        private void ProcessInputTurn()
        {

        }
    }
}
