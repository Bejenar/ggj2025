using System;
using _Project.Source.Util;
using Engine.Math;
using TMPro;

namespace _Project.Source.Village.UI
{
    public class CardTooltip : BaseTooltip
    {
        public TMP_Text text;
        private InteractiveCard card;

        public void Init(InteractiveCard card)
        {
            this.card = card;
            this.text.text = $"<size=150%>{card.card.color.ToDescription()} soap</size>\n" +
                             $"Gives <size=120%>{card.card.baseScore.ToString().Color("#64A2B9")}</size> score";
            card.OnPointerExitEvent += Hide;
        }

        private void OnDestroy()
        {
            card.OnPointerExitEvent -= Hide;
        }
    }
}