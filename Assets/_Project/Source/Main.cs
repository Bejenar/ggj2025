using System.Collections.Generic;
using _Project.Source.Artifacts;
using _Project.Source.Util;
using _Project.Source.Village.UI;
using JamBootstrap.jam_bootstrap.Runtime;
using UnityEngine;

namespace _Project.Source
{
    public class ServicedMain : AbstractServicedMain
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InstantiateAutoSaveSystem()
        {
            InstantiateAutoSaveSystem<ServicedMain>();
        }

        protected override void Awake()
        {
            base.Awake();

            G.fader = gameObject.AddComponent<ScreenFader>();
            G.main = new GameManager(Resources.Load<CardTooltip>("Prefabs/CardTooltip"));
            G.deckGenerator = new DeckGenerator();
            G.cardSpriteResolver = Resources.Load<CardSpriteResolver>("SO/CardSpriteMap");
            G.scoreManager = new ScoreManager();
            G.levelManager = new LevelManager();

            G.audio.MusicVolume = 0.2f;

            // if no safe
            G.main.InitRunState();
        }
    }

    public class G : Core
    {
        public static GameManager main;
        public static ScreenFader fader;
        public static DeckGenerator deckGenerator;
        public static CardSpriteResolver cardSpriteResolver;
        public static ScoreManager scoreManager;
        public static LevelManager levelManager;
        public static ShopManager shopManager;
        public static ParticleManager particles;

        public static InteractiveCard dragCard;

        public static RunState state;

        // Scene Managers
        public static BattleScene battleScene;

        public static bool animationsEnabled = true;
    }

    public class RunState
    {
        public Deck deck;

        public int gold;
        public int maxHandSize;
        public int maxPlays;
        public int maxDiscards;
        public int level;
        public List<IArtifact> artifacts = new List<IArtifact>();
    }
}