using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Framework.UI {
    public abstract class BaseView : MonoBehaviour {
        // 所属层级
        public UILayer Layer = UILayer.Normal;
        public void Init(UILayer layer) {
            UILayerController.SetLayerOrder(this.transform, layer);
            var graphicRaycaster = transform.GetComponent<GraphicRaycaster>();
            if (graphicRaycaster == null) {
                graphicRaycaster = transform.gameObject.AddComponent<GraphicRaycaster>();
            }

            OnInit();
        }

        public void Open() { OnOpen(); }
        public void Close() { OnClose(); }
        public void Clear() { OnClear(); }
        protected virtual void OnInit(){}
        protected virtual void OnOpen() {}
        protected virtual void OnClose() {}
        protected virtual void OnClear() { }
    }
}