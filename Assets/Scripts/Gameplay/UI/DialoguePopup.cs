using System.Collections;
using Framework.Bootstrap;
using Framework.Services.Localization;
using Framework.Services.UI;
using Gameplay.Player;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public class DialoguePopup : UIBase
    {
        [SerializeField] private TMP_Text speakerText;
        [SerializeField] private TMP_Text contentText;
        [SerializeField, Min(1f)] private float charsPerSecond = 30f;

        private PlayerController _player;
        private PlayerDialogue _dialogue;
        private ILocalizationService _localization;

        private Coroutine _typeRoutine;
        private bool _isTyping;
        private int _visibleCharacters;
        private int _totalCharacters;

        protected override void OnSetup(object args)
        {
            Unbind();

            _player = args as PlayerController;
            if (_player == null || _player.Dialogue == null)
            {
                Debug.LogError("[DialoguePopup] 参数不是 PlayerController");
                return;
            }

            _dialogue = _player.Dialogue;
            _localization = GameRoot.Instance.Services.Get<ILocalizationService>();

            _player.DialogueAdvanceRequested += OnAdvanceRequested;
            _dialogue.LineChanged += ShowLine;
            _localization.OnLanguageChanged += OnLanguageChanged;
        }

        protected override void OnShow()
        {
            if (_dialogue == null)
                return;

            ShowLine(_dialogue.CurrentLine);
        }

        protected override void OnHide()
        {
            Unbind();
        }

        protected override void OnDestroyed()
        {
            Unbind();
        }

        private void Unbind()
        {
            if (_typeRoutine != null)
            {
                StopCoroutine(_typeRoutine);
                _typeRoutine = null;
            }

            _isTyping = false;

            if (_player != null)
                _player.DialogueAdvanceRequested -= OnAdvanceRequested;

            if (_dialogue != null)
                _dialogue.LineChanged -= ShowLine;

            if (_localization != null)
                _localization.OnLanguageChanged -= OnLanguageChanged;

            _player = null;
            _dialogue = null;
            _localization = null;
        }

        private void ShowLine(PlayerDialogue.Line line)
        {
            if (contentText == null || _localization == null)
                return;

            if (speakerText != null)
                speakerText.text = _localization.GetText(line.SpeakerKey);

            contentText.text = _localization.GetText(line.TextKey);
            contentText.ForceMeshUpdate();

            _totalCharacters = contentText.textInfo.characterCount;

            if (line.Stream && charsPerSecond > 0f && _totalCharacters > 0)
                StartTyping();
            else
                ShowAll();
        }

        private void StartTyping()
        {
            if (_typeRoutine != null)
                StopCoroutine(_typeRoutine);

            _visibleCharacters = 0;
            _isTyping = true;
            contentText.maxVisibleCharacters = 0;
            _typeRoutine = StartCoroutine(TypeRoutine());
        }

        private IEnumerator TypeRoutine()
        {
            while (_visibleCharacters < _totalCharacters)
            {
                _visibleCharacters += Mathf.Max(
                    1,
                    Mathf.RoundToInt(charsPerSecond * Time.unscaledDeltaTime));

                contentText.maxVisibleCharacters =
                    Mathf.Min(_visibleCharacters, _totalCharacters);

                yield return null;
            }

            ShowAll();
        }

        private void ShowAll()
        {
            if (_typeRoutine != null)
            {
                StopCoroutine(_typeRoutine);
                _typeRoutine = null;
            }

            contentText.maxVisibleCharacters = _totalCharacters;
            _isTyping = false;
        }

        private void OnAdvanceRequested()
        {
            if (_dialogue == null)
                return;

            if (_isTyping)
            {
                ShowAll();
                return;
            }

            _dialogue.Advance();
        }

        private void OnLanguageChanged()
        {
            if (_dialogue != null)
                ShowLine(_dialogue.CurrentLine);
        }
    }
}