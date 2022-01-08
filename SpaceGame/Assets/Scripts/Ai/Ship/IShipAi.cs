using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Weapons.Targeting;

namespace SpaceGame.Ai.Ship
{
    public enum ShipState
    { 
        IDLE,
        PURSUING_TARGET,
        GOING_TO_POINT
    }

    public interface IShipAi
    {
        ShipState State { get; set; }   

        Transform Transform { get; }
        float StoppingRange { get; }
        Vector3? TargetPosition { get; }
        Vector3? TargetWanderPoint { get; set; }
        TargetLead TargetLead { get; set; }

        void MoveForward(float thrust);
        void TurnTowards(Vector3 heading);
    }
}
