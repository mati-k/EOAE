using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.States
{
    // Extracted from native character screen into reusable component
    public class CharacterSwitcherVM : ViewModel
    {
        private List<Hero> heroList = new();
        private int heroIndex = 0;
        private Func<string, TextObject> getKeyTextFromKeyId;
        private Action<Hero> onHeroSelected;

        private SelectorVM<SelectorItemVM> _characterList;
        private string _currentCharacterNameText;
        private BasicTooltipViewModel _previousCharacterHint;
        private BasicTooltipViewModel _nextCharacterHint;
        private InputKeyItemVM _previousCharacterInputKey;
        private InputKeyItemVM _nextCharacterInputKey;

        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> CharacterList
        {
            get { return this._characterList; }
            set
            {
                if (value != this._characterList)
                {
                    this._characterList = value;
                    base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(
                        value,
                        "CharacterList"
                    );
                }
            }
        }

        [DataSourceProperty]
        public string CurrentCharacterNameText
        {
            get { return this._currentCharacterNameText; }
            set
            {
                if (value != this._currentCharacterNameText)
                {
                    this._currentCharacterNameText = value;
                    base.OnPropertyChangedWithValue<string>(value, "CurrentCharacterNameText");
                }
            }
        }

        [DataSourceProperty]
        public BasicTooltipViewModel PreviousCharacterHint
        {
            get { return this._previousCharacterHint; }
            set
            {
                if (value != this._previousCharacterHint)
                {
                    this._previousCharacterHint = value;
                    base.OnPropertyChangedWithValue<BasicTooltipViewModel>(
                        value,
                        "PreviousCharacterHint"
                    );
                }
            }
        }

        [DataSourceProperty]
        public BasicTooltipViewModel NextCharacterHint
        {
            get { return this._nextCharacterHint; }
            set
            {
                if (value != this._nextCharacterHint)
                {
                    this._nextCharacterHint = value;
                    base.OnPropertyChangedWithValue<BasicTooltipViewModel>(
                        value,
                        "NextCharacterHint"
                    );
                }
            }
        }

        [DataSourceProperty]
        public InputKeyItemVM PreviousCharacterInputKey
        {
            get { return this._previousCharacterInputKey; }
            set
            {
                if (value != this._previousCharacterInputKey)
                {
                    this._previousCharacterInputKey = value;
                    base.OnPropertyChangedWithValue<InputKeyItemVM>(
                        value,
                        "PreviousCharacterInputKey"
                    );
                }
            }
        }

        [DataSourceProperty]
        public InputKeyItemVM NextCharacterInputKey
        {
            get { return this._nextCharacterInputKey; }
            set
            {
                if (value != this._nextCharacterInputKey)
                {
                    this._nextCharacterInputKey = value;
                    base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "NextCharacterInputKey");
                }
            }
        }

        public CharacterSwitcherVM(Action<Hero> onHeroSelected)
        {
            this.onHeroSelected = onHeroSelected;
            heroList = GetPartyHeroes();
            CharacterList = new SelectorVM<SelectorItemVM>(
                heroList.Select(hero => hero.Name.ToString()),
                heroIndex,
                OnCharacterSelection
            );
            CurrentCharacterNameText = heroList[heroIndex].Name.ToString();
        }

        private void SetPreviousCharacterHint()
        {
            this.PreviousCharacterHint = new BasicTooltipViewModel(
                delegate()
                {
                    GameTexts.SetVariable("HOTKEY", this.GetPreviousCharacterKeyText());
                    GameTexts.SetVariable(
                        "TEXT",
                        GameTexts.FindText("str_inventory_prev_char", null)
                    );
                    return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
                }
            );
        }

        private void SetNextCharacterHint()
        {
            this.NextCharacterHint = new BasicTooltipViewModel(
                delegate()
                {
                    GameTexts.SetVariable("HOTKEY", this.GetNextCharacterKeyText());
                    GameTexts.SetVariable(
                        "TEXT",
                        GameTexts.FindText("str_inventory_next_char", null)
                    );
                    return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
                }
            );
        }

        public void SetPreviousCharacterInputKey(HotKey hotKey)
        {
            this.PreviousCharacterInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
            this.SetPreviousCharacterHint();
        }

        public void SetNextCharacterInputKey(HotKey hotKey)
        {
            this.NextCharacterInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
            this.SetNextCharacterHint();
        }

        private TextObject GetPreviousCharacterKeyText()
        {
            if (this.PreviousCharacterInputKey == null || this.getKeyTextFromKeyId == null)
            {
                return TextObject.Empty;
            }
            return this.getKeyTextFromKeyId(this.PreviousCharacterInputKey.KeyID);
        }

        private TextObject GetNextCharacterKeyText()
        {
            if (this.NextCharacterInputKey == null || this.getKeyTextFromKeyId == null)
            {
                return TextObject.Empty;
            }
            return this.getKeyTextFromKeyId(this.NextCharacterInputKey.KeyID);
        }

        public void SetGetKeyTextFromKeyIDFunc(Func<string, TextObject> getKeyTextFromKeyId)
        {
            this.getKeyTextFromKeyId = getKeyTextFromKeyId;
        }

        private List<Hero> GetPartyHeroes()
        {
            var roster = MobileParty.MainParty.MemberRoster?.GetTroopRoster();

            if (roster == null)
            {
                return new List<Hero>();
            }

            return roster
                .Where(member => member.Character.HeroObject != null)
                .Select(member => member.Character.HeroObject)
                .ToList();
        }

        public override void OnFinalize()
        {
            base.OnFinalize();
        }

        private void OnCharacterSelection(SelectorVM<SelectorItemVM> newIndex)
        {
            if (
                newIndex.SelectedIndex >= 0
                && newIndex.SelectedIndex < this.heroList.Count
                && newIndex.SelectedIndex != heroIndex
            )
            {
                Hero previousHero = heroList[heroIndex];
                heroIndex = newIndex.SelectedIndex;
                SelectHero(heroList[heroIndex], previousHero);
            }
        }

        private void SelectHero(Hero newHero, Hero previousHero)
        {
            CurrentCharacterNameText = newHero.Name.ToString();
            onHeroSelected(previousHero);
        }

        public void RegisterHotKeys()
        {
            SetGetKeyTextFromKeyIDFunc(
                new Func<string, TextObject>(
                    Game.Current.GameTextManager.GetHotKeyGameTextFromKeyID
                )
            );
            SetPreviousCharacterInputKey(
                HotKeyManager
                    .GetCategory("GenericPanelGameKeyCategory")
                    .GetHotKey("SwitchToPreviousTab")
            );
            SetNextCharacterInputKey(
                HotKeyManager
                    .GetCategory("GenericPanelGameKeyCategory")
                    .GetHotKey("SwitchToNextTab")
            );
        }

        public void HandleHotKeyNavigation(GauntletLayer gauntletLayer)
        {
            if (gauntletLayer.Input.IsHotKeyDownAndReleased("SwitchToPreviousTab"))
            {
                CharacterList.ExecuteSelectPreviousItem();
            }
            else if (gauntletLayer.Input.IsHotKeyDownAndReleased("SwitchToNextTab"))
            {
                CharacterList.ExecuteSelectNextItem();
            }
        }

        public Hero GetCurrentHero()
        {
            return heroList[heroIndex];
        }
    }
}
