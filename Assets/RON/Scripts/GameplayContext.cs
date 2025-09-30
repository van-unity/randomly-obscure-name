using System;
using UnityEngine;

namespace RON.Scripts {
    [DefaultExecutionOrder(-1000)]
    public class GameplayContext : MonoBehaviour {
        [SerializeField] private Camera _camera;
        [SerializeField] private BoardLayoutConfiguration _boardLayoutConfig;
        [SerializeField] private TileWidget _tileWidget;
        [SerializeField] private int _tilePoolSize = 100;
        [SerializeField] private int _tilePoolGrowSize = 3;

        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public Board Board { get; private set; }

        public TileContainerFactory TileContainerFactory { get; private set; }
        public TilePool TilePool { get; private set; }
        public Bounds TileWidgetBounds { get; private set; }

        private void Awake() {
            TileContainerFactory = new TileContainerFactory(_camera, _boardLayoutConfig);
            TileWidgetBounds = _tileWidget.transform.GetBounds();
            TilePool = new TilePool(_tileWidget, _tilePoolSize, _tilePoolGrowSize);
        }
    }
}