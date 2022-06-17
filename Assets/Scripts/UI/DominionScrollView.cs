using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Events;
using UniRx;

namespace sfproj
{
    public class DominionScrollView : WindowBase, IEnhancedScrollerDelegate
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
        private EnhancedScrollerCellView m_cell = null;

        // �f�[�^���X�g
        private List<DominionScrollCellData> m_dataList = new List<DominionScrollCellData>();

        // ���ݑI�𒆂̃Z���f�[�^
        private DominionScrollCellData m_selectedCellData = null;

        protected override void Start()
        {
            base.Start();
            
            // ��ʍ������擾
            float height = Screen.height;

            // ��ʍ����̂R���̂P�̃T�C�Y���擾
            float heightOneThird = height * 0.3333f;

            // �X�N���[���r���[�̍��������̃T�C�Y�ɐݒ�
            var rectTransform = gameObject.GetComponent<RectTransform>();
            var size = rectTransform.sizeDelta;
            size.y = heightOneThird;
            rectTransform.sizeDelta = size;

            // ��ʍ����̂R���̂Q�̈ʒu���擾
            //float posTwoThird = height * 0.6666f;

            // ���̈ʒu�ɃX�N���[���r���[�̍�����ݒ�
            var pos = rectTransform.localPosition;
            pos.y = -heightOneThird;
            rectTransform.localPosition = pos;

            // �Z���T�C�Y���v�Z�B�����̂X�O��
            CELL_SIZE = heightOneThird * 0.9f;

            CellSize = new Vector2(heightOneThird * 0.9f, heightOneThird * 0.9f);

            MaskCellSize = new Vector2(heightOneThird * 0.65f, heightOneThird * 0.65f);

            SelectedImageSize = new Vector2(heightOneThird * 0.2f, heightOneThird * 0.2f);
        }

        /// <summary>
        /// �̈� ID ����f�[�^���X�g���쐬
        /// </summary>
        /// <param name="dominionId"></param>
        private void CreateData(SfDominionRecord dominionRecord) {

            m_dataList.Clear();

            // �̈�ɂ��� �n�� ID ���擾
            List<uint> areaIdList = dominionRecord.AreaIdList;

            // �n�� ID ���X�g����n�惌�R�[�h���擾���Z���f�[�^���쐬���Ă���
            foreach(uint areaId in areaIdList) {

                var areaRecord = SfAreaRecordTableManager.Instance.Get(areaId);
                if (areaRecord == null)
                    continue;

                m_dataList.Add(new DominionScrollCellData(areaRecord));
            }
        }


        // �J������
        public bool Open(SfDominionRecord dominionRecord, UnityAction opened = null)
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

            m_scroller.Delegate = this;

            m_scroller.ReloadData();

            return true;
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
            var cell = scroller.GetCellView(m_cell) as DominionScrollCell;

            // �Z���̖��O��n��̖��O�ɂ������̂� SetData ������ name �ɑ΂��Ēn�於��ݒ肷��
            //cell.name = m_dataList[dataIndex].Identifier + "_Cell";
            cell.RectTransform.sizeDelta = CellSize;

            cell.SetData(dataIndex, m_dataList[dataIndex]);

            cell.Selected = OnSelected;

            return cell;
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
        public void OnSelected(DominionScrollCell cell)
        {
            // �I�����Ă�����̂��^�b�`�����ꍇ�̓t�H�[�J�X����
            bool isFocus = true;

            if (ReferenceEquals(m_selectedCellData, cell.Data))
                isFocus = false;

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
