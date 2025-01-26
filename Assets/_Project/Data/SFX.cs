using Common;
using UnityEngine;

namespace _Project.Data
{
    public class SFX
    {
        
    }

    public class BubbleGunSFX : CMSEntity
    {
        public BubbleGunSFX()
        {
            Define<SFXTag>();
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/bubble_launch"));
        }
    }
    
    public class ShakingSFX : CMSEntity
    {
        public ShakingSFX()
        {
            Define<SFXTag>();
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/shaking_fast_liquid"));
        }
    }
    
    public class ButtonSFX : CMSEntity
    {
        public ButtonSFX()
        {
            Define<SFXTag>().VaryPitch = true;
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/Button"));
            Define<SFXArray>().volume = 0.5f;
        }
    }
    
    public class ButtonHighSFX : CMSEntity
    {
        public ButtonHighSFX()
        {
            Define<SFXTag>();
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/ButtonHigh"));
        }
    }
    
    public class ButtonLowSFX : CMSEntity
    {
        public ButtonLowSFX()
        {
            Define<SFXTag>();
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/ButtonLow"));
        }
    }
    
    
    public class CoinsSFX : CMSEntity
    {
        public CoinsSFX()
        {
            Define<SFXTag>().VaryPitch = true;
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/Coins"));
            Define<SFXArray>().volume = 0.5f;
        }
    }
    
    public class HitSFX : CMSEntity
    {
        public HitSFX()
        {
            Define<SFXTag>().VaryPitch = true;
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/taking_damage"));
            Define<SFXArray>().volume = 0.5f;
        }
    }
    
    public class BurningSFX : CMSEntity
    {
        public BurningSFX()
        {
            Define<SFXTag>().VaryPitch = true;
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/burning_effect"));
            Define<SFXArray>().volume = 0.5f;
        }
    }
    
    public class SuccessSFX : CMSEntity
    {
        public SuccessSFX()
        {
            Define<SFXTag>().VaryPitch = true;
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/successful_click"));
            Define<SFXArray>().volume = 0.5f;
        }
    }
    
    public class ArtifactSFX : CMSEntity
    {
        public ArtifactSFX()
        {
            Define<SFXTag>().VaryPitch = true;
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/artifact"));
            Define<SFXArray>().volume = 0.5f;
        }
    }
    
    public class WinSFX : CMSEntity
    {
        public WinSFX()
        {
            Define<SFXTag>().VaryPitch = true;
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/Win"));
            Define<SFXArray>().volume = 0.5f;
        }
    }
    
    
    public class LightSwitchSFX : CMSEntity
    {
        public LightSwitchSFX()
        {
            Define<SFXTag>().VaryPitch = true;
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/light_switch"));
            Define<SFXArray>().volume = 0.5f;
        }
    }
    
    public class TypeSFX : CMSEntity
    {
        public TypeSFX()
        {
            Define<SFXTag>().VaryPitch = true;
            Define<SFXArray>().files.Add(Resources.Load<AudioClip>("SFX/key_type"));
            Define<SFXArray>().volume = 0.5f;
        }
    }
    
    public class GameplayMusic : CMSEntity
    {
        public GameplayMusic()
        {
            Define<MusicTag>().clip = Resources.Load<AudioClip>("Music/Gameplay");
        }
    }
    
    public class CutsceneMusic : CMSEntity
    {
        public CutsceneMusic()
        {
            Define<MusicTag>().clip = Resources.Load<AudioClip>("Music/Cutscene");
        }
    }
}