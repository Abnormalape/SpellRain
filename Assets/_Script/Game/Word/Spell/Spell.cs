using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    public abstract class Spell
    {

        public virtual void TryUseSpell(int combo, Player spellUser, PhotonView spellSpawner)
        {
            Debug.Log("Try Use Spell!");
        }

        public bool JudgeSpell(int combo, int comboNeeded)
        {
            if (combo >= comboNeeded)
            {
                Debug.Log("Spell Used!");
                return true;
            }
            else
            {
                Debug.Log("Spell Not Used!");
                return false;
            }
        }
    }
}
