using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai.Ship
{ 
    public interface IShipAi
    {
        Transform Transform { get; }
        Vector3? Target { get; set; }

        void MoveForward(float thrust);
        void TurnTowards(Vector3 heading);
    }
}
