using Cysharp.Threading.Tasks;

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

    public enum BuyType
    {
        APPLY_FUNCTION,
        CARD_PICKER
    }
}