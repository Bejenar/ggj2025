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

    public interface IAfterOnComboEvaluated
    {
        UniTask OnComboEvaluated();
    }
    
    public class AddGoldOnCombo : IArtifact, IAfterOnComboEvaluated
    {
        public int goldAmount;
        public ICombo combo;
        public string Name { get; }

        public AddGoldOnCombo(int goldAmount, ICombo combo, string name)
        {
            this.goldAmount = goldAmount;
            this.combo = combo;
            Name = name;
        }

        public async UniTask OnComboEvaluated()
        {
            if (G.battleScene._currentCombo.Name != combo.Name) return;
            
            G.state.gold += goldAmount;
            await artifact.Apply(Animate());
        }

        private async UniTask Animate()
        {
            await UniTask.WaitForSeconds(0.3f);
            G.audio.Play<ButtonSFX>();
            G.battleScene.ShowText(G.battleScene.artifactTextSpawnPosition.position, $"{Name}\n <color=#FABC76>+${goldAmount}</color>", true);
            G.battleScene.UpdateGoldView();
            await UniTask.WaitForSeconds(0.5f);
        }

        public string Description => $"Get <color=#FABC76>${goldAmount}</color> after playing {combo.Name}";
        public IArtifact artifact => this;
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

        public string Description => $"{color.ToDescription()} soap gives <color=#00B6F7>{additionalScore}</color> extra score";
        public IArtifact artifact => this;
        public string Name { get; }
    }
    
    public class AdditionalScore : IArtifact, ICardValueModifier
    {
        public int additionalScore;
        public string Name { get; }

        public AdditionalScore(int additionalScore, string name)
        {
            this.additionalScore = additionalScore;
            Name = name;
        }

        public async UniTask<int> ModifyCardValue(Card card, int value)
        {
            await artifact.Apply(Animate());
            return additionalScore + value;
        }

        private async UniTask Animate()
        {
            await UniTask.WaitForSeconds(0.3f);
            G.audio.Play<ButtonSFX>();
            G.battleScene.ShowText(G.battleScene.artifactTextSpawnPosition.position, $"{Name}\n+{additionalScore}", true);
            await UniTask.WaitForSeconds(0.5f);
        }

        public string Description => $"All soap gives {additionalScore} extra score";
        public IArtifact artifact => this;
    }
}