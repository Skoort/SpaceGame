using SpaceGame.Weapons.Targeting;
using System;
using UnityEngine;

namespace SpaceGame
{
	public class HullIntegrity : MonoBehaviour
    {
        [SerializeField] private float _currValue = 100;
        [SerializeField] private float _maxValue = 100;

        public float Value => _currValue;
        public float MaxValue => _maxValue;

        public Action OnChanged = default;
        public Action OnDeath = default;

        private Team _team;

		private void Start()
		{
            _team = GetComponentInChildren<Target>().Team;  // The space station has multiple targets. Just grab one and use that.
		}

		public void TakeDamage(float damage, GameObject source)
        {
            Debug.Assert(damage > 0, "Attempted to deal negative damage!");

            _currValue = Mathf.Clamp(_currValue - damage, 0, _maxValue);
            OnChanged?.Invoke();

            if (_currValue == 0)
            {
                OnDeath?.Invoke();

                if (_team == Team.ALIENS)
                {
                    GameManager.Instance.IncrementAlienDeathCount();
                    if (source.tag == Strings.PlayerTag)
                    { 
                        GameManager.Instance.IncrementPlayerEnemyKillCount();
                    }
                } else
                if (_team == Team.HUMANS)
                {
                    if (gameObject.tag == Strings.PlayerTag || gameObject.tag == Strings.SpaceStationTag)
                    {
                        GameManager.Instance.LoadHangarGameOver();
                    }
                    else
                    {
                        GameManager.Instance.IncrementAllyDeathCount();
                        if (source.tag == Strings.PlayerTag)
                        { 
                            GameManager.Instance.IncrementPlayerAllyKillCount();
                        }
                    }
                }

                Destroy(this.gameObject);
            }
        }

        public void RestoreDamage()
        {
            _currValue = _maxValue;
            OnChanged?.Invoke();
        }

        public void SetValues(float value, float? maxValue = null)
        {
            _currValue = value;
            if (maxValue != null)
            { 
                _maxValue = maxValue.Value;
            }
            OnChanged?.Invoke();
        }
    }
}
