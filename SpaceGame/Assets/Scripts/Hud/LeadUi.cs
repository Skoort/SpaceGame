using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SpaceGame;
using SpaceGame.Pooling;
using SpaceGame.Weapons.Targeting;

namespace SpaceGame.Hud
{ 
    public class LeadUi : ProjectToCanvas
    {
        [SerializeField] private TextMeshProUGUI _distance = default;
        [SerializeField] private Image _image = default;

        private Team _team;
        private HullIntegrity _hullIntegrity;
        private Transform _playerTransform;

		public override void Show()
        {
            base.Show();

            var dirToTarget = TrackedObject.position - _playerTransform.position;

            var angle = Mathf.Abs(Vector3.Angle(transform.forward, dirToTarget));
            if (GraphicsRoot.gameObject.activeInHierarchy && angle > 90)
            {
                GraphicsRoot.gameObject.SetActive(false);
                return;
            } else
            if (!GraphicsRoot.gameObject.activeInHierarchy && angle < 90)
            {
                GraphicsRoot.gameObject.SetActive(true);
            }

            _distance.text = Mathf.FloorToInt(dirToTarget.magnitude).ToString();
			_distance.color = Color;

			_image.color = Color;
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
                    Color = Color.green;
                    break;
                }
                default:
                {
                    Color = Color.red;
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
