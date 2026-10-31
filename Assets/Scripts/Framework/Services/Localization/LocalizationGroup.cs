using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.Services.Localization
{
    [System.Serializable]
    public struct LocalizationGroup
    {
        [SerializeField] private string key;
        [SerializeField] private string textEn;
        [SerializeField] private string textCh;
        
        public string Key => key;
        public string TextEn => textEn;
        public string TextCh => textCh;
    }
}