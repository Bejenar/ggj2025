using _Project.Data;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Source.Cutscene
{
    public class Cutscene : MonoBehaviour
    {
        public Image swordImage;
        public Image revealImage;
        
        public TMP_Text text;

        public async void Start()
        {
            await G.smartWait.SmartWait(3f);
         
            await Print(text, "The children are in danger");
            G.audio.Play<CutsceneMusic>();
            await G.smartWait.SmartWait(5f);
            
            await Print(text, "Save them, oh the chosen one");
            await G.smartWait.SmartWait(3f);
            await Print(text, "Here is your implement");
            await G.smartWait.SmartWait(2f);
            await text.DOFade(0, 1f);
            
            
            await swordImage.DOFade(1, 1f);
            
            await G.smartWait.SmartWait(2f);
            await G.smartWait.SmartWait(2f);

            G.audio.Stop(AudioType.Music);
            G.audio.Play<LightSwitchSFX>();
            await revealImage.DOFade(1, 0);

            await UniTask.WaitForSeconds(4);
            
            swordImage.gameObject.SetActive(false);
            await revealImage.DOFade(0, 2f);

            await SceneManager.LoadSceneAsync(1);
        }
        
        public static async UniTask Print(TMP_Text text, string actionDefinition, string fx = "wave")
        {
            var visibleLength = TextUtils.GetVisibleLength(actionDefinition);
            if (visibleLength == 0) return;
    
            for (var i = 0; i < visibleLength; i++)
            {
                text.text = $"<link={fx}>{TextUtils.CutSmart(actionDefinition, 1 + i)}</link>";
                G.audio.Play<TypeSFX>();
                await UniTask.WaitForSeconds(0.01f);
            }
        }
    }
}