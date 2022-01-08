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

        private Dictionary<Compartment, FiringSystem> _weaponsByCompartment;


        [SerializeField] private TargetLead _targetLeadMouse = default;

        [SerializeField] private LeadUi _leadHudElementPrefab = default;
        private Dictionary<TargetLead, LeadUi> _leadToHudMap;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;
        private Vector2 _uiOffset;

        private void CreateLeadsHud()
        {
            _leadToHudMap = new Dictionary<TargetLead, LeadUi>();
            foreach (var lead in _currentTargetingSystem.Leads)
            {
                OnTargetLeadAdded(lead);
            }
        }

        private void CreateLeadHud(TargetLead lead)
        {
            if (!_leadToHudMap.ContainsKey(lead))
            {
                var elem = ObjectPool.Instance.RequestObject(
                    _leadHudElementPrefab.ResourceName,
                    _leadHudElementPrefab.gameObject,
                    desiredParent: _canvas.transform
                    ).GetComponent<LeadUi>();
                elem.Camera = _camera;
                elem.Canvas = _canvas;
                elem.CanvasRect = _canvas.GetComponent<RectTransform>();
                elem.TargetLead = lead;
                elem.SetTeam(lead.Target.Team);
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
            }
        }
    }
}
