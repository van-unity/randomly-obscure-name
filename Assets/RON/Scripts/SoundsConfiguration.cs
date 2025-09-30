using UnityEngine;

namespace RON.Scripts {
    [CreateAssetMenu(menuName = "RON/Sounds Configuration", fileName = "sounds_Configuration")]
    public class SoundsConfiguration : ScriptableObject {
        [field: SerializeField] public AudioClip BackgroundMusic { get; private set; }
        [field: SerializeField] public AudioClip TileClicked { get; private set; }
        [field: SerializeField] public AudioClip Match { get; private set; }
        [field: SerializeField] public AudioClip NoMatch { get; private set; }
    }
}