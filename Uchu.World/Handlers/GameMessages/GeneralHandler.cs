using System.Threading.Tasks;
using Uchu.Core;

namespace Uchu.World.Handlers.GameMessages
{
    public class GeneralHandler : HandlerGroup
    {
        [PacketHandler]
        public async Task RequestUseHandler(RequestUseMessage message, Player player)
        {
            player.SendChatMessage($"Interacted with {message.TargetObject}");
            
            await player.GetComponent<QuestInventory>().UpdateObjectTaskAsync(
                MissionTaskType.Interact,
                message.TargetObject.Lot,
                message.TargetObject
            );

            if (message.IsMultiInteract)
            {
                //
                // Multi-interact is mission
                //

                if (message.MultiInteractType == default)
                    player.GetComponent<QuestInventory>().MessageOfferMission(
                        (int) message.MultiInteractId,
                        message.TargetObject
                    );
            }
            else
            {
                message.TargetObject.Interact(player);
            }
        }

        [PacketHandler]
        public void RequestResurrectHandler(RequestResurrectMessage message, Player player)
        {
            player.GetComponent<DestructibleComponent>().Resurrect();
        }

        [PacketHandler]
        public void RequestSmashHandler(RequestSmashPlayerMessage message, Player player)
        {
            player.GetComponent<DestructibleComponent>().Smash(player, player);
        }

        [PacketHandler]
        public void RebuildCancel(RebuildCancelMessage message, Player player)
        {
            if (message.Associate.TryGetComponent<RebuildComponent>(out var rebuild))
            {
                rebuild.StopRebuild(player, RebuildFailReason.Canceled);
            }
        }

        [PacketHandler]
        public void ReadyForUpdates(ReadyForUpdateMessage message, Player player)
        {
            player.Perspective.ClientLoadedObjectCount++;
        }
    }
}