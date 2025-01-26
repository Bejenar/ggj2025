using _Project.Data;
using _Project.Source.Util;
using Cysharp.Threading.Tasks;

namespace _Project.Source.Artifacts
{
    public class MultPerDiscard : IArtifact, IAfterOnComboEvaluated
    {
        public float multStep;
        public string Name { get; }

        public MultPerDiscard(float multStep, string name)
        {
            this.multStep = multStep;
            Name = name;
        }

        public async UniTask OnComboEvaluated()
        {
            float baseMult = 1;
            baseMult += G.battleScene.discards * multStep;
            
            G.battleScene.score *= baseMult;
            await artifact.Apply(Animate(baseMult));
        }

        private async UniTask Animate(float mult)
        {
            await UniTask.WaitForSeconds(0.3f);
            G.audio.Play<ButtonSFX>();
            G.battleScene.ShowText(G.battleScene.artifactTextSpawnPosition.position, $"{Name}\nx{mult:F1}", true);
            G.battleScene.UpdateScoreView();
            await UniTask.WaitForSeconds(0.5f);
        }

        public string Description => $"Gain <color=#EA4345><b>x{multStep:F1}</b> mult</color> per discard";
        public IArtifact artifact => this;
    }
}