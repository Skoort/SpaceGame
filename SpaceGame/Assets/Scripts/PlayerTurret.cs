using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Weapons.Targeting;

namespace SpaceGame
{ 
    public class PlayerTurret : Turret
    {
        [SerializeField] private TargetLead _targetLead = default;

        protected override TargetLead TargetLead { get => _targetLead; set => _targetLead = value; }
    }
}
