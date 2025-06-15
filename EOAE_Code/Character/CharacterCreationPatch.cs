using HarmonyLib;
using SandBox;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;

namespace EOAE_Code.Character;

[HarmonyPatch]
public class CharacterCreationPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SandBoxGameManager), "LaunchSandboxCharacterCreation")]
    public static bool LaunchCustomCharacterCreation()
    {
        var characterCreationState =
            Game.Current.GameStateManager.CreateState<CharacterCreationState>(
                new CustomCharacterCreationContent()
            );
        Game.Current.GameStateManager.CleanAndPushState(characterCreationState, 0);

        return false;
    }
}
