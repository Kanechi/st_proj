using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Events;
using UniRx;

namespace sfproj
{
    public class SfZoneFacilityScrollView : WindowBase, IEnhancedScrollerDelegate
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

        /// <summary>
        /// �^�b�`�������̃f�[�^
        /// </summary>
        private SfZoneCellData m_touchedZoneCellData = null;

        // �f�[�^���X�g
        private List<SfZoneFacilityScrollCellData> m_dataList = new List<SfZoneFacilityScrollCellData>();

        // ���ݑI�𒆂̃Z���f�[�^
        private SfZoneFacilityScrollCellData m_selectedCellData = null;

        // Start is called before the first frame update
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

        private UnityAction m_openedReloadDataEvent = null;

        private void Update()
        {
            m_openedReloadDataEvent?.Invoke();
        }

        private void CreateData() {

            m_dataList.Clear();

            var list = SfZoneFacilityRecordTable.Instance.RecordList;

            foreach (var record in list)
            {
                m_dataList.Add(new SfZoneFacilityScrollCellData(record, m_touchedZoneCellData));
            }
        }

        public bool Open(SfZoneCellData touchedZoneCellData, UnityAction opened = null)
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

            CreateData();

            m_openedReloadDataEvent = OpenedReloadData;

            return true;
        }


        private void OpenedReloadData()
        {
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
            var cell = scroller.GetCellView(m_cell) as SfZoneFacilityScrollCell;

            // �Z���̖��O��n��̖��O�ɂ������̂� SetData ������ name �ɑ΂��Ēn�於��ݒ肷��
            //cell.name = m_dataList[dataIndex].Identifier + "_Cell";
            cell.RectTransform.sizeDelta = CellSize;

            cell.SetData(m_dataList[dataIndex]);

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
        public void OnSelected(SfZoneFacilityScrollCell cell)
        {
            bool isFocus = true;

#if false
            // �I�����Ă�����̂��^�b�`�����ꍇ�̓t�H�[�J�X����
            if (ReferenceEquals(m_selectedCellData, cell.Data))
                isFocus = false;
#endif
            foreach (var data in m_dataList)
                data.IsSelected = false;

            if (isFocus == true)
            {
                m_selectedCellData = cell.Data;

                cell.Data.IsSelected = true;

                // ���ݎ{�ݑI���E�B���h�E�J��
            }
            else
            {
                m_selectedCellData = null;

                cell.Data.IsSelected = false;

                // ���ݎ{�ݑI���E�B���h�E���J�����Ă��������
            }

            foreach (var data in m_dataList)
                cell.SetSelected(data.IsSelected);

            m_scroller.ReloadData(m_scroller.ScrollRect.horizontalNormalizedPosition);
        }
    }
}