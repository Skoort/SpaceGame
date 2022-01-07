using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Weapons.Targeting;

namespace SpaceGame.Ai.Ship
{ 
    public interface IShipAi
    {
        Transform Transform { get; }
        Vector3? TargetPosition { get; set; }
        TargetLead CurrentTarget { get; set; }

        void MoveForward(float thrust);
        void TurnTowards(Vector3 heading);
    }
}
