using System.Collections.Generic;

namespace BHS.AcidRain.Game
{
    public class SpellManager
    {
        public SpellManager(WordManager owner)
        {
            _wordManager = owner;
            InstantiateSpellDictionary();
        }


        private WordManager _wordManager;
        public Dictionary<string, Spell> SpellDictionary = new();

        private void InstantiateSpellDictionary()
        {
            SpellDictionary.Add("1번디버그를찍어다오",new DebugSpell1());
            SpellDictionary.Add("2번디버그를찍어다오", new DebugSpell2());
        }
    }
}
