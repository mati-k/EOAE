﻿using System.Collections.Generic;
using EOAE_Code.Data.Loaders;
using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml.Spells;
using TaleWorlds.Library;

namespace EOAE_Code.Magic;

public class MagicDebugCommands
{
    [CommandLineFunctionality.CommandLineArgumentFunction("reload", "magic")]
    public static string ReloadSpells(List<string> args)
    {
        SpellManager.Clear();
        XmlDataLoader.LoadXmlDataCustomRoot<SpellData, SpellManager, SpellListData>("spells.xml");
        return "Spells reloaded.";
    }
}
