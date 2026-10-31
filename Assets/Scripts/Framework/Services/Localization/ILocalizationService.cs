using System;

namespace Framework.Services.Localization
{
    public enum Language
    {
        English,
        ChineseSimple,
    }
    public interface ILocalizationService
    {
        Language Language { get; }
        string GetText(string key);
        void SetLanguage(Language language);
        event Action OnLanguageChanged;
    }
}