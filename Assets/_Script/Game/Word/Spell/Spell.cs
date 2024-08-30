using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    public abstract class Spell
    {
        public Spell(int comboNeeded, SpellManager ownerSpellManager)
        {
            this.comboNeeded = comboNeeded;
            OwnerSpellManager = ownerSpellManager;
            ScoreBoard = OwnerSpellManager.WordManager.ScoreBoardManagerInstance;
        }


        public int comboNeeded;
        public SpellManager OwnerSpellManager;
        public ScoreBoardManager ScoreBoard;


        public virtual void TryUseSpell(int combo, Player spellUser, PhotonView spellSpawner)
        {
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
