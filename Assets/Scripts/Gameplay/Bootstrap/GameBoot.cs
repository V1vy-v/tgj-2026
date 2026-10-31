using Framework.Bootstrap;
using Framework.Services.UI;
using Gameplay.UI;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    /// <summary>
    /// 启动场景入口：打开开始界面。
    /// 只放在启动场景，不放进游戏场景。
    /// </summary>
    public class GameBoot : MonoBehaviour
    {
        private void Start()
        {
            var ui = GameRoot.Instance.Services.Get<IUIService>();
            ui.OpenScreen<BeginScreen>();
        }
    }
}
