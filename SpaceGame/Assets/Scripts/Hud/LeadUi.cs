using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SpaceGame.Pooling;
using SpaceGame.Weapons.Targeting;

namespace SpaceGame.Hud
{ 
    public class LeadUi : PoolableObject
    {
        [SerializeField] private RectTransform _elementRoot = default;
        [SerializeField] private TextMeshProUGUI _distance = default;
        [SerializeField] private Image _image = default;

        public Camera Camera { get; set; }
        public Canvas Canvas { get; set; }
        public RectTransform CanvasRect { get; set; }
        public TargetLead TargetLead { get; set; }

        private Color _color;
        private Team _team;
        private HullIntegrity _hullIntegrity;
        private Transform _playerTransform;

		protected override void Update()
		{
            Show();

            base.Update();
		}

        public void Show()
        {
            _distance.text = Vector3.Distance(TargetLead.transform.position, _playerTransform.position).ToString();
            _distance.color = _color;

            _image.color = _color;

            //var uiOffset = new Vector2((float)CanvasRect.sizeDelta.x / 2f, (float)CanvasRect.sizeDelta.y / 2f);
            //var viewportPoint = Camera.WorldToViewportPoint(TargetLead.transform.position);
            //var proportionalPosition = new Vector2(viewportPoint.x * CanvasRect.sizeDelta.x, viewportPoint.y * CanvasRect.sizeDelta.y);
            //_elementRoot.position = Camera.WorldToScreenPoint(TargetLead.transform.position);// proportionalPosition - uiOffset;

            //var screenPosition = Camera.WorldToScreenPoint(TargetLead.transform.position);
            //screenPosition.z = 0;



            //         var screenPosition = RectTransformUtility.WorldToScreenPoint(Camera, TargetLead.transform.position);

            //RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //	CanvasRoot,
            //	screenPosition,
            //	Canvas.renderMode == RenderMode.ScreenSpaceOverlay
            //		? null
            //		: Camera,
            //	out var newPos);

            //         _elementRoot.anchoredPosition = newPos;


            var ViewportPosition = Camera.WorldToViewportPoint(TargetLead.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position of the ui element
            _elementRoot.anchoredPosition = WorldObject_ScreenPosition;


            //Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera, TargetLead.transform.position);

            //Vector2 anchoredPosition = transform.InverseTransformPoint(screenPoint);

            //_elementRoot.anchoredPosition = anchoredPosition;

            //var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera, TargetLead.transform.position);
            //         _elementRoot.anchoredPosition = screenPoint - CanvasRect.sizeDelta / 2f;
        }

        public void SetPlayerTransform(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

		public void SetTeam(Team team)
        {
            _team = team;
            switch (_team)
            {
                case Team.HUMANS:
                {
                    _color = Color.green;
                    break;
                }
                default:
                {
                    _color = Color.red;
                    break;
                }
            }
        }

        public void SetHullIntegrity(HullIntegrity hullIntegrity)
        {
            _hullIntegrity = hullIntegrity;
        }
    }
}
