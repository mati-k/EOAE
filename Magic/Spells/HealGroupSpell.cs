﻿using EOAE_Code.Data.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public class HealGroupSpell : Spell
    {
        public override bool IsThrown { get { return false; } }

        public HealGroupSpell(SpellDataXml data) : base(data)
        {

        }

        public override void Cast(Agent caster)
        {
            throw new NotImplementedException();
        }
    }
}