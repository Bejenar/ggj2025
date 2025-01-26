using System;
using System.Collections.Generic;
using _Project.Source.Util;
using UnityEngine;

namespace _Project.Source
{
    [CreateAssetMenu(menuName = "Bubble/CardSpriteResolver")]
    public class CardSpriteResolver : ScriptableObject
    {
        [SerializeField]
        private CardSpriteEntry[] cardSprites;
        private readonly Dictionary<CardColor, Sprite> _spriteMap = new();


        private void OnEnable()
        {
            OnValidate();
        }

        public Sprite GetSprite(CardColor color)
        {
            return _spriteMap[color];
        }
        
        private void OnValidate()
        {
            _spriteMap.Clear();
            
            if (cardSprites == null) return;
            
            foreach (var entry in cardSprites)
            {
                _spriteMap[entry.color] = entry.sprite;
            }
        }
    }
}