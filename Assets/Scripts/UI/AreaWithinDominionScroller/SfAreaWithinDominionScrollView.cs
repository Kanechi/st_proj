using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Events;
using UniRx;

namespace sfproj
{
    /// <summary>
    /// �̈���n��X�N���[���r���[
    /// </summary>
    public class SfAreaWithinDominionScrollView : WindowBase, IEnhancedScrollerDelegate
    {
        // �Z���T�C�Y(�P�ʃx�N�g��)
        static private float CELL_SIZE = 0.0f;
        // �Z���T�C�Y(������)
        static public Vector2 CellSize = Vector2.zero;
        // �}�X�N�Z���T�C�Y(������)
        static public Vector2 MaskCellSize = Vector2.zero;
        // �I���摜�T�C�Y(������)
        static public Vector2 SelectedImageSize = Vector2.zero;

        [SerializeField]
        private EnhancedScroller m_scroller = null;

        [SerializeField]
        private EnhancedScrollerCellView m_cellView = null;
        //private EnhancedScrollerCellView m_cell = null;

        // �f�[�^���X�g
        private List<SfAreaWithinDominionScrollCellData> m_dataList = new List<SfAreaWithinDominionScrollCellData>();

        // ���ݑI�𒆂̃Z���f�[�^
        private SfAreaWithinDominionScrollCellData m_selectedCellData = null;

        protected override void Start()
        {
            base.Start();

            closeBtn_.OnClickAsObservable().Subscribe(_ => { Close(); });

            var canvasTransform = SfGameManager.Instance.CurrentCanvas.GetComponent<RectTransform>();

            // ��ʍ������擾
            float height = canvasTransform.sizeDelta.y; //Screen.height;

            // ��ʍ����̂S���̂P�̃T�C�Y���擾
            float heightOneFour = height * 0.25f;

            // �X�N���[���r���[�̍��������̃T�C�Y�ɐݒ�
            var rectTransform = gameObject.GetComponent<RectTransform>();
            var size = rectTransform.sizeDelta;
            size.y = heightOneFour;
            rectTransform.sizeDelta = size;

            // ��ʍ����̂R���̂Q�̈ʒu���擾
            //float posTwoThird = height * 0.6666f;

            // ���̈ʒu�ɃX�N���[���r���[�̍�����ݒ�
            var pos = rectTransform.localPosition;
            pos.y = -(heightOneFour + (heightOneFour * 0.5f));
            rectTransform.localPosition = pos;

            // �Z���T�C�Y���v�Z�B�����̂X�O��
            CELL_SIZE = heightOneFour * 0.9f;

            CellSize = new Vector2(heightOneFour * 0.9f, heightOneFour * 0.9f);

            MaskCellSize = new Vector2(heightOneFour * 0.65f, heightOneFour * 0.65f);

            SelectedImageSize = new Vector2(heightOneFour * 0.2f, heightOneFour * 0.2f);
        }

        /// <summary>
        /// �̈���^�b�`�����ۃA�N�e�B�u������Ă̓����t���[���ł� ReloadData() �����f����Ȃ��̂�
        /// ���t���[���ȍ~�� ReloadData ���Ȃ���΂Ȃ�Ȃ��B���̂��߂̃C�x���g�ŁA
        /// �̈�X�N���[���r���[�J�����C�x���g�ɏ�������������������� null ��������
        /// </summary>
        private UnityAction m_openedReloadDataEvent = null;

        private void Update()
        {
            m_openedReloadDataEvent?.Invoke();
        }



        /// <summary>
        /// �̈� ID ����f�[�^���X�g���쐬
        /// </summary>
        /// <param name="dominionId"></param>
        private void CreateData(SfDominion dominionRecord) {

            m_dataList.Clear();

            // �̈�ɂ��� �n�� ID ���擾
            List<uint> areaIdList = dominionRecord.AreaIdList;

            // �n�� ID ���X�g����n�惌�R�[�h���擾���Z���f�[�^���쐬���Ă���
            foreach(uint areaId in areaIdList) {

                var areaRecord = SfAreaTableManager.Instance.Table.Get(areaId);
                if (areaRecord == null)
                    continue;

                m_dataList.Add(new SfAreaWithinDominionScrollCellData(areaRecord));
            }
        }


        // �J������
        public bool Open(SfDominion dominionRecord, UnityAction opened = null)
        {
            gameObject.SetActive(true);

            if (tweenCtrl_ != null)
            {
                tweenCtrl_.Play("Open", () => {
                    opened?.Invoke();
                });
            }
            else
            {
                opened?.Invoke();
            }

            CreateData(dominionRecord);

            m_openedReloadDataEvent = OpenedReloadData;

            return true;
        }

        private void OpenedReloadData() {
            m_scroller.Delegate = this;

            m_scroller.ReloadData();

            m_openedReloadDataEvent = null;
        }

        // ����ۂ̏���
        public override void Close(UnityAction closed = null)
        {
            if (tweenCtrl_ != null)
            {
                tweenCtrl_.Play("Close", () => {
                    closed?.Invoke();
                    gameObject.SetActive(false);
                });
            }
            else
            {
                closed?.Invoke();
                gameObject.SetActive(false);
            }
        }

        // �X�N���[��������
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
#if true
            var cell = scroller.GetCellView(m_cellView) as SfAreaWithinDominionScrollCell;

            // �Z���̖��O��n��̖��O�ɂ������̂� SetData ������ name �ɑ΂��Ēn�於��ݒ肷��
            //cell.name = m_dataList[dataIndex].Identifier + "_Cell";
            cell.RectTransform.sizeDelta = CellSize;

            cell.SetData(dataIndex, m_dataList[dataIndex]);

            cell.Selected = OnSelected;

            return cell;
#else
            return null;
#endif
        }

        // �Z���T�C�Y
        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return CELL_SIZE;
        }

        // �Z���̐�
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return m_dataList.Count;
        }

        /// <summary>
        /// �Z���I�����̏���
        /// </summary>
        /// <param name="cell"></param>
        public void OnSelected(SfAreaWithinDominionScrollCell cell)
        {
            // �I�����Ă�����̂��^�b�`�����ꍇ�̓t�H�[�J�X����
            bool isFocus = true;

#if false
            if (ReferenceEquals(m_selectedCellData, cell.Data))
                isFocus = false;
#endif
            foreach (var data in m_dataList)
                data.IsSelected = false;

            if (isFocus == true)
            {
                m_selectedCellData = cell.Data;

                cell.Data.IsSelected = true;

                // �n��E�B���h�E�J��
            }
            else
            {
                m_selectedCellData = null;

                cell.Data.IsSelected = false;

                // �n��E�B���h�E���J�����Ă��������
            }

            foreach (var data in m_dataList)
                cell.SetSelected(data.IsSelected);

            m_scroller.ReloadData(m_scroller.ScrollRect.horizontalNormalizedPosition);
        }
    }

#if false
    
    /// <summary>
    /// ���̃N���X�͒������K�v
    /// �ʂ̃Z�����^�b�v�����ۂɊJ������ԂŃZ�����������ւ���Ƃ����������Ȃ��Ƃ����Ȃ�
    /// ���󂾂ƊJ���Ă��锻��ɂȂ邽�߃Z���̍Đݒ肪�ł��Ȃ�
    /// 
    /// �f�o�b�O�݂����ɏ��߂����ʏ�ɔz�u���Ă����ĕ\����\���őΉ���������ǂ���������Ȃ�
    /// </summary>
    public class PopupDominionScrollViewController : Singleton<PopupDominionScrollViewController>
    {
        private DominionScrollView Window { get; set; } = null;

        public void Open(SfDominionRecord dominionRecord)
        {
            if (Window != null && Window.IsOpen == true)
                return;

            var prefab = AssetManager.Instance.Get<GameObject>("DominionScrollView");

            var obj = GameObject.Instantiate(prefab, CanvasManager.Instance.Current.transform);

            Window = obj.GetComponent<DominionScrollView>();

            Window.CloseBtn.Btn.OnClickAsObservable().Subscribe(_ => Close());

            Window.Open(dominionRecord);
        }

        public void Close()
        {
            if (Window == null)
                return;

            if (Window.IsOpen == false)
                return;

            GameObject.Destroy(Window.gameObject);

            Window = null;
        }
    }
#endif
}
