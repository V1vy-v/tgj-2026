using System.Collections.Generic;
using Framework.Services.Localization;
using Framework.Services.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Framework.Bootstrap
{
    /// <summary>启动配置：框架需要的资源引用集中在这里，GameRoot 注入给管理器。</summary>
    [CreateAssetMenu(menuName = "Framework/Bootstrap Config", fileName = "BootstrapConfig")]
    public class BootstrapConfig : ScriptableObject
    {
        [SerializeField][Header("输入系统配置")]
        private InputActionAsset inputAsset;
        public InputActionAsset InputAsset => inputAsset;

        [SerializeField][Header("音频系统配置")]
        private AudioMixer mainAudioMixer;
        public AudioMixer MainAudioMixer => mainAudioMixer;

        [SerializeField] [Header("文本配置")]
        private List<LocalizationTable> localizationTables;
        public List<LocalizationTable> LocalizationTables => localizationTables;
        
        [SerializeField][Header("UI根节点预制体")]
        private GameObject uiRoot;
        public GameObject UIRoot => uiRoot;

        [SerializeField][Header("UI注册表")]
        private UIRegistry uiRegistry;
        public UIRegistry UIRegistry => uiRegistry;
    }
}
