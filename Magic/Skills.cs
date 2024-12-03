﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace EOAE_Code.Magic
{
    public class Skills
    {
        private static Skills _instance;
        public static Skills Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Skills();
                }

                return _instance;
            }
        }


        public SkillObject Destruction { get; private set; }
        public SkillObject Restoration { get; private set; }
        public SkillObject Conjuration { get; private set; }

        public void Initialize()
        {
            Destruction = Game.Current.ObjectManager.RegisterPresumedObject(new SkillObject("Destruction"));
            Restoration = Game.Current.ObjectManager.RegisterPresumedObject(new SkillObject("Restoration"));
            Conjuration = Game.Current.ObjectManager.RegisterPresumedObject(new SkillObject("Conjuration"));
            Destruction.Initialize(new TextObject("{=!}Destruction", null), new TextObject("{=!}Destruction, break shit!.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(Attributes.Instance.Magic);
            Restoration.Initialize(new TextObject("{=!}Restoration", null), new TextObject("{=!}Restoration, fix shit!.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(Attributes.Instance.Magic);
            Conjuration.Initialize(new TextObject("{=!}Conjuration", null), new TextObject("{=!}Conjuration, summon shit!.", null), SkillObject.SkillTypeEnum.Personal).SetAttribute(Attributes.Instance.Magic);
        }
    }
}
