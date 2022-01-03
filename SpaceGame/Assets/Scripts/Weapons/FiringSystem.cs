using System;
using UnityEngine;

namespace SpaceGame.Weapons
{
	public abstract class FiringSystem : MonoBehaviour
    {
        [SerializeField] public bool IsEquipped = default;  // Determines if the weapon is physically on the character, as opposed to able to be picked up.

        [SerializeField] protected Transform Target = default;  // If the Target is null, shoot straight forward.

        [SerializeField] protected Transform[] Origins = default;

        [SerializeField] private Weapon _weapon = default;
        public Weapon Weapon => _weapon;

        [SerializeField] protected LayerMask HitLayer = default;

        [SerializeField] protected bool IsShotReadyOnSwap = default;

        // Consider making these serializable UnityEvents for animations, muzzle flashes and other effects.
        public event Action OnOutOfAmmo;
        public event Action OnFire;

        // I envision this being used to make a gun glow with energy when it is ready to fire, but this is probably better done with animation events.
        public event Action OnNextShotProgressAt50;
        public event Action OnNextShotReady;

        public bool IsFiring { get; protected set; }

        public int Ammo { get; protected set; }

        public float NextShotProgress { get; protected set; } = 1;

        [SerializeField] private float _nextShotInputLeeway = 0;

        public void Fire()
        {
            IsFiring = true;

            TryToFire();  // Always fire the first shot during the first frame, unless the weapon is not ready to fire yet.

            if (!Weapon.IsAutomatic && (1 - NextShotProgress) > _nextShotInputLeeway)
            {
                IsFiring = false;
            }
        }

        // Use this method to stop firing an automatic weapon.
        public void StopFiring()
        {
            IsFiring = false;
        }

        protected bool TryToFire()
        {
            if (!IsFiring || NextShotProgress < 1)
            {
                return false;
            }

            if (Ammo <= 0)
            {
                OnOutOfAmmo?.Invoke();
                return false;
            }

            DoFire();

            Ammo -= 1;
            //NextShotProgress -= 1;
            NextShotProgress = 0;  // This is a temporary fix to the problem of if you don't shoot for a while, NextShotProgress may be very high and make you shoot every frame.
            OnFire?.Invoke();

            return true;
        }

        protected abstract void DoFire();

        public void Reload()
        {
            Ammo = Weapon.MaxAmmo;
        }

        private void Update()
        {
            NextShotProgress += Time.deltaTime * Weapon.RateOfFire;

            TryToFire();
        }

        private void OnEnable()
        {
            if (IsShotReadyOnSwap)
            {
                NextShotProgress = 1;  // Should add a new property, HasLongNextShotDurations, which prevents swapping weapons to fire faster.
            }
            else
            {
                NextShotProgress = 0;
            }
        }

        private void OnDisable()
        {
            StopFiring();
        }
    }
}
