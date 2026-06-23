using UnityEngine;
using UnityEngine.EventSystems;

namespace CardGame.UI
{
    [RequireComponent(typeof(CardView))]
    public class CardTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        CardView _view;

        void Awake() => _view = GetComponent<CardView>();

        public void OnPointerEnter(PointerEventData e) =>
            CardTooltipSystem.Instance?.BeginHover(_view.Card, e.position);

        public void OnPointerMove(PointerEventData e) { }

        public void OnPointerExit(PointerEventData e) =>
            CardTooltipSystem.Instance?.EndHover();

        void OnDisable() =>
            CardTooltipSystem.Instance?.EndHover();
    }
}
