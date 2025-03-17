using EOAE_Code.Agents;
using EOAE_Code.Extensions;
using EOAE_Code.Magic.Spells;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;

namespace EOAE_Code.Magic
{
    public class SpellSelectorVM : ViewModel
    {
        [DataSourceProperty]
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (value != isVisible)
                {
                    isVisible = value;
                    OnPropertyChangedWithValue(value);
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<EquipmentActionItemVM> Spells => spells;

        [DataSourceProperty]
        public string HoveredItemText => hoveredItem?.ActionText ?? "";

        private bool isVisible;
        private MBBindingList<EquipmentActionItemVM> spells = new();
        private EquipmentActionItemVM? hoveredItem;

        public void OnToggle(bool open)
        {
            IsVisible = open;
            Spells.ApplyActionOnAllItems(vm => vm.OnFinalize());
            Spells.Clear();

            if (open)
            {
                hoveredItem = null;
                Spells.Add(
                    new EquipmentActionItemVM(
                        new TextObject("{=dZh6ZLIr}Cancel").ToString(),
                        "None",
                        null,
                        OnItemHovered
                    )
                );

                var character = Agent.Main?.Character as CharacterObject;
                if (character != null)
                {
                    character
                        .HeroObject.GetPickedSpells()
                        .ForEach(spell =>
                            Spells.Add(
                                new EquipmentActionItemVM(
                                    spell.Name,
                                    spell.Icon,
                                    spell,
                                    OnItemHovered
                                )
                            )
                        );
                }
            }
            else if (hoveredItem is { Identifier: Spell spell })
            {
                Agent.Main?.EquipSpell(spell);
            }

            RefreshValues();
        }

        private void OnItemHovered(EquipmentActionItemVM item)
        {
            hoveredItem = item;
            OnPropertyChangedWithValue(HoveredItemText, nameof(HoveredItemText));
        }
    }
}
