using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Weapons
{
	public class WeaponsSystem : MonoBehaviour
    {
        //[SerializeField] private TextMeshPro _ammoLabel = default;

        [SerializeField] private FiringSystem _initalWeapon = default;

        private FiringSystem _currentWeapon;
        private Dictionary<Compartment, FiringSystem> _weaponsByCompartment;

        private void Awake()
        {
            _weaponsByCompartment = new Dictionary<Compartment, FiringSystem>();

            _currentWeapon = _initalWeapon;

            SetAmmoUi();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Started shooting!");
                _currentWeapon?.Fire();
            }
            else
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Debug.Log("Stopped shooting!");
                _currentWeapon?.StopFiring();
            }
            else
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Reloading!");
                _currentWeapon?.Reload();
                SetAmmoUi();
            }
        }

        public void Equip(FiringSystem weapon)
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.OnFire -= OnFire;
                _currentWeapon.OnOutOfAmmo -= OnOutOfAmmo;
            }

            _currentWeapon = weapon;
            _currentWeapon.OnFire += OnFire;
            _currentWeapon.OnOutOfAmmo += OnOutOfAmmo;

            SetAmmoUi();
        }

        private void OnOutOfAmmo()
        {
            Debug.Log("Out of ammo!");
            SetAmmoUi();
        }

        private void OnFire()
        {
            Debug.Log("Shot!");
            SetAmmoUi();
        }

        private void SetAmmoUi()
        {
            //if (_currentWeapon == null)
            //{
            //    _ammoLabel.text = "";
            //    return;
            //}

            //var text = $"Ammo: {_currentWeapon.Ammo}/{_currentWeapon.Weapon.MaxAmmo}";

            //_ammoLabel.text = text;
            //_ammoLabel.color = _currentWeapon.Ammo > 0
            //    ? Color.white
            //    : Color.red;
        }
    }
}
