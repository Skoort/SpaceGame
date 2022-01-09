using SpaceGame.Weapons;
using SpaceGame.Weapons.Targeting;
using System.Linq;
using UnityEngine;

namespace SpaceGame
{
	public class LevelLoader : MonoBehaviour
    {
		[SerializeField] private Spaceship _playerSpaceshipComponent = default;
		[SerializeField] private HullIntegrity _playerHullIntegrity = default;
		[SerializeField] private WeaponsSystem _playerWeaponsSystem = default;
		[SerializeField] private FiringSystem[] _weapons = default;
		[SerializeField] private FiringSystem _robotTurret = default;
		[SerializeField] private FiringSystem[] _missiles = default;

		[SerializeField] private HullIntegrity _stationHullIntegrity = default;  // Probably unneeded, as we just restore its value each time.

		private void Start()
		{
			GameManager.Instance.SetNumberAliens(GameObject.FindObjectsOfType<Target>().Where(x => x.Team == Team.ALIENS).Count());

			//_playerWeaponsSystem.AssignTargetLeadAim(

			AssignWeapon();
			AssignRobot();
			AssignHealth();
			AssignMissiles();

			GameManager.Instance.IsMissionLoaded = true;
		}

		private void AssignWeapon()
		{
			var weapon = _weapons[(int)GameManager.Instance.State.WeaponUpgrade];
			_playerWeaponsSystem.AssignWeapon(weapon);
			_playerSpaceshipComponent.AssignGun(weapon);
		}

		private void AssignRobot()
		{
			if (GameManager.Instance.State.HasTurret)
			{
				_robotTurret.gameObject.SetActive(true);
			}
		}

		private void AssignHealth()
		{
			var currHealth = GameManager.Instance.State.PlayerHealth;
			_playerHullIntegrity.SetValues(currHealth);
		}

		private void AssignMissiles()
		{
			if (GameManager.Instance.State.HasRocket1)
			{
				_missiles[0].gameObject.SetActive(true);
			}
			if (GameManager.Instance.State.HasRocket2)
			{
				_missiles[1].gameObject.SetActive(true);
			}
			if (GameManager.Instance.State.HasRocket3)
			{
				_missiles[2].gameObject.SetActive(true);
			}
		}
	}
}
