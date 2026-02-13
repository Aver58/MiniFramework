using System;
using System.Collections.Generic;
using Scripts.Framework.Resource;
using UnityEngine;

namespace Scripts.Framework.UI {
    /// <summary>
    /// MVVM UI 框架 - 外部调用接口
    /// 使用方法: UIFramework.Open<PlayerInfoView>();
    ///          UIFramework.Close<PlayerInfoView>();
    /// </summary>
    public class UIFramework : MonoSingleton<UIFramework> {
        private Dictionary<Type, BaseViewModel> openViewModels = new();
        private Canvas uiCanvas; // UI根节点
        public Canvas UICanvas {
            get {
                if (uiCanvas == null) {
                    uiCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
                }

                return uiCanvas;
            }
        }

        /// <summary>
        /// 打开 UI 界面
        /// </summary>
        public static void OpenAsync<T>(Action<T> onOpen = null) where T : BaseViewModel, new() {
            Instance.OpenAsyncInternal<T>(onOpen);
        }

        /// <summary>
        /// 关闭 UI 界面
        /// </summary>
        public static void Close<T>() where T : BaseViewModel {
            Instance.CloseInternal<T>();
        }

        /// <summary>
        /// 获取已打开的 UI 实例
        /// </summary>
        public static T GetViewModel<T>() where T : BaseView {
            var type = typeof(T);
            if (Instance.openViewModels.TryGetValue(type, out var view)) {
                return view as T;
            }
            return null;
        }

        // ===== 内部实现 =====
        private const string VIEW_MODEL = "ViewModel";
        private const string MODEL = "Model";
        private const string NAMESPACE_PREFIX = "Business.MVVM.Model.";
        private void OpenAsyncInternal<TViewModel>(Action<TViewModel> onOpen) where TViewModel : BaseViewModel, new() {
            var type = typeof(TViewModel);

            // 如果已打开，直接返回
            if (openViewModels.TryGetValue(type, out var vm)) {
                vm.Open();
                onOpen?.Invoke(vm as TViewModel);
            }

            // 从资源加载 Prefab
            var prefabName = typeof(TViewModel).Name;
            var key = prefabName.Replace(VIEW_MODEL, "");
            // 实例化
            var config = StaticConfig.UIViewDefine.Get(key);
            if (config == null) {
                Debug.LogError($"{key} not found in UIViewDefineConfig.csv");
                return;
            }

            var assetPath = $"{ResourceConfig.Instance.UIAssetPathPrefix}/{config.AssetName}.prefab";
            ResourceManager.Instance.LoadAssetAsync<GameObject>(assetPath, o => {
                if (o == null) {
                    Debug.LogError($"Failed to load UI prefab: {assetPath}");
                    return;
                }

                var parent = UICanvas.transform;
                var go = Instantiate(o, parent);
                var view = go.GetComponent<BaseView>();
                if (view == null) {
                    Debug.LogError($"{go} prefab not found {prefabName} component!");
                    return;
                }

                // 创建 ViewModel
                var viewModel = new TViewModel();
                // 用名称反射出具体 Model 类型并实例化
                var modelTypeName = prefabName.Replace(VIEW_MODEL, MODEL);
                var modelType = Type.GetType(modelTypeName);
                if (modelType == null) {
                    // 如果没找到，尝试默认的 Model 命名空间
                    var fullModelTypeName = $"{NAMESPACE_PREFIX}{modelTypeName}";
                    modelType = Type.GetType(fullModelTypeName);
                }

                if (modelType != null) {
                    var modelInstance = Activator.CreateInstance(modelType) as BaseModel;
                    viewModel.Bind(view, modelInstance);
                } else {
                    Debug.LogError($"没找到具体数据类。类名 {modelTypeName}，命名空间下也找不到：Business.MVVM.Model.{modelTypeName}");
                }

                var layer = (UILayer)config.UILayer;
                viewModel.Init(layer);
                viewModel.Open();
                openViewModels[type] = viewModel;
                onOpen?.Invoke(viewModel);
            });
        }

        private void CloseInternal<T>() where T : BaseViewModel {
            var type = typeof(T);
            if (openViewModels.TryGetValue(type, out var viewModel)) {
                viewModel.Close();
                openViewModels.Remove(type);
            }
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            openViewModels.Clear();
        }
    }
}
