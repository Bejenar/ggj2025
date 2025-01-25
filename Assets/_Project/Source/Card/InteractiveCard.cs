using System;
using _Project.Data;
using _Project.Source.Village.UI;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Source.Util
{
    public class InteractiveCard : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public CardZone zone;
        public Card card;
        public Image image;

        public MoveableBase moveable;
        public DraggableSmoothDamp draggable;
        public IdleAnimation idleAnimation;
        public int order;

        public float Width = 1;

        public bool selected = false;

        public event Action OnPointerExitEvent;
        
        public bool interactable = true;
        public bool IsPlayed = false;
        
        public void Init(Card card)
        {
            this.card = card;

            UpdateCardView();
        }

        public void SetForViewOnly()
        {
            interactable = false;
        }

        public void UpdateCardView()
        {
            image.sprite = G.cardSpriteResolver.GetSprite(card.color);

            if (IsPlayed)
            {
                image.color = image.color.WithAlpha(0.5f);
            }
        }

        public void Punch()
        {
            transform.DOPunchScale(Vector3.one, 0.1f);
        }

        public void Punch(float power)
        {
            transform.DOPunchScale(Vector3.one * power, 0.1f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (G.dragCard != null || !interactable)
            {
                return;
            }
            
            G.audio.Play<ButtonSFX>();

            if (!selected)
            {
                // TODO move to game manager
                if (G.battleScene.selected.Count >= 5) return;
                
                selected = true;
                Select();
            }
            else
            {
                selected = false;
                Deselect();
            }
        }

        public int movePower;

        public void Select()
        {
            idleAnimation.Kill();
            // TODO move to game manager
            G.battleScene.Select(this);
            Debug.Log("Select");
            image.transform.DOKill(true);
            image.transform.DOPunchRotation(Vector3.one * 10f, 0.1f, 100);
            image.transform.DOLocalMoveY(movePower, 0.1f);
        }

        public void Deselect()
        {
            // TODO move to game manager
            G.battleScene.Deselect(this);
            Debug.Log("Deselect");
            image.transform.DOKill(true);
            image.transform.DOPunchRotation(Vector3.one * 10f, 0.1f, 100);
            image.transform.DOLocalMoveY(0, 0.1f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            idleAnimation.Kill();
            Debug.Log("Pointer Enter" + gameObject.name);
            if (G.dragCard != null) return;
            
            G.main.ShowCardTooltip(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitEvent?.Invoke();
            
            if (!selected) idleAnimation.Play();
        }
    }

    [Serializable]
    public class CardSpriteEntry
    {
        public CardColor color;
        public Sprite sprite;
    }
}