using EOAE_Code.Data.Managers;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;
using TaleWorlds.ObjectSystem;

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
                Spells.Add(new EquipmentActionItemVM("Cancel", "None", null, OnItemHovered));
                SpellManager
                    .GetAllSpell()
                    .ForEach(spell =>
                        Spells.Add(
                            new EquipmentActionItemVM(spell.Name, spell.Icon, spell, OnItemHovered)
                        )
                    );
            }
            else if (hoveredItem is { Identifier: Spell spell })
            {
                var player = Agent.Main;
                var spellObject = MBObjectManager.Instance.GetObject<ItemObject>(spell.ItemName);
                var spellWeapon = new MissionWeapon(spellObject, null, null);

                player.EquipWeaponWithNewEntity(EquipmentIndex.ExtraWeaponSlot, ref spellWeapon);
                player.TryToWieldWeaponInSlot(
                    EquipmentIndex.ExtraWeaponSlot,
                    Agent.WeaponWieldActionType.Instant,
                    false
                );
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
