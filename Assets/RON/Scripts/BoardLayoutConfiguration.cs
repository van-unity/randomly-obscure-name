using UnityEngine;

namespace RON.Scripts {
    [CreateAssetMenu(menuName = "RON/Board Layout Configuration", fileName = "board-layout_Configuration")]
    public class BoardLayoutConfiguration : ScriptableObject {
        [field: SerializeField] public Vector2 AnchorMin { get; private set; }
        [field: SerializeField] public Vector2 AnchorMax { get; private set; }
        [field: SerializeField] public float PaddingLeft { get; private set; }
        [field: SerializeField] public float PaddingRight { get; private set; }
        [field: SerializeField] public float PaddingTop { get; private set; }
        [field: SerializeField] public float PaddingBottom { get; private set; }
        [field: SerializeField] public float SpacingX { get; private set; }
        [field: SerializeField] public float SpacingY { get; private set; }
    }
}