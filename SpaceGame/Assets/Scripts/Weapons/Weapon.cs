using SpaceGame.Weapons.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Weapons
{
    public enum Compartment
    { 
        FORE,
        DECK,
        SIDES
    }

    // Consider refactoring this into Weapon, ProjectileWeapon, EmitterWeapon & HitscanWeapon. Differentiation between single-shot and automatic weapons is handled by the FireController class (What about burst weapons?). Need to look up how to serialize polymorphic data. Look into SerializedReference.
    [CreateAssetMenu(menuName = "Weapons/Weapon")]
    public class Weapon : ScriptableObject
    {
        public string Name;

        public Compartment Compartment;

        public int MaxAmmo;

        public int ReloadTime;

        public float MinDamage;
        public float MaxDamage;

        public bool IsAutomatic;

        [Tooltip(tooltip: "The number of shots per second. Only necessary if IsAutomatic == true.")]
        public float RateOfFire;

        public bool IsHitscan;
        [Tooltip(tooltip: "Only necessary if IsHitscan == false.")]
        public Projectile ProjectilePrefab;

        public float Range;

        public float Spread;
    }
}
