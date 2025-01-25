using _Project.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Source
{
    public class BagView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOKill(true);
            transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.InOutBack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOKill(true);
            transform.DOScale(Vector3.one, 0.1f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Show deck");
            G.audio.Play<ButtonLowSFX>();
            G.battleScene.ShowDeck();
        }
    }
}