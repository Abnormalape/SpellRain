using Photon.Pun;
using Photon.Realtime;

namespace BHS.AcidRain.Game
{
    public class MakePreciousWord : Spell
    {
        public MakePreciousWord(int comboNeed, SpellManager spellholder) : base(comboNeed, spellholder)
        {

        }

        public override void TryUseSpell(int inputCombo, Player spellUser, PhotonView targetSpawner)
        {
            if (JudgeSpell(inputCombo, comboNeeded))
            {
                //Select Target.
                //return Player.

                //bool isPublic, int spellLevel, int Loops, bool isSpellWord, int score(0), float speed(1f)
                OwnerSpellManager.WordManager.AdjustCombo(-comboNeeded);
                targetSpawner.RPC("OrderSpawnerSpawnWord", RpcTarget.MasterClient, true, comboNeeded, 1, true, comboNeeded * 1000, 1f);
            }
            else
            {
                UnityEngine.Debug.Log("not enough combo");
            }
        }
    }
}
