namespace _Project.Data
{
    public class LevelEntity : CMSEntity
    {
    }

    public class Level1 : LevelEntity
    {
        public Level1()
        {
            Define<TagListChallenges>().all.Add(E.Id<Enemy1>());
            Define<TagReward>().gold = 3;
        }
    }

    public class Level2 : LevelEntity
    {
        public Level2()
        {
            Define<TagListChallenges>().all.Add(E.Id<Enemy2>());
            Define<TagReward>().gold = 5;
        }
    }

    public class Level3 : LevelEntity
    {
        public Level3()
        {
            Define<TagListChallenges>().all.Add(E.Id<Enemy2>());
            Define<TagListChallenges>().all.Add(E.Id<Enemy3>());
            Define<TagReward>().gold = 8;
        }
    }

    public class Level4 : LevelEntity
    {
        public Level4()
        {
            Define<TagListChallenges>().all.Add(E.Id<EnemySink125>());
            Define<TagReward>().gold = 3;
        }
    }


    public class Level5 : LevelEntity
    {
        public Level5()
        {
            Define<TagListChallenges>().all.Add(E.Id<EnemySink125>());
            Define<TagListChallenges>().all.Add(E.Id<EnemyBlue3>());
            Define<TagReward>().gold = 5;
        }
    }
    
    public class Level6 : LevelEntity
    {
        public Level6()
        {
            Define<TagListChallenges>().all.Add(E.Id<EnemySink250>());
            Define<TagListChallenges>().all.Add(E.Id<Monochrome>());
            Define<TagReward>().gold = 8;
        }
    }
    
    public class Level7 : LevelEntity
    {
        public Level7()
        {
            Define<TagListChallenges>().all.Add(E.Id<EnemySink1000>());
            Define<TagListChallenges>().all.Add(E.Id<Monochrome>());
            Define<TagListChallenges>().all.Add(E.Id<Enemy2>());
            Define<TagReward>().gold = 10;
        }
    }
    
    public class Level8 : LevelEntity
    {
        public Level8()
        {
            Define<TagListChallenges>().all.Add(E.Id<EnemySink9999>());
            Define<TagReward>().gold = 10;
        }
    }

    public class LevelLast : LevelEntity
    {
        public LevelLast()
        {
            Define<TagListChallenges>().all.Add(E.Id<LastEnemy>());
            Define<TagReward>().gold = 999;
        }
    }
}