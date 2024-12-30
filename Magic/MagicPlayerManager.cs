using EOAE_Code.Data.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOAE_Code.Magic
{
    public class MagicPlayerManager
    {
        private static int currentSpellIndex = 0;

        public static void SwitchPlayerSpell(int change)
        {
            currentSpellIndex = (currentSpellIndex + change + SpellLoader.GetSpellCount()) % SpellLoader.GetSpellCount();
        }

        public static int GetPlayerSpellIndex()
        {
            return currentSpellIndex;
        }
    }
}
