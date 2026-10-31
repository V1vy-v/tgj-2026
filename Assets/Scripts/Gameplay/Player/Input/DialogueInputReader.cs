using System;
using UnityEngine.InputSystem;

namespace Gameplay.Player.Input
{
    public sealed class DialogueInputReader : IDisposable
    {
        private readonly InputAction _advance;

        public event Action AdvancePressed;

        public DialogueInputReader(InputActionMap map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            _advance = map.FindAction("Advance", true);
            _advance.performed += OnAdvance;
        }

        public void Dispose()
        {
            _advance.performed -= OnAdvance;
        }

        private void OnAdvance(InputAction.CallbackContext _)
        {
            AdvancePressed?.Invoke();
        }
    }
}