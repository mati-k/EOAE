using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;
using EOAE_Code.States;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core;

namespace EOAE_Code.Character
{
    [ViewModelMixin]
    public class CharacterDeveloperVMMixin : BaseViewModelMixin<CharacterDeveloperVM>
    {
        public CharacterDeveloperVMMixin(CharacterDeveloperVM vm)
            : base(vm) { }

        [DataSourceMethod]
        public void ExecuteOpenSpellbook()
        {
            var manager = Game.Current.GameStateManager;
            manager.PushState(manager.CreateState<SpellbookState>());
        }
    }
}
