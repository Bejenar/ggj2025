using System.Collections.Generic;
using System.Linq;

namespace _Project.Source.Util
{
    public interface ICombo
    {
        string Name { get; }
        float Multiplier { get; }
        bool IsMatch(List<Card> cards);
    }
    
    public class EmptyCombo : ICombo
    {
        public string Name => "";
        public float Multiplier => 1;

        public bool IsMatch(List<Card> cards)
        {
            return cards.Count == 0;
        }
    }

    public class NoCombo : ICombo
    {
        public string Name => "No combo";
        public float Multiplier => 1;

        public bool IsMatch(List<Card> cards)
        {
            return true;
        }
    }

    public class MonochromeCombo : ICombo
    {
        public string Name => "Monochrome";
        public float Multiplier => 1.5f;

        public bool IsMatch(List<Card> cards)
        {
            return cards.Select(c => c.color).Distinct().Count() == 1;
        }
    }

    public class TwoColorCombo : ICombo
    {
        public string Name => "Two-Color";
        public float Multiplier => 2;

        public bool IsMatch(List<Card> cards)
        {
            return cards.Select(c => c.color).Distinct().Count() == 2;
        }
    }

    public class ThreeColorCombo : ICombo
    {
        public string Name => "Three-Color";
        public float Multiplier => 3;

        public bool IsMatch(List<Card> cards)
        {
            return cards.Select(c => c.color).Distinct().Count() == 3;
        }
    }

    public class FourColorCombo : ICombo
    {
        public string Name => "Four-Color";
        public float Multiplier => 4;

        public bool IsMatch(List<Card> cards)
        {
            return cards.Select(c => c.color).Distinct().Count() == 4;
        }
    }
    
    public class FiveColorCombo : ICombo
    {
        public string Name => "Five-Color";
        public float Multiplier => 5;

        public bool IsMatch(List<Card> cards)
        {
            return cards.Select(c => c.color).Distinct().Count() == 5;
        }
    }
}