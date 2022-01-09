using SpaceGame.Weapons.Targeting;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceGame
{
    [System.Serializable]
    public class GameState
    {
        public enum WeaponUpgradeState
        {
            WEAPON1,
            WEAPON2,
            WEAPON3,
            WEAPON4
        }

        [SerializeField] private int _missionId = 0;
        [SerializeField] private int _totalCredits = 0;
        [SerializeField] private int _creditsThisMission = 0;
        [SerializeField] private float _playerHealth = 0;
        [SerializeField] private float _stationHealth = 0;
        [SerializeField] private int _playerKillCount = 0;
        [SerializeField] private int _allyDeathCount = 0;
        [SerializeField] private WeaponUpgradeState _weaponUpgrade = WeaponUpgradeState.WEAPON1;
        [SerializeField] private bool _hasTurret = false;
        [SerializeField] private bool _hasRocket1 = false;
        [SerializeField] private bool _hasRocket2 = false;
        [SerializeField] private bool _hasRocket3 = false;

        public int MissionId { get => _missionId; set => _missionId = value; }
        public int TotalCredits { get => _totalCredits; set => _totalCredits = value; }
        public int CreditsThisMission { get => _creditsThisMission; set => _creditsThisMission = value; }
        public int PlayerKillCount { get => _playerKillCount; set => _playerKillCount = value; }
        public int AllyDeathCount { get => _allyDeathCount; set => _allyDeathCount = value; }
        public float StationHealth { get => _stationHealth; set => _stationHealth = value; }
        public float PlayerHealth { get => _playerHealth; set => _playerHealth = value; }
        public WeaponUpgradeState WeaponUpgrade { get => _weaponUpgrade; set => _weaponUpgrade = value; }
        public bool HasTurret { get => _hasTurret; set => _hasTurret = value; }
        public bool HasRocket1 { get => _hasRocket1; set => _hasRocket1 = value; }
        public bool HasRocket2 { get => _hasRocket2; set => _hasRocket2 = value; }
        public bool HasRocket3 { get => _hasRocket3; set => _hasRocket3 = value; }

        public int GetFirstAvailableRocketIndex(int i)
        {
            if (_hasRocket1)
            {
                return 0;
            } else
            if (_hasRocket2)
            {
                return 1;
            } else
            if (_hasRocket3)
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

        public int GetFirstMissingRocketIndex(int i)
        {
            if (!_hasRocket1)
            {
                return 0;
            } else
            if (!_hasRocket2)
            {
                return 1;
            } else
            if (!_hasRocket3)
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }
    }

	public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);

                Instance = this;

                Targets = new List<Target>();
            }
            else
            {
                Debug.LogError($"Attempted to make another GameManager instance {this.name}!");
                Destroy(this);
            }
        }

        [SerializeField] private HullIntegrity _playerHullIntegrity = default;
        [SerializeField] private HullIntegrity _stationHullIntegrity = default;

        [SerializeField] private GameState _state = default;
        public GameState State => _state;

        public void IncrementPlayerKillCount()
        {
            ++State.PlayerKillCount;
        }

        public void IncrementAllyDeathCount()
        {
            ++State.AllyDeathCount;
        }

        private float _savedPlayerHealth;
        private bool _savedHasRocket1;
        private bool _savedHasRocket2;
        private bool _savedHasRocket3;
        public void SetGameOver()
        {
            State.PlayerHealth = _savedPlayerHealth;
            State.HasRocket1 = _savedHasRocket1;
            State.HasRocket2 = _savedHasRocket2;
            State.HasRocket3 = _savedHasRocket3;

            // Show game over screen with option to retry level or exit to main menu.
        }

        // Use this to load scenes that are missions.
        public void LoadMission(int index)
        {
            _savedPlayerHealth = State.PlayerHealth;
            _savedHasRocket1 = State.HasRocket1;
            _savedHasRocket2 = State.HasRocket2;
            _savedHasRocket3 = State.HasRocket3;

            State.MissionId = index;

            var sceneName = $"Mission {State.MissionId }";
            // Load the scene with this name.
        }

        // Use this to load scenes that aren't missions (menu, options, game over, etc.).
        public void LoadLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);   
        }

        public List<Target> Targets { get; private set; }

        public event Action<Target> OnTargetAdded;
        public event Action<Target> OnTargetRemoved;

        public void RegisterTarget(Target target)
        {
            Targets.Add(target);
            OnTargetAdded?.Invoke(target);
        }

        public void UnregisterTarget(Target target)
        {
            Targets.Remove(target);
            OnTargetRemoved?.Invoke(target);
        }
    }
}
