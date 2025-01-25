using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace _Project.Source.Util
{
    [Serializable]
    public class Deck
    {
        public readonly Stack<Card> cards = new();
        
        public int Count => cards.Count;
        
        public Deck(IEnumerable<Card> initialCards)
        {
            foreach (var card in initialCards) cards.Push(card);
        }
        
        public void AddCard(Card card)
        {
            cards.Push(card);
        }

        public Card DrawCard()
        {
            return cards.Count > 0 ? cards.Pop() : null;
        }

        public void Shuffle()
        {
            var shuffled = cards.OrderBy(_ => Random.value).ToList();
            cards.Clear();
            foreach (var card in shuffled)
            {
                cards.Push(card);
            }
        }
        
        public Deck DeepCopy()
        {
            return new Deck(cards);
        }
    }
}