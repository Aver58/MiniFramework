using Scripts.Framework.UI;
using UnityEngine.UI;

namespace Business.MVVM.View {
    public class TestView : BaseView{
        public Text TxtHp;
        public Button BtnHp;
        public void UpdateHp(int hp) {
            TxtHp.text = "Hp:" + hp.ToString();
        }
    }
}