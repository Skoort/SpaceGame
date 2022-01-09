using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceGame.Ui
{
    public class HangarTalkToGM : TalkToGM
    {
		[SerializeField] private PurchaseButton[] _weaponButtons = default;  // Upgrade 2/3/4 & "You already have this upgrade."
		[SerializeField] private PurchaseButton[] _turretButtons = default;  // Upgrade Turret & "You already have this upgrade."
		[SerializeField] private PurchaseButton _upgradeHealthButton = default;  // Heal health & "Hull fully restored."
		[SerializeField] private PurchaseButton[] _purchaseMissilesButtons = default;  // Buy 1/3, 2/3, 3/3 & "You do not have any more capacity."

		protected override void Start()
		{
			base.Start();

			LoadWeapons();
			LoadTurret();
			LoadHealth();
			LoadRockets();
		}

		private void LoadWeapons()
		{
			var activeButton = _weaponButtons[(int)GameManager.State.WeaponUpgrade];
			activeButton.gameObject.SetActive(true);
		}

		private void LoadTurret()
		{
			var hasTurret = GameManager.State.HasTurret;
			if (!hasTurret)
			{
				_turretButtons[0].gameObject.SetActive(true);
			}
			else
			{
				_turretButtons[1].gameObject.SetActive(true);
			}
		}

		private void LoadHealth()
		{
			var _hasTurret = GameManager.State.PlayerHealth == GameManager.MaxPlayerHealth;
			if (!_hasTurret)
			{
				_turretButtons[0].gameObject.SetActive(true);
			}
			else
			{
				_turretButtons[1].gameObject.SetActive(true);
			}
		}

		private void LoadRockets()
		{
			var numRockets = GameManager.State.GetNumberOfRockets();
			var rocketButton = _purchaseMissilesButtons[numRockets];
			rocketButton.gameObject.SetActive(true);
		}

		public void MakePurchase()
		{
			OnPurchaseMade?.Invoke();
		}

		public event Action OnPurchaseMade;
	}
}