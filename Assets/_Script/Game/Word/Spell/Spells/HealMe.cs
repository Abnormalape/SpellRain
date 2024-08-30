using Photon.Pun;
using Photon.Realtime;

namespace BHS.AcidRain.Game
{
    public class HealMe : Spell
    {
        public HealMe(int comboNeed, SpellManager spellholder) : base(comboNeed, spellholder)
        {

        }

        public override void TryUseSpell(int inputCombo, Player spellUser, PhotonView targetSpawner)
        {
            if (JudgeSpell(inputCombo, comboNeeded))
            {
                OwnerSpellManager.WordManager.AdjustHP(comboNeeded);
                OwnerSpellManager.WordManager.AdjustCombo(-comboNeeded);
            }
            else
            {
                UnityEngine.Debug.Log("not enough combo");
            }
        }
    }
}
