using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Weapons.Targeting;

namespace SpaceGame.Weapons.Turret
{ 
    public class PlayerTurret : Turret
    {
        [SerializeField] private TargetLead _targetLead = default;

        public override TargetLead TargetLead { get => _targetLead; set => _targetLead = value; }
    }
}
