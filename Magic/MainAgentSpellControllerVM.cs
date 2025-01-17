using System;
using System.ComponentModel;
using EOAE_Code.Data.Managers;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Magic
{
    public class MainAgentSpellControllerVM : ViewModel
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

        private MBBindingList<EquipmentActionItemVM> spells = new();

        [DataSourceProperty]
        public MBBindingList<EquipmentActionItemVM> Spells => spells;

        [DataSourceProperty]
        public string HoveredItemText => hoveredItem?.ActionText ?? "";

        private EquipmentActionItemVM? hoveredItem;
        private bool isVisible;

        public MainAgentSpellControllerVM()
        {
            Spells.Add(new EquipmentActionItemVM("Cancel", "None", null, OnItemHovered));
            SpellManager
                .GetAllSpell()
                .ForEach(spell =>
                    Spells.Add(new EquipmentActionItemVM(spell.Name, "None", spell, OnItemHovered))
                );
        }

        public void Initialize()
        {
            Mission.Current.OnMainAgentChanged += OnMainAgentChanged;
            OnMainAgentChanged(null, null);
        }

        public void OnToggle(bool open)
        {
            IsVisible = open;
            Spells.ApplyActionOnAllItems(
                delegate(EquipmentActionItemVM o)
                {
                    o.OnFinalize();
                }
            );
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
            else
            {
                if (hoveredItem != null)
                {
                    InformationManager.DisplayMessage(
                        new InformationMessage("Using " + hoveredItem.ActionText)
                    );
                    var spell = hoveredItem.Identifier as Spell;
                    if (spell != null)
                    {
                        var player = Agent.Main;
                        var spellObject = MBObjectManager.Instance.GetObject<ItemObject>(
                            spell.ItemName
                        );
                        var spellWeapon = new MissionWeapon(spellObject, null, null);

                        player.EquipWeaponWithNewEntity(
                            EquipmentIndex.ExtraWeaponSlot,
                            ref spellWeapon
                        );
                        player.TryToWieldWeaponInSlot(
                            EquipmentIndex.ExtraWeaponSlot,
                            Agent.WeaponWieldActionType.Instant,
                            false
                        );
                    }
                    else
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Not a spell!"));
                    }
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage("Cancelled"));
                }
            }

            RefreshValues();
        }

        private void OnMainAgentChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (Agent.Main != null)
            {
                Agent.Main.OnMainAgentWieldedItemChange += OnMainAgentWeaponChange;
            }
        }

        private void OnMainAgentWeaponChange()
        {
            RefreshValues();
        }

        private void OnItemHovered(EquipmentActionItemVM item)
        {
            hoveredItem = item;
            OnPropertyChangedWithValue(HoveredItemText, nameof(HoveredItemText));
        }
    }
}
