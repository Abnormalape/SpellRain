using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    public class DebugSpell1 : Spell
    {
        public DebugSpell1()
        {
            comboNeeded = 2;
        }


        private int comboNeeded;

        public override void TryUseSpell(int inputCombo, Player spellUser, PhotonView spellSpawner)
        {
            base.TryUseSpell(inputCombo, spellUser, spellSpawner);
            if(JudgeSpell(inputCombo, comboNeeded))
            {
                Debug.Log("SPELL ONE USED!");
            }
            spellSpawner.RPC("OrderSpawnerSpawnWord", RpcTarget.All, "소환된단어", false);
        }
    }
}
