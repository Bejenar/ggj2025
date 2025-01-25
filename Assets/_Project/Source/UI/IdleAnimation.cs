using System;
using DG.Tweening;
using UnityEngine;

namespace _Project.Source.Village.UI
{
    public class IdleAnimation : MonoBehaviour
    {
        public Sequence tween;

        private void Start()
        {
            Play();
        }

        public void Kill()
        {
            tween.Kill();
            transform.rotation = Quaternion.identity;
        }

        private void OnDestroy()
        {
            Kill();
        }

        public void Play()
        {
            var angle = UnityEngine.Random.Range(-10, 10);
            var angle2 = UnityEngine.Random.Range(-10, 10);
            var duration = UnityEngine.Random.Range(2, 4);
            var duration2 = UnityEngine.Random.Range(2, 4);
            tween = DOTween.Sequence();
            
            
            tween.Append(transform.DORotate(new Vector3(0, 0, angle), duration, RotateMode.LocalAxisAdd))
                .Append(transform.DORotate(new Vector3(0, 0, -angle2), duration2, RotateMode.LocalAxisAdd))
                .SetLoops(-1, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    angle = UnityEngine.Random.Range(5, 10);
                    angle2 = UnityEngine.Random.Range(5, 10);
                    duration = UnityEngine.Random.Range(3, 7);
                    duration2 = UnityEngine.Random.Range(3, 7);
                });
        }
    }
}