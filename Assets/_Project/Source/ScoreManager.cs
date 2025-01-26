using System.Collections.Generic;
using _Project.Source.Util;
using Cysharp.Threading.Tasks;

namespace _Project.Source
{
    public class ScoreManager
    {
        private static ICombo DefaultCombo = new NoCombo();

        private List<ICombo> Combos = new List<ICombo>
        {
            new MonochromeCombo(),
            new TwoColorCombo(),
            new ThreeColorCombo(),
            new FourColorCombo(),
            new FiveColorCombo(),
            new EmptyCombo()
        };
        
        public async UniTask<float> CalculateScore(List<Card> cards)
        {
            var combo = await EvaluateComboType(cards);
            var baseScore = CalculateBaseScore(cards);
            
            return baseScore * combo.Multiplier;
        }
        
        private float CalculateBaseScore(List<Card> cards)
        {
            float score = 0;
            foreach (var card in cards)
            {
                var cardScore = card.baseScore;
                score += cardScore;
            }

            return score;
        }
        
        public async UniTask<ICombo> EvaluateComboType(List<Card> cards)
        {
            var evaluatedCombo = DefaultCombo;
            
            foreach (var combo in Combos)
            {
                if (combo.IsMatch(cards))
                {
                    evaluatedCombo = combo;
                }
            }

            foreach (var interactor in G.interactor.FindAll<IOnComboEvaluatedInteractor>())
            {
                await interactor.OnComboEvaluated(evaluatedCombo);
            }
            return evaluatedCombo;
        }
    }

    public interface IOnCardScored
    {
        UniTask OnCardScored(Card card, float score);
    }

    public interface IOnComboEvaluatedInteractor
    {
        UniTask OnComboEvaluated(ICombo combo);
    }
    
    public class ScoreManagerView : BaseInteraction, IOnComboEvaluatedInteractor
    {
        public UniTask OnComboEvaluated(ICombo combo)
        {
            // G.battleScene.UpdateComboView(combo.Name);
            return UniTask.CompletedTask;
        }
    }
}