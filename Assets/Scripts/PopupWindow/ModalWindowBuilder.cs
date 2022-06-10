using UnityEngine;

namespace Framework {

    public abstract class BaseModalWindowBuilder {

        protected ModalWindow window_ = null;

        public ModalWindow GetResult() {
            return window_;
        }

        public virtual void Create() {

            //var prefab = ResourceController.Instance.Load<GameObject>("Prefabs/UI/common/common_modal");

            var prefab = AssetManager.Instance.Get<GameObject>("popup_window");

            var obj = GameObject.Instantiate(prefab, CanvasManager.Instance.Current.transform);

            window_ = obj.GetComponent<ModalWindow>();
        }

        public abstract void SettingTitleActive();
        public abstract void SettingMainActive();
        public abstract void SettingBtnActive();

        public abstract void SettingName();
        public abstract void SettingTitle();
        public abstract void SettingMain();
        public abstract void SettingBtn();
    }
    /// <summary>
    /// モーダルウィンドウディレクター
    /// </summary>
    public class ModalWindowDirector {

        private BaseModalWindowBuilder builder_ = null;

        public ModalWindowDirector(BaseModalWindowBuilder builder) {
            builder_ = builder;
        }

        public ModalWindow GetResult() {
            return builder_.GetResult();
        }

        public void Construct() {

            builder_.Create();

            builder_.SettingTitleActive();
            builder_.SettingMainActive();
            builder_.SettingBtnActive();

            builder_.SettingName();
            builder_.SettingTitle();
            builder_.SettingMain();
            builder_.SettingBtn();
        }
    }
}
