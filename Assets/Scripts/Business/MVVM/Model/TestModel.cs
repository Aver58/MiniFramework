using Scripts.Framework.UI;

namespace Business.MVVM.Model {
    public class TestModel : BaseModel {
        private int hp;
        public int HP {
            get => hp;
            set {
                if (hp != value) {
                    hp = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}