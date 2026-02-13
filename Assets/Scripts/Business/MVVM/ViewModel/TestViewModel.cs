using Business.MVVM.Model;
using Business.MVVM.View;
using Scripts.Framework.UI;

namespace Scripts.TetrisGame {
    public class TestViewModel : BaseViewModel {
        private TestView testView;
        private TestModel testModel;
        protected override void OnBind() {
            testView = (TestView)View;
            testModel = (TestModel)Model;
            RegisterBinding(nameof(testModel.HP), _ => testView.UpdateHp(testModel.HP));

            testView.BtnHp.onClick.AddListener(() => {
                testModel.HP -= 10;
            });
            testView.UpdateHp(testModel.HP);

        }
    }
}