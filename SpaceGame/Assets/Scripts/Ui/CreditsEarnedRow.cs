using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpaceGame.Ui
{ 
    public class CreditsEarnedRow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _reasonText = default;
        [SerializeField] private TextMeshProUGUI _amountOfCashText = default;

        public CreditsEarnedRow SetReason(string reason)
        {
            _reasonText.text = reason;
            return this;
        }

        private int _amount;
        public CreditsEarnedRow SetAmount(int amount)
        {
            _amount = amount;
            if (_amount >= 0)
            {
                _amountOfCashText.text = $"${_amount}";
            }
            else
            {
                _amountOfCashText.text = $"-${Mathf.Abs(_amount)}";
            }
            return this;
        }

        public int GetAmount()
        {
            return _amount;
        }
    }
}
