using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    public class DebugSpell2 : Spell
    {
        public DebugSpell2()
        {
            comboNeeded = 2;
        }


        private int comboNeeded;

        public override void TryUseSpell(int inputCombo, Player spellUser, PhotonView spellSpawner)
        {
            base.TryUseSpell(inputCombo, spellUser, spellSpawner);
            if (JudgeSpell(inputCombo, comboNeeded))
            {
                Debug.Log("SPELL TWO USED!");
            }
        }
    }
}
