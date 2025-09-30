using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace RON.Scripts {
    public class TileWidget : MonoBehaviour {
        [SerializeField] private SpriteRenderer _tileRenderer;
        [SerializeField] private float _revealDuration = .3f;
        [SerializeField] private Ease _rotationEase = Ease.InOutQuad;
        [SerializeField] private Vector3 _punch = new Vector3(-0.2f, -0.2f, 0);
        [SerializeField] private int _punchVibrato = 6;
        [SerializeField] private Ease _punchEase = Ease.OutQuad;
        [SerializeField] private float _matchDuration = .2f;

        private Transform _transform;

        public void SetTileSprite(Sprite tileSprite) {
            _tileRenderer.sprite = tileSprite;
        }

        public void Clear() {
            _tileRenderer.sprite = null;
            _transform.rotation = Quaternion.identity;
            _transform.localScale = Vector3.one;
        }

        public Sequence Reveal() {
            this.DOKill();
            var rotationTween = _transform
                .DORotate(Vector3.up * 180, _revealDuration)
                .SetEase(_rotationEase);
            var punchTween = _transform
                .DOPunchScale(_punch, _revealDuration, _punchVibrato)
                .SetEase(_punchEase);

            return DOTween.Sequence()
                .Join(rotationTween)
                .Join(punchTween)
                .SetTarget(this);
        }

        public Sequence Hide() {
            this.DOKill();
            var rotationTween = _transform
                .DORotate(Vector3.zero, _revealDuration)
                .SetEase(_rotationEase);
            var punchTween = _transform
                .DOPunchScale(_punch, _revealDuration, _punchVibrato)
                .SetEase(_punchEase);

            return DOTween.Sequence()
                .Join(rotationTween)
                .Join(punchTween)
                .SetTarget(this);
        }

        public Sequence Match() {
            this.DOKill();
            return DOTween.Sequence()
                .Join(_transform.DOScale(Vector3.zero, _matchDuration))
                .SetTarget(this);
        }

        private void Awake() {
            _transform = transform;
        }
    }
}