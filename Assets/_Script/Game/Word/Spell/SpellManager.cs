using System.Collections.Generic;

namespace BHS.AcidRain.Game
{
    public class SpellManager
    {
        public SpellManager(WordManager owner)
        {
            WordManager = owner;
            InstantiateSpellDictionary();
        }


        public WordManager WordManager { get; private set; }
        public Dictionary<string, Spell> SpellDictionary = new();
        private readonly string _ATTACK_RANK_1_LEVEL_1 = "1등공격1";
        private readonly string _ATTACK_RANK_1_LEVEL_2 = "1등공격2";
        private readonly string _ATTACK_RANK_1_LEVEL_3 = "1등공격3";
        private readonly string _ATTACK_RANK_1_LEVEL_4 = "1등공격4";
        private readonly string _ATTACK_RANK_1_LEVEL_5 = "1등공격5";

        private readonly string _ATTACK_RANK_LAST_LEVEL_1 = "꼴등공격1";
        private readonly string _ATTACK_RANK_LAST_LEVEL_2 = "꼴등공격2";
        private readonly string _ATTACK_RANK_LAST_LEVEL_3 = "꼴등공격3";
        private readonly string _ATTACK_RANK_LAST_LEVEL_4 = "꼴등공격4";
        private readonly string _ATTACK_RANK_LAST_LEVEL_5 = "꼴등공격5";

        private readonly string _HEAL_LEVEL_1 = "회복1";
        private readonly string _HEAL_LEVEL_2 = "회복2";
        private readonly string _HEAL_LEVEL_3 = "회복3";
        private readonly string _HEAL_LEVEL_4 = "회복4";
        private readonly string _HEAL_LEVEL_5 = "회복5";

        private readonly string _HIGH_VALUE_LEVEL_1 = "고득점1";
        private readonly string _HIGH_VALUE_LEVEL_2 = "고득점2";
        private readonly string _HIGH_VALUE_LEVEL_3 = "고득점3";
        private readonly string _HIGH_VALUE_LEVEL_4 = "고득점4";
        private readonly string _HIGH_VALUE_LEVEL_5 = "고득점5";


        private void InstantiateSpellDictionary()
        {
            string[] AttackFirst =
                {
                _ATTACK_RANK_1_LEVEL_1,
                _ATTACK_RANK_1_LEVEL_2,
                _ATTACK_RANK_1_LEVEL_3,
                _ATTACK_RANK_1_LEVEL_4,
                _ATTACK_RANK_1_LEVEL_5
            };
            string[] AttackLast =
            {
                _ATTACK_RANK_LAST_LEVEL_1,
                _ATTACK_RANK_LAST_LEVEL_2,
                _ATTACK_RANK_LAST_LEVEL_3,
                _ATTACK_RANK_LAST_LEVEL_4,
                _ATTACK_RANK_LAST_LEVEL_5
            };
            string[] Heal =
            {
                _HEAL_LEVEL_1,
                _HEAL_LEVEL_2,
                _HEAL_LEVEL_3,
                _HEAL_LEVEL_4,
                _HEAL_LEVEL_5
            };

            string[] High =
            {
                _HIGH_VALUE_LEVEL_1,
                _HIGH_VALUE_LEVEL_2,
                _HIGH_VALUE_LEVEL_3,
                _HIGH_VALUE_LEVEL_4,
                _HIGH_VALUE_LEVEL_5
            };

            for (int ix = 0; ix < 5; ix++)
            {
                SpellDictionary.Add(AttackFirst[ix], new AttackFirst(ix + 1, this));
                SpellDictionary.Add(AttackLast[ix], new AttackLast(ix + 1, this));
                SpellDictionary.Add(Heal[ix], new HealMe(ix + 1, this));
                SpellDictionary.Add(High[ix], new MakePreciousWord(ix + 1, this));
            }
        }
    }
}
