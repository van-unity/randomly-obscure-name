using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RON.Scripts {
    public static class TransformExtensions {
        public static List<Transform> GetChildren(this Transform parent)
            => Enumerable
                .Range(0, parent.childCount)
                .Select(parent.GetChild)
                .ToList();

        public static Bounds GetBounds(this Transform parent, bool includeInactive = true) {
            var renderers = parent.GetComponentsInChildren<Renderer>(includeInactive);
            if (renderers.Length == 0) {
                return new Bounds(parent.position, Vector3.zero);
            }

            var bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }
    }
}