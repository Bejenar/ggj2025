using System;
using Engine.Math;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Project.Source.Util
{
    [Serializable]
    public class Card
    {
        public int id;
        public CardColor color;
        public int baseScore = 1;
        
        public Card(int id, CardColor color, int baseScore)
        {
            this.id = id;
            this.color = color;
            this.baseScore = baseScore;
        }
    }

    public enum CardColor
    {
        NO_COLOR,
        RED,
        BLUE,
        YELLOW,
        GREEN
    }
    
    public static class CardColorExtensions
    {
        public static string ToDescription(this CardColor color)
        {
            return color switch
            {
                CardColor.NO_COLOR => "Colorless".Color("#E284F7"),
                CardColor.RED => "Red".Color("#EA4345"),
                CardColor.BLUE => "Blue".Color("#00B6F7"),
                CardColor.YELLOW => "Yellow".Color("#EF9200"),
                CardColor.GREEN => "Green".Color("#95C308"),
                _ => "Unknown"
            };
        }
        
        public static Color ToColor(this CardColor color)
        {
            return color switch
            {
                CardColor.NO_COLOR => ColorUtils.ToRGBA(0xFFFFFF),
                CardColor.RED => ColorUtils.ToRGBA(0xEA4345),
                CardColor.BLUE => ColorUtils.ToRGBA(0x00B6F7),
                CardColor.YELLOW => ColorUtils.ToRGBA(0xEF9200),
                CardColor.GREEN => ColorUtils.ToRGBA(0x95C308),
                _ => ColorUtils.ToRGBA(0x000000)
            };
        }
    }
}