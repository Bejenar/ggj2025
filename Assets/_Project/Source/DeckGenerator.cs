using System.Collections.Generic;
using _Project.Source.Util;

namespace _Project.Source
{
    public class DeckGenerator
    {
        public Deck GenerateDeck(int deckPreset = 0)
        {
            List<Card> list = new List<Card>();
            int id = 1;

            for (int i = 0; i < 5; i++) // for each color
            {
                var color = (CardColor)i;
                var cardCount = GetCardCount(color);
                for (int j = 0; j < cardCount; j++)
                {
                    var value = GetCardValue((CardColor)i);
                    var card = new Card(id++, (CardColor)i, value);
                    list.Add(card);
                }
            }

            return new Deck(list);
        }

        public static int GetCardCount(CardColor color)
        {
            switch (color)
            {
                case CardColor.RED:
                    return 5;
                case CardColor.YELLOW:
                    return 7;
                case CardColor.BLUE:
                    return 9;
                case CardColor.GREEN:
                    return 9;
                default:
                    return 10;
            }
        }

        public static Card GetCard(int id, CardColor color)
        {
            return new Card(id, color, GetCardValue(color));
        }
        
        public static int GetCardValue(CardColor color)
        {
            switch (color)
            {
                case CardColor.RED:
                    return 4;
                case CardColor.YELLOW:
                    return 3;
                case CardColor.BLUE:
                    return 2;
                case CardColor.GREEN:
                    return 2;
                default:
                    return 1;
            }
        }
    }
}