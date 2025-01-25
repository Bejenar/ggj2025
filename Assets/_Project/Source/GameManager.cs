using _Project.Source.Util;
using _Project.Source.Village.UI;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Source
{
    public class GameManager
    {
        public UnityAction<InteractiveCard> OnReleaseDrag;
        
        private CardTooltip cardTooltipPrefab;

        public GameManager(CardTooltip cardTooltipPrefab)
        {
            this.cardTooltipPrefab = cardTooltipPrefab;
        }
        
        public void ShowCardTooltip(InteractiveCard card)
        {
            var offset = card.interactable ? 200 : 100;
            var tooltip = Object.Instantiate(cardTooltipPrefab, card.transform.position + new Vector3(0, offset, 0), Quaternion.identity, card.transform);
            tooltip.Init(card);
        }
        
        public void StartDrag(DraggableSmoothDamp draggableSmoothDamp)
        {
            G.dragCard = draggableSmoothDamp.GetComponent<InteractiveCard>();
            G.dragCard.transform.SetAsLastSibling();
        }

        public void StopDrag()
        {
            OnReleaseDrag?.Invoke(G.dragCard);
            G.dragCard = null;
        }
        
        public void InitRunState()
        {
            var state = new RunState();
            state.deck = G.deckGenerator.GenerateDeck();
            state.gold = 0;
            state.maxHandSize = 8;
            state.maxPlays = 4;
            state.maxDiscards = 3;
            state.level = 0;
            state.artifacts.Clear();

            G.state = state;
        }
    }
}