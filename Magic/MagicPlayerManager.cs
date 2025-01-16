using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EOAE_Code.Data.Loaders;
using EOAE_Code.Data.Managers;

namespace EOAE_Code.Magic
{
    public class MagicPlayerManager
    {
        private static int currentSpellIndex = 0;

        public static void SwitchPlayerSpell(int change)
        {
            currentSpellIndex =
                (currentSpellIndex + change + SpellManager.GetSpellCount())
                % SpellManager.GetSpellCount();
        }

        public static int GetPlayerSpellIndex()
        {
            return currentSpellIndex;
        }
    }
}
