using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Weapons.Targeting;
using SpaceGame.Pooling;
using SpaceGame.Hud;

namespace SpaceGame.Weapons
{
	public class WeaponsSystem : MonoBehaviour
    {
        //[SerializeField] private TextMeshPro _ammoLabel = default;

        [SerializeField] private HullIntegrity _hullIntegrity = default;
        [SerializeField] private FiringSystem[] _weapons = default;
        [SerializeField] private FiringSystem[] _rockets = default;

        private FiringSystem _currentWeapon;
        private TargetingSystem _currentTargetingSystem;

        [SerializeField] private Transform _targetLeadMouse = default;
        [SerializeField] private Transform _targetLeadAim = default;

        [SerializeField] private ProjectToCanvas _targetLeadMouseHudPrefab = default;
        [SerializeField] private ProjectToCanvas _targetLeadAimHudPrefab = default;
        [SerializeField] private LeadUi _targetLeadHudPrefab = default;

        private ProjectToCanvas _targetLeadMouseHud;
        private ProjectToCanvas _targetLeadAimHud;
        private Dictionary<TargetLead, LeadUi> _leadToHudMap;

        [SerializeField] private Canvas _canvas = default;
        [SerializeField] private RectTransform _canvasRect = default;
        [SerializeField] private Camera _camera = default;

        private ProjectToCanvas ProjectObjectToCanvas(ProjectToCanvas prefab, Transform trackedObject, Color color)
        {
            var elem = Instantiate<ProjectToCanvas>(prefab, _canvas.transform);
            elem.Camera = _camera;
            elem.Canvas = _canvas;
            elem.CanvasRect = _canvasRect;
            elem.TrackedObject = trackedObject;
            elem.Color = color;

            return elem;
        }

        private void CreateLeadHud(TargetLead lead)
        {
            if (!_leadToHudMap.ContainsKey(lead))
            {
                var elem = Instantiate<LeadUi>(_targetLeadHudPrefab, _canvas.transform);
                elem.Camera = _camera;
                elem.Canvas = _canvas;
                elem.CanvasRect = _canvasRect;
                elem.TrackedObject = lead.transform;
                if (lead.Target != null)
                { 
                    elem.SetTeam(lead.Target.Team);
                }
                elem.SetPlayerTransform(this.transform);
                elem.SetHullIntegrity(_hullIntegrity);
                elem.Show();

                _leadToHudMap.Add(lead, elem);
            }
        }

        private void OnTargetLeadAdded(TargetLead lead)
        {
            CreateLeadHud(lead);
        }

        private void OnTargetLeadRemoved(TargetLead lead)
        {
            if (_leadToHudMap.ContainsKey(lead))
            {
                Destroy(_leadToHudMap[lead].gameObject);
                _leadToHudMap.Remove(lead);
            }
        }

        private void Awake()
        {
            _leadToHudMap = new Dictionary<TargetLead, LeadUi>();
            _targetLeadAimHud = ProjectObjectToCanvas(_targetLeadAimHudPrefab, _targetLeadAim, Color.white);
        }

		private void Update()
        {
            if (_currentWeapon == null)
            {
                Debug.Log("WeaponsSystem has no weapon!");
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Debug.Log("Started shooting!");
                _currentWeapon?.Fire();
            } else
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                Debug.Log("Stopped shooting!");
                _currentWeapon?.StopFiring();
            } else
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Shooting a rocket!");
                var freeIndex = GameManager.Instance.State.GetFirstEmptyRocketIndex();
                if (freeIndex != -1)
                {
                    var rocket = _rockets[freeIndex];
                    if (!rocket.gameObject.activeInHierarchy)
                    {
                        Debug.LogError($"Rocket {freeIndex} is disabled but GameManager says it is available!");
                        return;
                    }
                    rocket.Fire();
                    rocket.gameObject.SetActive(false);
                    GameManager.Instance.State.SetRocketState(freeIndex, false);
                }
            }
        }

        public void AssignWeapon(FiringSystem weapon)
        {
            _currentTargetingSystem = weapon.transform.GetComponent<TargetingSystem>();
            _currentTargetingSystem.OnTargetLeadAdded += OnTargetLeadAdded;
            _currentTargetingSystem.OnTargetLeadRemoved += OnTargetLeadRemoved;

            _currentWeapon = weapon;
            _currentWeapon.gameObject.SetActive(true);
        }

        public void AssignTargetLeadAim(Transform targetLeadAim)
        {
            _targetLeadAim = targetLeadAim;
            _targetLeadAimHud = ProjectObjectToCanvas(_targetLeadAimHudPrefab, _targetLeadAim, Color.white);
        }
    }
}
