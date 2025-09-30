using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace RON.Scripts {
    public class ComboWidget : MonoBehaviour {
        private const string AMOUNT_FORMAT = "{0}X";

        [SerializeField] private TextMeshPro _amountText;
        [SerializeField] private GameObject _amount;
        [SerializeField] private GameObject _combo;
        [SerializeField] private Vector3 _amountPunch = new Vector3(0.2f, 0.2f, 0);
        [SerializeField] private float _amountPunchDuration = .3f;
        [SerializeField] private int _amountPunchVibrato = 0;
        [SerializeField] private Ease _amountPunchEase = Ease.InOutQuad;
        [SerializeField] private float _amountPunchInterval = .1f;
        [SerializeField] private float _hideInterval = .5f;

        public void Show(int amount) {
            this.DOKill();
            _amountText.text = string.Format(AMOUNT_FORMAT, amount);
            _combo.SetActive(true);
            var amountPunchTween = _amountText.transform
                .DOPunchScale(_amountPunch, _amountPunchDuration, _amountPunchVibrato)
                .SetEase(_amountPunchEase);
            DOTween.Sequence()
                .AppendInterval(_amountPunchInterval)
                .AppendCallback(() => _amount.SetActive(true))
                .Append(amountPunchTween)
                .AppendInterval(_hideInterval)
                .AppendCallback(Hide)
                .SetTarget(this);
        }

        public void Hide() {
            _amount.SetActive(false);
            _combo.SetActive(false);
        }

        private void Awake() { }
    }
}