namespace _Project.Source.Util
{
    public class PlayerHand : CardZone
    {
        public override void Claim(InteractiveCard toClaim)
        {
            base.Claim(toClaim);
            spacing = 1120 / objects.Count;
            G.battleScene.UpdateHandSize();
        }
    }
}