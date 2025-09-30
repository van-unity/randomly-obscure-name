using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RON.Scripts {
    [CreateAssetMenu(menuName = "RON/Level Configuration", fileName = "level_Configuration")]
    public class LevelConfiguration : ScriptableObject {
        // Exposed constants (tweak X to your needs)
        public const int MIN_SIZE = 2;
        public const int MAX_SIZE = 20;

        public const int MIN_MATCH = 2;
        public const int MAX_MATCH = 10;

        [field: SerializeField]
        [field: Range(MIN_SIZE, MAX_SIZE)]
        public int Columns { get; private set; } = MIN_SIZE;

        [field: SerializeField]
        [field: Range(MIN_SIZE, MAX_SIZE)]
        public int Rows { get; private set; } = MIN_SIZE;

        [Tooltip("How many identical tiles user need to select")]
        [field: SerializeField]
        [field: Range(MIN_MATCH, MAX_MATCH)]
        public int MatchCount { get; private set; } = MIN_MATCH;

        [field: SerializeField] public Sprite[] Sprites { get; private set; }

#if UNITY_EDITOR
        private void OnValidate() {
            var hadIssue = false;
            var msg = new StringBuilder();
            msg.AppendLine($"[{nameof(LevelConfiguration)}] Validation for \"{name}\":");

            // Clamp Size
            int clampedSize = Mathf.Clamp(Columns, MIN_SIZE, MAX_SIZE);
            if (clampedSize != Columns) {
                hadIssue = true;
                msg.AppendLine($"- Size {Columns} out of range [{MIN_SIZE}..{MAX_SIZE}] → clamped to {clampedSize}.");
                Columns = clampedSize;
            }

            // MatchCount range: [MIN_MATCH, MAX_MATCH_EXCLUSIVE)
            int minAllowed = MIN_MATCH;
            int maxAllowedInclusive = MAX_MATCH - 1; // because it's exclusive
            int clampedMatch = Mathf.Clamp(MatchCount, minAllowed, maxAllowedInclusive);
            if (clampedMatch != MatchCount) {
                hadIssue = true;
                msg.AppendLine(
                    $"- MatchCount {MatchCount} out of range [{minAllowed}..{maxAllowedInclusive}] → clamped to {clampedMatch}.");
                MatchCount = clampedMatch;
            }

            // Sprites validation
            if (Sprites == null || Sprites.Length <= 1) {
                hadIssue = true;
                msg.AppendLine("- Sprites must contain more than 1 entry.");
            } else {
                var seen = new HashSet<Sprite>();
                for (int i = 0; i < Sprites.Length; i++) {
                    var s = Sprites[i];
                    if (s == null) {
                        hadIssue = true;
                        msg.AppendLine($"- Sprites[{i}] is null.");
                        continue;
                    }

                    if (!seen.Add(s)) {
                        hadIssue = true;
                        msg.AppendLine($"- Sprites contains duplicate entry \"{s.name}\" (index {i}).");
                    }
                }
            }

            if (hadIssue) {
                Debug.LogWarning(msg.ToString(), this);
            }
        }
#endif

        public bool TryValidate(out string report) {
            var ok = true;
            var sb = new StringBuilder();

            if (Columns < MIN_SIZE || Columns > MAX_SIZE) {
                ok = false;
                sb.AppendLine($"Size must be in [{MIN_SIZE}..{MAX_SIZE}] (was {Columns}).");
            }

            if (MatchCount < MIN_MATCH || MatchCount >= MAX_MATCH) {
                ok = false;
                sb.AppendLine($"MatchCount must be ≥ {MIN_MATCH} and < {MAX_MATCH} (was {MatchCount}).");
            }

            if (Sprites == null || Sprites.Length <= 1) {
                ok = false;
                sb.AppendLine("Sprites must contain more than 1 entry.");
            } else {
                var seen = new HashSet<Sprite>();
                for (int i = 0; i < Sprites.Length; i++) {
                    var s = Sprites[i];
                    if (s == null) {
                        ok = false;
                        sb.AppendLine($"Sprites[{i}] is null.");
                    } else if (!seen.Add(s)) {
                        ok = false;
                        sb.AppendLine($"Sprites contains duplicate entry \"{s.name}\" (index {i}).");
                    }
                }
            }

            report = sb.ToString().TrimEnd();
            return ok;
        }
    }
}