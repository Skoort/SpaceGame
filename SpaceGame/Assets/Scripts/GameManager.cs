using SpaceGame.Pooling;
using SpaceGame.Weapons.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public enum HangarArrivalState
        { 
            FAILURE,
            SUCCESS,
            MENU
        }

        [SerializeField] private int _missionId = 0;
        [SerializeField] private int _totalCredits = 0;
        [SerializeField] private int _creditsThisMission = 0;
        [SerializeField] private float _playerHealth = 500;
        [SerializeField] private float _stationHealth = 0;
        [SerializeField] private int _playerEnemyKillCount = 0;
        [SerializeField] private int _playerAllyKillCount = 0;
        [SerializeField] private int _allyDeathCount = 0;
        [SerializeField] private WeaponUpgradeState _weaponUpgrade = WeaponUpgradeState.WEAPON1;
        [SerializeField] private bool _hasTurret = false;
        [SerializeField] private bool _hasRocket1 = false;
        [SerializeField] private bool _hasRocket2 = false;
        [SerializeField] private bool _hasRocket3 = false;
        [SerializeField] private HangarArrivalState _hangarState = HangarArrivalState.MENU;

        public int MissionId { get => _missionId; set => _missionId = value; }
        public int TotalCredits { get => _totalCredits; set => _totalCredits = value; }
        public int CreditsThisMission { get => _creditsThisMission; set => _creditsThisMission = value; }
        public int PlayerEnemyKillCount { get => _playerEnemyKillCount; set => _playerEnemyKillCount = value; }
        public int PlayerAllyKillCount { get => _playerAllyKillCount; set => _playerAllyKillCount = value; }
        public int AllyDeathCount { get => _allyDeathCount; set => _allyDeathCount = value; }
        public float StationHealth { get => _stationHealth; set => _stationHealth = value; }
        public float PlayerHealth { get => _playerHealth; set => _playerHealth = value; }
        public WeaponUpgradeState WeaponUpgrade { get => _weaponUpgrade; set => _weaponUpgrade = value; }
        public bool HasTurret { get => _hasTurret; set => _hasTurret = value; }
        public bool HasRocket1 { get => _hasRocket1; set => _hasRocket1 = value; }
        public bool HasRocket2 { get => _hasRocket2; set => _hasRocket2 = value; }
        public bool HasRocket3 { get => _hasRocket3; set => _hasRocket3 = value; }
        public HangarArrivalState HangarState { get => _hangarState; set => _hangarState = value; }

        public int GetFirstEmptyRocketIndex()
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

		public int GetFirstReadyRocketIndex()
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

		public void SetRocketState(int i, bool state)
        {
            if (i == 0)
            {
                HasRocket1 = state;
            } else
            if (i == 1)
            {
                HasRocket2 = state;
            } else
            if (i == 2)
            {
                HasRocket3 = state;
            }
            else
            {
                Debug.LogError($"Attempted to assign to an invalid rocket index {i}!");
            }
        }

        public int GetNumberOfRockets()
        {
            int sum = 0;
            if (_hasRocket1)
            {
                ++sum;
            }
            if (_hasRocket2)
            {
                ++sum;
            }
            if (_hasRocket3)
            {
                ++sum;
            }

            return sum;
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
                Destroy(this.transform.root.gameObject);
            }
        }

        [SerializeField] private string _mainMenuSceneName = default;
        [SerializeField] private string _hangarSceneName = default;
        [SerializeField] private string _missionSceneNamePrefix = default;
        [SerializeField] private string _controlsSceneName = default;
        [SerializeField] private string _endOfGameSceneName = default;

        [SerializeField] private bool _isMissionLoaded = false;
        public bool IsMissionLoaded { get => _isMissionLoaded; set => _isMissionLoaded = value; }

        [SerializeField] private HullIntegrity _playerHullIntegrity = default;
        [SerializeField] private HullIntegrity _stationHullIntegrity = default;

        [SerializeField] private GameState _state = default;
        public GameState State => _state;

        [SerializeField] public float _maxPlayerHealth = 500;
        public float MaxPlayerHealth => _maxPlayerHealth;

        public void OnPlayerHealthChanged()
        {
            State.PlayerHealth = _playerHullIntegrity.Value;
        }

        public void OnStationHealthChanged()
        { 
            State.StationHealth = _stationHullIntegrity.Value;
        }

        public void SetPlayerHullIntegrity(HullIntegrity playerHullIntegrity)
        {
            _playerHullIntegrity = playerHullIntegrity;
            _playerHullIntegrity.OnChanged += OnPlayerHealthChanged;
        }

        public void SetStationHullIntegrity(HullIntegrity stationHullIntegrity)
        {
            _stationHullIntegrity = stationHullIntegrity;
            _stationHullIntegrity.OnChanged += OnStationHealthChanged;
        }

        public void IncrementAlienDeathCount()
        {
            --_currentNumEnemies;
            if (_currentNumEnemies <= 0)
            {
                LoadHangarSuccess();
            }
        }

        public void IncrementPlayerEnemyKillCount()
        {
            ++State.PlayerEnemyKillCount;
        }

        public void IncrementPlayerAllyKillCount()
        {
            ++State.PlayerAllyKillCount;
        }

        public void IncrementAllyDeathCount()
        {
            ++State.AllyDeathCount;
        }

        private int _totalNumEnemies;
        private int _currentNumEnemies;
        public void SetNumberAliens(int numAliens)
        {
            _totalNumEnemies = numAliens;
            _currentNumEnemies = _totalNumEnemies;
        }

        public void DoCleanup()
        {
            _stationHullIntegrity.OnChanged -= OnStationHealthChanged;
            _playerHullIntegrity.OnChanged -= OnPlayerHealthChanged;
            ObjectPool.Instance.DoCleanup();
        }

        private float _savedPlayerHealth;
        private bool _savedHasRocket1;
        private bool _savedHasRocket2;
        private bool _savedHasRocket3;
        private int _savedMissionId;
        public void LoadHangarGameOver()
        {
            State.PlayerHealth = _savedPlayerHealth;
            State.HasRocket1 = _savedHasRocket1;
            State.HasRocket2 = _savedHasRocket2;
            State.HasRocket3 = _savedHasRocket3;
            State.MissionId = _savedMissionId;

            State.HangarState = GameState.HangarArrivalState.FAILURE;

            // Screen fade to hangar. Shows text "FAILURE".

            LoadLevel(_hangarSceneName);
        }

        public void LoadHangarSuccess()
        {
            State.HangarState = GameState.HangarArrivalState.SUCCESS;

            // Screen fade to hangar. Shows text "SUCCESS".

            LoadLevel(_hangarSceneName);
        }

        public void LoadHangarFromMenu()
        {
            State.HangarState = GameState.HangarArrivalState.MENU;

            LoadLevel(_hangarSceneName);
        }

        public void LoadMission(int index)
        {
            _savedPlayerHealth = State.PlayerHealth;
            _savedHasRocket1 = State.HasRocket1;
            _savedHasRocket2 = State.HasRocket2;
            _savedHasRocket3 = State.HasRocket3;
            _savedMissionId = State.MissionId;

            State.MissionId = index;

            var sceneName = $"{_missionSceneNamePrefix} {State.MissionId }";
            LoadLevel(sceneName);
        }

        public void LoadMainMenu()
        {
            LoadLevel(_mainMenuSceneName);
        }

        private void LoadLevel(string sceneName)
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
