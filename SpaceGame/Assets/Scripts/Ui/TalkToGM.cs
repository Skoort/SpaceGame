using UnityEngine;

namespace SpaceGame.Ui
{
    public class TalkToGM : MonoBehaviour
    {
        public GameManager GameManager { get; set; }

        protected virtual void Start()
        {
            GameManager = GameManager.Instance;
        }

        public void LoadNextMission()
        {
            GameManager.LoadMission(GameManager.State.MissionId + 1);
        }

        public void LoadHangar()
        {
            GameManager.LoadHangarFromMenu();
        }

        public void LoadMainMenu()
        {
            GameManager.LoadMainMenu();
        }

        public void OnScreenFadedToBlack()
        {
            GameManager.OnScreenFadedToBlack();
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
