using SpaceGame.Weapons.Targeting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame
{
	public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public List<Target> Targets { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                Targets = new List<Target>();
            }
            else
            {
                Debug.LogError($"Attempted to make another GameManager instance {this.name}!");
                Destroy(this);
            }
        }

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

        public event Action<Target> OnTargetAdded;
        public event Action<Target> OnTargetRemoved;
    }
}
