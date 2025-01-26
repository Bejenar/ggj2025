using System.Linq;
using _Project.Source.Util;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityUtils;

namespace _Project.Source.Artifacts
{
    public interface IShopItem : IDescription
    {
        public int Cost { get; }
        public BuyType BuyType { get; }

        public UniTask Purchase();

        public string Name { get; }

        public int ScarcityCoef { get; }
    }

    public class ArtifactShopItem : IShopItem
    {
        public IArtifact artifact;
        public int cost;

        public ArtifactShopItem(IArtifact artifact, int cost, int scarcityCoef)
        {
            this.artifact = artifact;
            this.cost = cost;
            ScarcityCoef = scarcityCoef;
        }

        public int Cost => cost;
        public BuyType BuyType => BuyType.APPLY_FUNCTION;
        public string Description => artifact.Description;

        public async UniTask Purchase()
        {
            G.state.artifacts.Add(artifact);
            await UniTask.CompletedTask;
        }

        public string Name => artifact.Name;
        public int ScarcityCoef { get; }
    }

    public class PermanentlyAddHandSizeShopItem : IShopItem
    {
        public int cost;
        public int amount;
        public string description;

        public PermanentlyAddHandSizeShopItem(int cost, int amount, int scarcityCoef, string name, string description)
        {
            this.cost = cost;
            this.amount = amount;
            this.description = description;
            Name = name;
            ScarcityCoef = scarcityCoef;
        }

        public int Cost => cost;
        public BuyType BuyType => BuyType.APPLY_FUNCTION;
        public string Description => description;

        public async UniTask Purchase()
        {
            G.state.maxHandSize += amount;
            await UniTask.CompletedTask;
        }

        public string Name { get; }
        public int ScarcityCoef { get; }
    }

    public class PermanentlyAddMaxPlaysShopItem : IShopItem
    {
        public int cost;
        public int amount;
        public string description;

        public PermanentlyAddMaxPlaysShopItem(int cost, int amount, int scarcityCoef, string name, string description)
        {
            this.cost = cost;
            this.amount = amount;
            this.description = description;
            Name = name;
            ScarcityCoef = scarcityCoef;
        }

        public int Cost => cost;
        public BuyType BuyType => BuyType.APPLY_FUNCTION;
        public string Description => description;

        public async UniTask Purchase()
        {
            G.state.maxPlays += amount;
            await UniTask.CompletedTask;
        }

        public string Name { get; }
        public int ScarcityCoef { get; }
    }

    public class PermanentlyAddMaxDiscardsShopItem : IShopItem
    {
        public int cost;
        public int amount;
        public string description;

        public PermanentlyAddMaxDiscardsShopItem(int cost, int amount, int scarcityCoef, string name,
            string description)
        {
            this.cost = cost;
            this.amount = amount;
            this.description = description;
            Name = name;
            ScarcityCoef = scarcityCoef;
        }

        public int Cost => cost;
        public BuyType BuyType => BuyType.APPLY_FUNCTION;
        public string Description => description;

        public async UniTask Purchase()
        {
            G.state.maxDiscards += amount;
            await UniTask.CompletedTask;
        }

        public string Name { get; }
        public int ScarcityCoef { get; }
    }
    
    public class AddGoldShopItem : IShopItem
    {
        public int amount;
        public string description;

        public AddGoldShopItem(int amount, int scarcityCoef, string name, string description)
        {
            this.amount = amount;
            this.description = description;
            Name = name;
            ScarcityCoef = scarcityCoef;
        }

        public int Cost => 0;
        public BuyType BuyType => BuyType.APPLY_FUNCTION;
        public string Description => description;

        public async UniTask Purchase()
        {
            var otherShopItems = G.shopManager._shopItemViews.Where(view => view.description.text != description).ToList();
            
            if (otherShopItems.Count() != 2) return;
            
            G.state.gold += amount;

            foreach (var shopItemView in otherShopItems)
            {
                shopItemView.Dissolve();
            }

            G.shopManager._shopItemViews.RemoveAll(view => view.description.text != description);
            
            await UniTask.CompletedTask;
        }

        public string Name { get; }
        public int ScarcityCoef { get; }
    }
    
    public class AddCardsToDeckShopItem : IShopItem
    {
        public int amount;
        public CardColor color;

        public AddCardsToDeckShopItem(int cost, int scarcityCoef, int amount, CardColor color, string name)
        {
            Name = name;
            Cost = cost;
            ScarcityCoef = scarcityCoef;
            
            this.color = color;
            this.amount = amount;
        }

        public int Cost { get; }
        public BuyType BuyType => BuyType.APPLY_FUNCTION;
        public string Description => $"Add {amount} {color.ToDescription()} soap to your bag";

        public async UniTask Purchase()
        {
            for (int i = 0; i < amount; i++)
            {
                var id = G.state.deck.Count + 1;
                G.state.deck.cards.Push(DeckGenerator.GetCard(id, color));
            }
            
            await UniTask.CompletedTask;
        }

        public string Name { get; }
        public int ScarcityCoef { get; }
    }

    public enum BuyType
    {
        APPLY_FUNCTION,
        CARD_PICKER
    }
}