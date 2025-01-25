using System.Collections.Generic;
using System.Linq;
using _Project.Source.Artifacts;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace _Project.Source.Util
{
    public class DeckView : MonoBehaviour
    {
        public CardZone noColorZone;
        public CardZone greenZone;
        public CardZone blueZone;
        public CardZone yellowZone;
        public CardZone redZone;
        
        public GameObject deckContainer;
        public GameObject artifactContainer;
        
        public InteractiveCard cardPrefab;
        public ArtifactView artifactPrefab;

        public Button closeButton;
        public Button deckButton;
        public Button artifactButton;
        
        public List<InteractiveCard> cards = new List<InteractiveCard>();
        public List<ArtifactView> artifacts = new List<ArtifactView>();
        
        public void Awake()
        {
            closeButton.onClick.AddListener(() =>
            {
                G.battleScene.HideDeck();
            });
            
            deckButton.onClick.AddListener(() =>
            {
                var isDeck = deckContainer.activeSelf;
                
                if (isDeck) return;
                
                deckContainer.SetActive(true);
                InitDeck();
                artifactContainer.SetActive(false);
            });
            
            artifactButton.onClick.AddListener(() =>
            {
                var isArtifact = artifactContainer.activeSelf;
                
                if (isArtifact) return;
                
                deckContainer.SetActive(false);
                artifactContainer.SetActive(true);
                InitArtifacts();
            });
        }
        
        public void InitDeck()
        {
            cards.Clear();
            PurgeZones();
            
            foreach (var card in G.state.deck.cards)
            {
                var zone = GetZone(card.color);
                var interactiveCard = Object.Instantiate(cardPrefab, zone.transform);
                interactiveCard.Init(card);
                interactiveCard.SetForViewOnly();
                cards.Add(interactiveCard);
                zone.TryClaim(interactiveCard);
            }
            
            UpdateDeck();
        }

        public void InitArtifacts()
        {
            artifactContainer.transform.DestroyChildren();
            artifacts.Clear();
            
            foreach (var artifact in G.state.artifacts)
            {
                var artifactView = Instantiate(artifactPrefab, artifactContainer.transform);
                artifactView.Init(artifact);
                artifacts.Add(artifactView);
            }

            UpdateArtifacts();
        }
        
        public void UpdateArtifacts()
        {
            
        }

        public void PurgeZones()
        {
            PurgeZone(noColorZone);
            PurgeZone(greenZone);
            PurgeZone(blueZone);
            PurgeZone(yellowZone);
            PurgeZone(redZone);
        }

        public void PurgeZone(CardZone zone)
        {
            zone.objects.ForEach(c => Destroy(c.gameObject));
            zone.objects.Clear();
        }
        
        public void UpdateDeck()
        {
            foreach (var card in cards)
            {
                if (!G.battleScene._deck.cards.Contains(card.card))
                {
                    card.IsPlayed = true;
                }
                card.UpdateCardView();
            }
        }
        
        public CardZone GetZone(CardColor color)
        {
            switch (color)
            {
                case CardColor.NO_COLOR:
                    return noColorZone;
                case CardColor.GREEN:
                    return greenZone;
                case CardColor.BLUE:
                    return blueZone;
                case CardColor.YELLOW:
                    return yellowZone;
                case CardColor.RED:
                    return redZone;
            }

            return null;
        }
    }
}