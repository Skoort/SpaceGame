using UnityEngine;

namespace SpaceGame.Hud
{
	public class ProjectToCanvas : MonoBehaviour
    {
        [SerializeField] protected RectTransform ElementRoot = default;
        [SerializeField] protected RectTransform GraphicsRoot = default;
        public Camera Camera { get; set; }
        public Canvas Canvas { get; set; }
        public RectTransform CanvasRect { get; set; }
        public Transform TrackedObject { get; set; }
        public Color Color { get; set; }

        protected virtual void LateUpdate()
        {
            Show();
        }

        public virtual void Show()
        {
            var screenPosition = Camera.WorldToScreenPoint(TrackedObject.position);
            screenPosition.z = 0;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                CanvasRect,
                screenPosition,
                Canvas.renderMode == RenderMode.ScreenSpaceOverlay
                    ? null
                    : Camera,
                out var newPos);

            ElementRoot.anchoredPosition = newPos;
        }
    }
}
