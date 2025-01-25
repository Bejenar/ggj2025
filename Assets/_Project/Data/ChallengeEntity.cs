using System;
using System.Collections.Generic;
using _Project.Source.Util;
using Cysharp.Threading.Tasks;

namespace _Project.Data
{
    public class LevelEntity : CMSEntity
    {
    }
    
    public class ChallengeEntity : CMSEntity
    {
    }
    
    [Serializable]
    public class TagListChallenges : EntityComponentDefinition
    {
        public List<string> all = new List<string>();
    }
    
    [Serializable]
    public class TagScoreTask : EntityComponentDefinition
    {
        public int score;
    }
    
    [Serializable]
    public class TagPatternTask : EntityComponentDefinition
    {
        public CardColor color;
        public int count;
    }
    
    [Serializable]
    public class TagGameCompletedTask : EntityComponentDefinition
    {
    }
    
    [Serializable]
    public class TagReward : EntityComponentDefinition
    {
        public int gold;
    }
    
    public class Level1 : LevelEntity {
        public Level1()
        {
            Define<TagListChallenges>().all.Add(E.Id<Enemy1>());
            Define<TagReward>().gold = 5;
        }
    }
    
    public class Level2 : LevelEntity {
        public Level2()
        {
            Define<TagListChallenges>().all.Add(E.Id<Enemy2>());
            Define<TagReward>().gold = 10;
        }
    }
    
    public class Level3 : LevelEntity {
        public Level3()
        {
            Define<TagListChallenges>().all.Add(E.Id<Enemy2>());
            Define<TagListChallenges>().all.Add(E.Id<Enemy3>());
            Define<TagReward>().gold = 10;
        }
    }
    
    public class LevelLast : LevelEntity {
        public LevelLast()
        {
            Define<TagListChallenges>().all.Add(E.Id<LastEnemy>());
            Define<TagReward>().gold = 999;
        }
    }

    public class LastEnemy : ChallengeEntity
    {
        public LastEnemy()
        {
            Define<TagGameCompletedTask>();
        }
    }
    
    public class Enemy1 : ChallengeEntity
    {
        public Enemy1()
        {
            Define<TagScoreTask>().score = 75;
        }
    }
    
    public class Enemy2 : ChallengeEntity
    {
        public Enemy2()
        {
            Define<TagScoreTask>().score = 45;
            Define<TagPatternTask>().color = CardColor.RED;
            Define<TagPatternTask>().count = 2;
        }
    }
    
    public class Enemy3 : ChallengeEntity
    {
        public Enemy3()
        {
            Define<TagScoreTask>().score = 20;
            Define<TagPatternTask>().color = CardColor.NO_COLOR;
            Define<TagPatternTask>().count = 3;
        }
    }
}