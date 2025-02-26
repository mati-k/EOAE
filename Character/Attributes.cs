using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace EOAE_Code.Character
{
    public class Attributes
    {
        private static Attributes? instance;

        public static Attributes Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Attributes();
                }

                return instance;
            }
        }

        public CharacterAttribute Magic { get; private set; }

        public void Initialize()
        {
            Magic = Game.Current.ObjectManager.RegisterPresumedObject(
                new CharacterAttribute("magic")
            );
            Magic.Initialize(
                new TextObject("{=1XUiDuRD}Magic"),
                new TextObject("{=ssYqIdga}Magic is cool thing allowing you to do cool things."),
                new TextObject("{=4ZAQ8Q7P}MAG")
            );
        }
    }
}
