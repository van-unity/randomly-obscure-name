using System;
using UnityEngine;

namespace RON.Scripts {
    [DefaultExecutionOrder(-1000)]
    public class GameplayContext : MonoBehaviour{
        [SerializeField] private Camera  _camera;
        [SerializeField] private BoardLayoutConfiguration _boardLayoutConfig;
        
        public TileContainerFactory TileContainerFactory { get; private set; }

        private void Awake() {
            TileContainerFactory = new TileContainerFactory(_camera,  _boardLayoutConfig);
        }
    }
}