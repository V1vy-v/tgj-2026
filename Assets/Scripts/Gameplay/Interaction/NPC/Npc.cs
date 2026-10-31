using System;
using System.Collections.Generic;
using Framework.Bootstrap;
using Framework.Services.Localization;
using Gameplay.Interaction.Interface;
using Gameplay.Player.Interaction;
using UnityEngine;

namespace Gameplay.Interaction.NPC
{
    public class Npc : InteractableBehaviour
    {
        [SerializeField] private TextAsset dialogueScript;

        public override IReadOnlyList<InteractionOption> GetOptions(PlayerContext ctx)
        {
            if (dialogueScript == null || ctx.StartDialogue == null)
                return Array.Empty<InteractionOption>();

            string prompt = "Talk";

            if (GameRoot.Instance.Services.TryGet<ILocalizationService>(
                    out var localization))
            {
                prompt = localization.GetText("interaction.talk");
            }

            return SingleOption(
                prompt,
                _ => ctx.StartDialogue(dialogueScript));
        }
    }
}