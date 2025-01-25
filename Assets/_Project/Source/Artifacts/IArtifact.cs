using _Project.Data;
using _Project.Source.Util;
using Cysharp.Threading.Tasks;

namespace _Project.Source.Artifacts
{
    public interface IDescription
    {
        string Description { get; }
    }

    public interface IArtifact : IDescription
    {
        IArtifact artifact { get; }

        string Name { get; }

        public async UniTask Apply(UniTask action)
        {
            if (G.animationsEnabled)
            {
                await action;
                return;
            }

            await UniTask.CompletedTask;
        }
    }

    public interface ICardValueModifier
    {
        UniTask<int> ModifyCardValue(Card card, int value);
    }

    public class ColorScoreModifier : IArtifact, ICardValueModifier
    {
        public int additionalScore;
        public CardColor color;

        public ColorScoreModifier(int additionalScore, CardColor color, string name)
        {
            this.additionalScore = additionalScore;
            this.color = color;
            Name = name;
            
        }

        public async UniTask<int> ModifyCardValue(Card card, int value)
        {
            if (card.color == color)
            {
                await artifact.Apply(Animate());
                return additionalScore + value;
            }
                
            return value;
        }

        private async UniTask Animate()
        {
            await UniTask.WaitForSeconds(0.3f);
            G.audio.Play<ButtonSFX>();
            G.battleScene.ShowText(G.battleScene.artifactTextSpawnPosition.position, $"{Name}\n+{additionalScore}", true);
            await UniTask.WaitForSeconds(0.5f);
        }

        public string Description => $"{color.ToDescription()} soap gives {additionalScore} extra score";
        public IArtifact artifact => this;
        public string Name { get; }
    }
}