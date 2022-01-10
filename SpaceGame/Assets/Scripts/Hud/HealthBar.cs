using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceGame.Hud
{ 
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private HullIntegrity _trackedHullIntegrity = default;
        [SerializeField] private Image _imageSlider = default;

        private void OnEnable()
        {
            _trackedHullIntegrity.OnChanged += OnHullIntegrityChanged;
        }

        private void OnDisable()
        {
            _trackedHullIntegrity.OnChanged -= OnHullIntegrityChanged;
        }

        private void OnHullIntegrityChanged()
        {
            _imageSlider.fillAmount = _trackedHullIntegrity.Value / _trackedHullIntegrity.MaxValue;
        }
    }
}
