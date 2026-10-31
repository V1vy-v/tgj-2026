using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{
    public sealed class PlayerDialogue
    {
        public readonly struct Line
        {
            public string SpeakerKey { get; }
            public string TextKey { get; }
            public bool Stream { get; }

            public Line(string speakerKey, string textKey, bool stream)
            {
                SpeakerKey = speakerKey;
                TextKey = textKey;
                Stream = stream;
            }
        }

        private readonly List<Line> _lines = new();
        private int _index;

        public bool IsActive { get; private set; }
        public Line CurrentLine =>
            _lines.Count > 0 ? _lines[_index] : default;

        public event Action<Line> LineChanged;
        public event Action Ended;

        public bool Start(TextAsset script)
        {
            if (IsActive)
                return false;

            if (script == null)
            {
                Debug.LogError("[PlayerDialogue] 对话脚本为空");
                return false;
            }

            var parsed = Parse(script);
            if (parsed == null || parsed.Count == 0)
                return false;

            _lines.Clear();
            _lines.AddRange(parsed);
            _index = 0;
            IsActive = true;

            LineChanged?.Invoke(CurrentLine);
            return true;
        }

        public void Advance()
        {
            if (!IsActive)
                return;

            if (_index + 1 >= _lines.Count)
            {
                End();
                return;
            }

            _index++;
            LineChanged?.Invoke(CurrentLine);
        }

        public void End()
        {
            if (!IsActive)
                return;

            IsActive = false;
            Ended?.Invoke();
        }

        private static List<Line> Parse(TextAsset script)
        {
            string[] rawLines = script.text.Split('\n');
            var result = new List<Line>();
            string lastSpeaker = null;

            for (int i = 0; i < rawLines.Length; i++)
            {
                string raw = rawLines[i].Trim().Trim('\r', '\uFEFF');

                if (string.IsNullOrWhiteSpace(raw))
                    continue;

                if (raw.StartsWith("#"))
                    continue;

                string[] columns = raw.Split('|');

                if (columns.Length < 2 || columns.Length > 3)
                {
                    Debug.LogError(
                        $"[PlayerDialogue] {script.name} 第 {i + 1} 行格式错误：{raw}");
                    return null;
                }

                string speaker = columns[0].Trim();
                string textKey = columns[1].Trim();

                if (speaker == "-")
                    speaker = lastSpeaker;

                if (string.IsNullOrEmpty(speaker) || string.IsNullOrEmpty(textKey))
                {
                    Debug.LogError(
                        $"[PlayerDialogue] {script.name} 第 {i + 1} 行 speaker/textKey 为空");
                    return null;
                }

                bool stream = false;
                if (columns.Length == 3)
                {
                    string flag = columns[2].Trim();
                    stream = flag == "1" ||
                             string.Equals(flag, "true", StringComparison.OrdinalIgnoreCase);
                }

                result.Add(new Line(speaker, textKey, stream));
                lastSpeaker = speaker;
            }

            return result;
        }
    }
}