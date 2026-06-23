using UnityEngine;

namespace CardGame.UI
{
    public class BoardLayoutZones : MonoBehaviour
    {
        [Header("Shared")]
        public RectTransform deckZone;
        public RectTransform discardZone;
        public RectTransform offeredCardArea;

        [Header("Local Player")]
        public RectTransform localHandZone;
        public RectTransform localRecipeBoardZone;
        public RectTransform localRecipePileZone;
        public RectTransform localScoreDisplay;

        [Header("Opponent")]
        public RectTransform opponentHandZone;
        public RectTransform opponentRecipeBoardZone;
        public RectTransform opponentRecipePileZone;
        public RectTransform opponentScoreDisplay;

        [Header("Buttons")]
        public RectTransform endTurnButton;
        public RectTransform guideButton;
        public RectTransform deckButton;

        // Returns the world-space centre of a given card zone for animation targets.
        public Vector3 Centre(CardZone zone) => zone switch
        {
            CardZone.Deck        => WorldCentre(deckZone),
            CardZone.Hand        => WorldCentre(localHandZone),
            CardZone.RecipeBoard => WorldCentre(localRecipeBoardZone),
            CardZone.RecipePile  => WorldCentre(localRecipePileZone),
            CardZone.Discard     => WorldCentre(discardZone),
            CardZone.Offered     => WorldCentre(offeredCardArea),
            _                    => Vector3.zero
        };

        static Vector3 WorldCentre(RectTransform rt) =>
            rt != null ? rt.TransformPoint(rt.rect.center) : Vector3.zero;

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            DrawZoneGizmo(deckZone,                  Color.blue);
            DrawZoneGizmo(discardZone,               Color.gray);
            DrawZoneGizmo(offeredCardArea,            Color.yellow);
            DrawZoneGizmo(localHandZone,             Color.green);
            DrawZoneGizmo(localRecipeBoardZone,      Color.cyan);
            DrawZoneGizmo(localRecipePileZone,       new Color(0f, 0.8f, 0.4f));
            DrawZoneGizmo(opponentHandZone,          Color.red);
            DrawZoneGizmo(opponentRecipeBoardZone,   Color.magenta);
            DrawZoneGizmo(opponentRecipePileZone,    new Color(0.8f, 0.2f, 0.2f));
        }

        static void DrawZoneGizmo(RectTransform rt, Color color)
        {
            if (rt == null) return;
            Gizmos.color = color;
            var corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            for (int i = 0; i < 4; i++)
                Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
        }
#endif
    }
}
