using Photon.Pun;
using Photon.Realtime;

namespace BHS.AcidRain.Game
{
    public class AttackLast : Spell
    {
        public AttackLast(int comboNeed, SpellManager spellholder) : base(comboNeed, spellholder)
        {

        }

        public override void TryUseSpell(int inputCombo, Player spellUser, PhotonView targetSpawner)
        {
            if (JudgeSpell(inputCombo, comboNeeded))
            {
                //Select Target.
                //return Player.
                Player targetPlayer
                    = ScoreBoard.FindRankedPlayer(0);

                //bool isPublic, int spellLevel, int Loops, bool isSpellWord, int score(choice), float speed(choice)
                OwnerSpellManager.WordManager.AdjustCombo(-comboNeeded);
                targetSpawner.RPC("OrderSpawnerSpawnWord", targetPlayer, false, comboNeeded, 1, true, 0, 1f);
            }
            else
            {
                UnityEngine.Debug.Log("not enough combo");
            }
        }
    }
}
