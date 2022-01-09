using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceGame.Ui
{
	public class PurchaseButton : MonoBehaviour
    {
        [SerializeField] private HangarTalkToGM _hangarUiMaster = default;

        [SerializeField] private Button _thisButton = default;
        [SerializeField] private PurchaseButton _nextUpgrade = default;
        [SerializeField] private int _purchasePrice = 100;

        [SerializeField] private TextMeshProUGUI _hoverText = default;
        [SerializeField] private TextMeshProUGUI _buttonText = default;
        [SerializeField] private string _disabledText = "Insufficient Funds";  // Either "Insufficient Funds" or "Already Unlocked".
        [SerializeField] private string _onHoverText = "Purchase";

        [SerializeField] private bool _isStub = false;

        private void OnEnable()
        {
            if (_isStub)
            {
                _thisButton.enabled = true;
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
            if (_isStub && _thisButton.isActiveAndEnabled)
            {
                DisableButton();
                return;
            }

            var totalCredits = GameManager.Instance.State.TotalCredits;
            if (totalCredits > _purchasePrice && _thisButton.isActiveAndEnabled)
            {
                DisableButton();
            } else
            if (totalCredits <= _purchasePrice && !_thisButton.isActiveAndEnabled)
            {
                EnableButton();
            }
        }

        private void DisableButton()
        {
            _hoverText.gameObject.SetActive(true);
            _hoverText.text = _disabledText;

            _thisButton.enabled = false;
        }

        private void EnableButton()
        {
            _hoverText.gameObject.SetActive(false);

            _thisButton.enabled = true;
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
            var buttonText = _buttonText.text;
            if (buttonText == Strings.Weapon2Upgrade)
            {
                GameManager.Instance.State.WeaponUpgrade = GameState.WeaponUpgradeState.WEAPON2;
            } else
            if (buttonText == Strings.Weapon3Upgrade)
            {
                GameManager.Instance.State.WeaponUpgrade = GameState.WeaponUpgradeState.WEAPON3;
            } else
            if (buttonText == Strings.Weapon4Upgrade)
            {
                GameManager.Instance.State.WeaponUpgrade = GameState.WeaponUpgradeState.WEAPON4;
            } else
            if (buttonText == Strings.TurretUpgrade)
            {
                GameManager.Instance.State.HasTurret = true;
            } else
            if (buttonText == Strings.RepairHullDamageUpgrade)
            {
                GameManager.Instance.State.PlayerHealth = GameManager.Instance.MaxPlayerHealth;
            }
            else
            {
                Debug.LogError($"Made a purchase but couldn't find upgrade called {buttonText}!");
            }
        }
	}
}
