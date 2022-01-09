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
        [SerializeField] private GameObject _weapon1 = default;
        [SerializeField] private GameObject _weapon2 = default;
        [SerializeField] private GameObject _weapon3 = default;
        [SerializeField] private GameObject _weapon4 = default;

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

        private void CreateLeadsHud()
        {
            _leadToHudMap = new Dictionary<TargetLead, LeadUi>();
            foreach (var lead in _currentTargetingSystem.Leads)
            {
                OnTargetLeadAdded(lead);
            }
        }

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
            _currentWeapon = _weapon1.GetComponent<FiringSystem>();
            _currentTargetingSystem = _weapon1.GetComponent<TargetingSystem>();
        }

		private void Start()
		{
            _currentTargetingSystem.OnTargetLeadAdded += OnTargetLeadAdded;
            _currentTargetingSystem.OnTargetLeadRemoved += OnTargetLeadRemoved;    

            CreateLeadsHud();
            //_targetLeadMouseHud = ProjectObjectToCanvas(_targetLeadMouseHudPrefab, _targetLeadMouse, Color.white);
            _targetLeadAimHud = ProjectObjectToCanvas(_targetLeadAimHudPrefab, _targetLeadAim, Color.white);
        }

		private void Update()
        {
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
                _currentWeapon?.Reload();
            }
        }
    }
}
