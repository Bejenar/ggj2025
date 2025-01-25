using System.Collections.Generic;
using System.Linq;
using _Project.Source.Util;
using UnityEngine;

namespace _Project.Source.Challenges
{
    
    public interface ITask
    {
        string BaseDescription { get; }
        bool Evaluate();
        
        bool Completed { get; }

        string GetDescription()
        {
            return BaseDescription;
        }
    }
    
    public class PatternTask : ITask
    {
        public CardColor targetColor;
        public int count;
        
        public bool Completed => Evaluate();
        
        public string BaseDescription => $"Must use\n <b>{count} {targetColor.ToDescription()}<b>";
        
        public PatternTask(CardColor targetColor, int count)
        {
            this.targetColor = targetColor;
            this.count = count;
        }
        
        public bool Evaluate()
        {
            return G.battleScene.selected.Count(ic => ic.card.color == targetColor) >= count;
        }
    }

    public class ScoreTask : ITask
    {
        public float score;
        public float targetScore;

        public bool completed;
        
        public float remainingScore => Mathf.Max(0, targetScore - score);
        
        public bool Completed => completed;
        
        public string BaseDescription => $"Score\n<style=score>{remainingScore:F0}</style>";

        public ScoreTask(float targetScore)
        {
            this.targetScore = targetScore;
        }
        
        public bool Evaluate()
        {
            score += G.battleScene.Score;
            completed = score >= targetScore;
            return completed;
        }
    }

    public class GameCompletedTask : ITask
    {
        public string BaseDescription => "Thank you for playing!";
        public bool Evaluate()
        {
            return false;
        }

        public bool Completed => false;
    }

    public class ScoreAndPatternTask : ITask
    {
        public ScoreTask scoreTask;
        public PatternTask patternTask;
        
        public bool Completed => scoreTask.Completed && patternTask.Completed;
        
        public string BaseDescription => $"{scoreTask.BaseDescription}\n{patternTask.BaseDescription}";
        
        public ScoreAndPatternTask(ScoreTask scoreTask, PatternTask patternTask)
        {
            this.scoreTask = scoreTask;
            this.patternTask = patternTask;
        }
        
        public bool Evaluate()
        {
            bool result = scoreTask.Evaluate() && patternTask.Evaluate();

            if (!result)
            {
                scoreTask.score = 0;
            }

            return result;
        }
    }
    
    
}