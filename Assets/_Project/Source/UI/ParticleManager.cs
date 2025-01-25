using System.Collections.Generic;
using System.Linq;
using _Project.Data;
using _Project.Source.Util;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Source.Village.UI
{
    public class ParticleManager : MonoBehaviour
    {
        public ParticleSystem bubbles;
        public Transform bottle;

        public CardColor[] colors;

        public Transform initPosition;
        public Transform attackPosition;

        private void Start()
        {
            G.particles = this;
        }

        public async UniTask AttackAnimation()
        {
            float spinUpDuration = 0.5f; // Time to build up intensity
            float shakeDuration = 1f; // Duration of the vigorous shake
            float rotationAngle = 30f; // Maximum shake rotation angle
            float positionDistance = 20f; // Shake position intensity
            int shakeLoops = 5; // Number of shake loops

            var seq = DOTween.Sequence();

            var shakeSequence = DOTween.Sequence();
            shakeSequence.Append(bottle.DOLocalMoveY(positionDistance, spinUpDuration / shakeLoops)
                    .SetEase(Ease.InOutSine))
                .Append(bottle.DOLocalMoveY(-positionDistance, spinUpDuration / shakeLoops)
                    .SetEase(Ease.InOutSine))
                .SetLoops(shakeLoops, LoopType.Yoyo); // Builds up to the shake

            seq.Append(shakeSequence);
            seq.JoinCallback(() => G.audio.Play<ShakingSFX>());
            seq.Append(bottle.DOMove(attackPosition.position, 1.0f).SetEase(Ease.InOutBack));
            seq.Join(bottle.DORotateQuaternion(attackPosition.rotation, 1.0f).SetEase(Ease.InOutBack));

            await seq;

            PlayBubbles();
            await UniTask.WaitWhile(() => bubbles.isEmitting);
            await UniTask.WaitForSeconds(0.5f);

            bottle.DORotateQuaternion(initPosition.rotation, 1.0f).SetEase(Ease.InOutBack);
            await bottle.DOMove(initPosition.position, 1.0f).SetEase(Ease.InOutBack);
        }

        public void PlayBubbles()
        {
            G.audio.Play<BubbleGunSFX>();
            Debug.Log("PlayBubbles");
            var main = bubbles.main;
            var gradient = new ParticleSystem.MinMaxGradient(
                GetGradient(G.battleScene.selected
                    .Select(x => x.card.color).ToList()))
            {
                mode = ParticleSystemGradientMode.RandomColor
            };

            main.startColor = gradient;

            bubbles.Play();
        }

        public Gradient GetGradient(List<CardColor> colorsToInclude)
        {
            Gradient gradient = new Gradient();
            gradient.mode = GradientMode.Fixed;

            GradientColorKey[] colorKeys = new GradientColorKey[colorsToInclude.Count];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[colorsToInclude.Count];

            for (int i = 0; i < colorsToInclude.Count; i++)
            {
                colorKeys[i].color = colorsToInclude[i].ToColor();
                colorKeys[i].time = (float)(i + 1) / (colorsToInclude.Count);
                alphaKeys[i].alpha = 1.0f;
                alphaKeys[i].time = (float)(i + 1) / (colorsToInclude.Count);
            }

            gradient.SetKeys(colorKeys, alphaKeys);
            return gradient;
        }
    }
}