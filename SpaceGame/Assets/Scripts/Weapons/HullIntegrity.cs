using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{ 
    public class HullIntegrity : MonoBehaviour
    {
        [SerializeField] private float _currValue = 100;
        [SerializeField] private float _maxValue = 100;

        [SerializeField] private UnityEvent _onChanged = default;
        [SerializeField] private UnityEvent _onDeath = default;

        public void TakeDamage(float damage, GameObject source)
        {
            Debug.Assert(damage > 0, "Attempted to deal negative damage!");

            _currValue = Mathf.Clamp(_currValue - damage, 0, _maxValue);
            _onChanged?.Invoke();

            if (_currValue == 0)
            {
                _onDeath?.Invoke();
                if (source.tag == Strings.PlayerTag)
                {
                    GameManager.Instance.OnPlayerGotKill();
                }
                Destroy(this.gameObject);
            }
        }

        public void RestoreDamage()
        {
            _currValue = _maxValue;
            _onChanged?.Invoke();
        }
    }
}
