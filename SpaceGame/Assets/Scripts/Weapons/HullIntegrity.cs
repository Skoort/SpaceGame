using SpaceGame.Weapons.Targeting;
using System;
using UnityEngine;

namespace SpaceGame
{
	public class HullIntegrity : MonoBehaviour
    {
        [SerializeField] private float _currValue = 100;
        [SerializeField] private float _maxValue = 100;

        [SerializeField] private Camera[] _cameras = default;

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
            if (Value <= 0)
            {
                return;  // We're already dead and just haven't disappeared yet.
            }

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
                    if (gameObject.tag == Strings.PlayerTag)
                    {
                        foreach (var camera in _cameras)
                        {
                            camera.transform.SetParent(parent: null, worldPositionStays: true);
                        }

                        GameManager.Instance.LoadScoreScreenGameOver();
                    } else
                    if (gameObject.tag == Strings.SpaceStationTag)
                    {
                        GameManager.Instance.LoadScoreScreenGameOver();
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
