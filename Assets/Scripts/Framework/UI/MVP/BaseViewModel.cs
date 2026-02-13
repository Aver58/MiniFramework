using System;
using System.Collections.Generic;

namespace Scripts.Framework.UI {
    // ViewModel 基类（MVVM框架的核心）
    // 属性变更通知由 BaseModel 提供，这里作为 ViewModel 使用
    public abstract class BaseViewModel {
        protected BaseView View;
        protected BaseModel Model;
        private Dictionary<string, Action<BaseModel>> propertyBindingMap = new();

        public virtual void Bind(BaseView view, BaseModel model) {
            View = view;
            Model = model;
            if (Model != null) {
                Model.PropertyChanged += OnModelPropertyChanged;
            }

            OnBind();
        }

        /// <summary>
        /// 注册属性绑定（自动更新 UI）
        /// </summary>
        protected void RegisterBinding(string propertyName, Action<BaseModel> updateAction) {
            propertyBindingMap[propertyName] = updateAction;
        }

        // 当Model属性变更时调用
        protected virtual void OnModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (propertyBindingMap.TryGetValue(e.PropertyName, out var action)) {
                action?.Invoke(Model);
            }

            OnPropertyChanged(e.PropertyName);
        }

        protected virtual void OnPropertyChanged(string propertyName) { }
        protected abstract void OnBind();

        public virtual void Unbind() {
            if (Model != null) {
                Model.PropertyChanged -= OnModelPropertyChanged;
            }
        }

        public void Open() { View?.Open(); }
        public void Close() { View?.Close(); }
        public void Init(UILayer layer) { View?.Init(layer); }
        public void Clear() {
            Unbind();
            View?.Clear();
        }
    }
}