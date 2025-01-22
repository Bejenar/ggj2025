using JamBootstrap.jam_bootstrap.Runtime;
using UnityEngine;

namespace _Project.Source
{
    public class Main : AbstractServicedMain
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InstantiateAutoSaveSystem()
        {
            InstantiateAutoSaveSystem<Main>();
        }

        protected override void Awake()
        {
            base.Awake();

            G.fader = gameObject.AddComponent<ScreenFader>();
        }
    }
    
    public class G : Core
    {
        public static Main main;
        public static ScreenFader fader;
    }
}