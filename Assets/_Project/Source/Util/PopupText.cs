using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Source.Village.UI
{
    public class PopupText : MonoBehaviour
    {
        private Sequence _sequence;
        
        public void Start()
        {
            TMP_Text label = GetComponent<TMP_Text>();
            Vector3 position = transform.position;
            transform.localScale = Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(-0.3f, 0.3f));
            
            _sequence = DOTween.Sequence()
                .Append(transform.DOMoveY(position.y + Random.Range(-35, -55), 1f).SetEase(Ease.OutElastic))
                .Append(label.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.4f).SetDelay(0.3f));

            _sequence.Insert(0, transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutElastic))
                .Join(transform.DORotate(Vector3.zero, 2.0f).SetEase(Ease.OutElastic));

            _sequence.OnComplete(() =>
            {
                _sequence.Kill(true);
                Destroy(gameObject);
            });
        }
        
        public void Kill()
        {
            _sequence.Kill(true);
            Destroy(gameObject);
        }

        public void SetText(string text)
        {
            // TODO pass animation style to reuse in different scenarios with different popup strategies
            GetComponent<TMP_Text>().text = text;
        }
        
    }
}