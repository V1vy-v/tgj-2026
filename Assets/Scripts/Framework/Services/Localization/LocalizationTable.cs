using System.Collections.Generic;
using UnityEngine;

namespace Framework.Services.Localization
{
    [CreateAssetMenu(fileName = "LocalizationTable", menuName = "Framework/LocalizationTable")]
    public class LocalizationTable : ScriptableObject
    {
        [Header("Localization Table")] 
        [SerializeField] private List<LocalizationGroup> localizationGroups;
        
        public List<LocalizationGroup> LocalizationGroups => localizationGroups;
    }
}
