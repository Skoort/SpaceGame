using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceGame.Ui
{
	public class PurchaseButton : MonoBehaviour
    {
        [SerializeField] private HangarTalkToGM _hangarUiMaster = default;

        [SerializeField] private int _purchasePrice = 100;
        [SerializeField] private string _currentName = "Current Name";
        [SerializeField] private string _upgradeName = "Upgrade Name";

        [SerializeField] private Button _thisButton = default;
        [SerializeField] private TextMeshProUGUI _upgradeInfoText = default;
        [SerializeField] private TextMeshProUGUI _weaponNameText = default;

        [SerializeField] private bool _isStub = false;
        [SerializeField] private PurchaseButton _nextUpgrade = default;

        private void OnEnable()
        {
            if (_isStub)
            {
                DisableButton();
            }
            else
            { 
                Refresh();

                _hangarUiMaster.OnPurchaseMade += Refresh;
            }
        }

		private void OnDisable()
		{
            if (!_isStub)
            { 
                _hangarUiMaster.OnPurchaseMade -= Refresh;
            }
		}

        private void Refresh()
        {
            if (_weaponNameText != null)
            { 
                _weaponNameText.text = _currentName;
            }

            _upgradeInfoText.text = $"${_purchasePrice} {_upgradeName}";
            
            if (_isStub)
            {
                DisableButton();
                return;
            }

            var totalCredits = GameManager.Instance.State.TotalCredits;
            if (totalCredits > _purchasePrice)
            {
                EnableButton();
            } else
            if (totalCredits <= _purchasePrice)
            {
                DisableButton();
            }
        }

        private void DisableButton()
        {
            if (_isStub)
            {
                _thisButton.gameObject.SetActive(false);
            }
            else
            {
                _upgradeInfoText.color = Color.red;
                _thisButton.interactable = false;
            }
        }

        private void EnableButton()
        {
            _thisButton.interactable = true;
            _upgradeInfoText.color = Color.black;
        }

		public void Purchase()
        {
            Debug.Assert(!_isStub, $"Somehow pressed purchase on a stub button!");
            Debug.Assert(GameManager.Instance.State.TotalCredits >= _purchasePrice, $"Attempted to purchase item {this.gameObject.name} even though you don't have enough credits!");

            GameManager.Instance.State.TotalCredits -= _purchasePrice;

            DoUpgrade();

            _nextUpgrade.gameObject.SetActive(true);
            this.gameObject.SetActive(false);

            _hangarUiMaster.MakePurchase();
        }

        protected void DoUpgrade()
        {
            if (_upgradeName == Strings.Weapon2Upgrade)
            {
                GameManager.Instance.State.WeaponUpgrade = GameState.WeaponUpgradeState.WEAPON2;
            } else
            if (_upgradeName == Strings.Weapon3Upgrade)
            {
                GameManager.Instance.State.WeaponUpgrade = GameState.WeaponUpgradeState.WEAPON3;
            } else
            if (_upgradeName == Strings.Weapon4Upgrade)
            {
                GameManager.Instance.State.WeaponUpgrade = GameState.WeaponUpgradeState.WEAPON4;
            } else
            if (_upgradeName == Strings.TurretUpgrade)
            {
                GameManager.Instance.State.HasTurret = true;
            } else
            if (_upgradeName == Strings.RepairHullDamageUpgrade)
            {
                GameManager.Instance.State.PlayerHealth = GameManager.Instance.MaxPlayerHealth;
            } else
            if (_upgradeName == Strings.PurchaseMissile)
            {
                var freeSpot = GameManager.Instance.State.GetFirstEmptyRocketIndex();
                Debug.Assert(freeSpot >= 0, "Attempted to add more missiles than there is room for!");
                GameManager.Instance.State.SetRocketState(freeSpot, true);
            }
            else
            {
                Debug.LogError($"Made a purchase but couldn't find upgrade called {_upgradeName}!");
            }
        }
	}
}
