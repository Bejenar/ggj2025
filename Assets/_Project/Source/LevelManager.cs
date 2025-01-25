using System.Collections.Generic;
using System.Linq;
using _Project.Data;
using _Project.Source.Challenges;
using UnityEngine;

namespace _Project.Source
{
    public class LevelManager
    {
        List<string> levels = new List<string>
        {
            E.Id<Level1>(),
            E.Id<Level2>(),
            E.Id<LevelLast>(),
        };

        public LevelManager()
        {
        }

        public int GetLevelReward()
        {
            var level = GetCurrentLevel();
            return level.Get<TagReward>().gold;
        }

        public List<CMSEntity> GetChallengesFromCurrentLevel()
        {
            var level = G.levelManager.GetCurrentLevel();

            return GetChallengesFromLevel(level);
        }

        private CMSEntity GetCurrentLevel()
        {
            G.state.level = Mathf.Min(G.state.level, levels.Count - 1);

            return CMS.Get<CMSEntity>(levels[G.state.level]);
        }

        private List<CMSEntity> GetChallengesFromLevel(CMSEntity level)
        {
            return level.Get<TagListChallenges>().all
                .Select(ch => CMS.Get<CMSEntity>(ch))
                .ToList();
        }

        public List<ITask> GetTasksFromChallenge(CMSEntity challenge)
        {
            var tasks = new List<ITask>();

            if (challenge.Is(out TagScoreTask tst) && challenge.Is(out TagPatternTask tpt))
            {
                var scoreTask = new ScoreTask(tst.score);
                var patternTask = new PatternTask(tpt.color, tpt.count);
                tasks.Add(new ScoreAndPatternTask(scoreTask, patternTask));
            }
            else if (challenge.Is(out TagScoreTask tagScoreTask))
            {
                tasks.Add(new ScoreTask(tagScoreTask.score));
            }
            else if (challenge.Is(out TagPatternTask tagPatternTask))
            {
                tasks.Add(new PatternTask(tagPatternTask.color, tagPatternTask.count));
            }
            
            if (challenge.Is(out TagGameCompletedTask tagTimeTask))
            {
                tasks.Add(new GameCompletedTask());
            }

            return tasks;
        }
    }
}