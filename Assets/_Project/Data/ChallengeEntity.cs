using System;
using System.Collections.Generic;
using _Project.Source.Util;
using Cysharp.Threading.Tasks;

namespace _Project.Data
{
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
    
    public class EnemySink125 : ChallengeEntity
    {
        public EnemySink125()
        {
            Define<TagScoreTask>().score = 125;
        }
    }
    
    public class EnemySink250 : ChallengeEntity
    {
        public EnemySink250()
        {
            Define<TagScoreTask>().score = 250;
        }
    }
    
    public class EnemySink1000 : ChallengeEntity
    {
        public EnemySink1000()
        {
            Define<TagScoreTask>().score = 550;
        }
    }
    
    public class EnemySink9999 : ChallengeEntity
    {
        public EnemySink9999()
        {
            Define<TagScoreTask>().score = 2000;
        }
    }
    
    public class EnemyBlue3 : ChallengeEntity
    {
        public EnemyBlue3()
        {
            Define<TagScoreTask>().score = 50;
            Define<TagPatternTask>().color = CardColor.BLUE;
            Define<TagPatternTask>().count = 3;
        }
    }
    
    public class Monochrome : ChallengeEntity
    {
        public Monochrome()
        {
            Define<TagScoreTask>().score = 100;
            Define<TagPatternTask>().color = CardColor.NO_COLOR;
            Define<TagPatternTask>().count = 2;
        }
    }
}