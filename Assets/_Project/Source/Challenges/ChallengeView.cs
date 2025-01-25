using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Data;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Source.Challenges
{
    public class ChallengeView : MonoBehaviour
    {
        public TMP_Text descriptionText;
        public CanvasGroup done;
        public Image checkmark;
        private List<ITask> tasks;

        public bool completed;
        
        private List<Material> _materials;
        
        TextAnimation _textAnimation;
        
        private void Start()
        {
            _materials = new List<Material>(GetComponentsInChildren<Image>().Select(i =>
            {
                var mat = new Material(i.material);
                i.material = mat;
                return mat;
            }));
            _textAnimation = descriptionText.AddComponent<TextAnimation>();
        }

        public void Init(List<ITask> tasks)
        {
            this.tasks = tasks;
            done.DOFade(0, 0);
            UpdateView();
        }
        
        public void UpdateView()
        {
            descriptionText.text = string.Join("\n", tasks.ConvertAll(task => task.GetDescription()));
            
            if (completed)
            {
                G.audio.Play<SuccessSFX>();
                done.DOFade(1, 0.1f);
                checkmark.transform.localScale = Vector3.zero;
                checkmark.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            }
        }

        public void Evaluate()
        {
            foreach (var task in tasks)
            {
                if (task.Completed)
                    continue;

                if (!task.Evaluate())
                {
                    G.audio.Play<HitSFX>();
                    transform.DOShakeRotation(0.2f, Vector3.forward * 5, vibrato:10, randomness:45, randomnessMode: ShakeRandomnessMode.Harmonic);
                    return;
                }
            }

            completed = true;
        }

        public void Dissolve()
        {
            Debug.Log("Dissolving");

            descriptionText.transform.DOScale(Vector3.zero, 0.2f);
            G.audio.Play<BurningSFX>();
            foreach (var mat in _materials)
            {
                mat.DOFloat(0, "_Dissolve", 1f).OnComplete(() => Destroy(gameObject));
            }
        }
    }
}