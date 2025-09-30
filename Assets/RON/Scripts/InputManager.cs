using System;
using UnityEngine;

namespace RON.Scripts {
    public class InputManager : MonoBehaviour {
        public event Action<Vector3> Clicked;

        private void Update() {
            if (Input.GetMouseButtonUp(0)) {
                var mousePos = Input.mousePosition;
                var worldPos =
                    Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
                Clicked?.Invoke(worldPos);
            }
        }
    }
}