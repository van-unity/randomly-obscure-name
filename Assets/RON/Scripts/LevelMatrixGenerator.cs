using System;
using System.Collections.Generic;
using UnityEngine;

namespace RON.Scripts {
    public static class LevelMatrixGenerator {
        /// <summary>
        /// Generates a rows×cols matrix of sprite indices (indices into cfg.Sprites) such that:
        /// - Total tiles = Columns*Rows
        /// - Each sprite appears in a count that is a multiple of MatchCount
        /// - The board can be cleared by selecting any k tiles of the same sprite (no adjacency required)
        /// Throws on invalid input; use TryGenerate if you prefer a non-throwing path.
        /// </summary>
        public static int[,] GenerateIndices(LevelConfiguration cfg, int? seed = null) {
            ValidateOrThrow(cfg);

            int cols = cfg.Columns;
            int rows = cfg.Rows;
            int k = cfg.MatchCount;
            int n = cols * rows;

            if (n % k != 0)
                throw new InvalidOperationException($"Board size {n} must be divisible by MatchCount {k}.");

            // Number of k-groups we must allocate across sprites
            int groups = n / k;

            // Distribute groups across sprites as evenly as possible
            int spriteCount = cfg.Sprites.Length;
            var groupsPerSprite = DistributeGroups(groups, spriteCount, seed);

            // Build flat array of sprite indices
            var flat = new int[n];
            int p = 0;
            for (int s = 0; s < spriteCount; s++) {
                int count = groupsPerSprite[s] * k; // multiple of k
                for (int i = 0; i < count; i++)
                    flat[p++] = s;
            }

            // Safety: due to rounding, ensure we filled exactly N
            if (p != n)
                throw new Exception($"Internal error: filled {p} tiles, expected {n}.");

            // Shuffle flat indices
            ShuffleInPlace(flat, seed);

            // Write into matrix row-major
            var matrix = new int[rows, cols];
            p = 0;
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    matrix[r, c] = flat[p++];

            return matrix;
        }
        
        private static void ValidateOrThrow(LevelConfiguration cfg) {
            if (cfg == null) throw new ArgumentNullException(nameof(cfg));
            if (cfg.Sprites == null || cfg.Sprites.Length <= 1)
                throw new ArgumentException("Sprites must contain more than 1 entry.", nameof(cfg));
            for (int i = 0; i < cfg.Sprites.Length; i++)
                if (cfg.Sprites[i] == null)
                    throw new ArgumentException($"Sprites[{i}] is null.", nameof(cfg));

            if (cfg.Columns < LevelConfiguration.MIN_SIZE || cfg.Columns > LevelConfiguration.MAX_SIZE)
                throw new ArgumentOutOfRangeException(nameof(cfg.Columns),
                    $"Columns must be in [{LevelConfiguration.MIN_SIZE}..{LevelConfiguration.MAX_SIZE}]");
            if (cfg.Rows < LevelConfiguration.MIN_SIZE || cfg.Rows > LevelConfiguration.MAX_SIZE)
                throw new ArgumentOutOfRangeException(nameof(cfg.Rows),
                    $"Rows must be in [{LevelConfiguration.MIN_SIZE}..{LevelConfiguration.MAX_SIZE}]");

            if (cfg.MatchCount < LevelConfiguration.MIN_MATCH || cfg.MatchCount >= LevelConfiguration.MAX_MATCH)
                throw new ArgumentOutOfRangeException(nameof(cfg.MatchCount),
                    $"MatchCount must be ≥ {LevelConfiguration.MIN_MATCH} and < {LevelConfiguration.MAX_MATCH}");
        }

        /// <summary>
        /// Evenly distributes 'groups' across 'spriteCount' entries (e.g., {⌈g/m⌉ or ⌊g/m⌋}).
        /// Uses round-robin with an optional seeded start offset for variety.
        /// </summary>
        private static int[] DistributeGroups(int groups, int spriteCount, int? seed) {
            var arr = new int[spriteCount];
            if (spriteCount <= 0) return arr;
            if (groups <= 0) return arr;

            int offset = 0;
            if (seed.HasValue) {
                // simple deterministic offset to vary which sprites get the extra group
                var rng = new System.Random(seed.Value);
                offset = rng.Next(0, spriteCount);
            }

            // Round-robin: assign 1 group per step until we run out
            for (int g = 0; g < groups; g++) {
                int idx = (offset + g) % spriteCount;
                arr[idx]++;
            }

            return arr;
        }

        private static void ShuffleInPlace(int[] a, int? seed) {
            var rng = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
            for (int i = a.Length - 1; i > 0; i--) {
                int j = rng.Next(i + 1);
                (a[i], a[j]) = (a[j], a[i]);
            }
        }
    }
}