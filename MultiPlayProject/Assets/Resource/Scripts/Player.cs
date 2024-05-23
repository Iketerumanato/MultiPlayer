using UnityEngine;
using TMPro;
using Mirror;

namespace QuickStart
{
    public class Player : NetworkBehaviour
    {
        [SerializeField]
        Vector3 OriginPlayerPos = new(0f, -0.3f, 0.6f);

        [SerializeField]
        Vector3 OriginPlayerScale = new(0.1f, 0.1f, 0.1f);

        Vector3 OriginCamPos = new(0f, 0f, 0f);

        [SerializeField] float PlayerSpeedX = 110f;
        [SerializeField] float PlayerSpeedZ = 4f;

        [SerializeField] TMP_Text PlayerNameText;
        [SerializeField] GameObject FloatingInfo;

        SynchronizationText _synchronizationtext;

        Material PlayerMaterialClone;
        private void Awake()
        {
            _synchronizationtext = GameObject.FindObjectOfType<SynchronizationText>();
        }

        #region//�T�[�o�[��œ���������ϐ�
        //hook��SyncVar�̒l���ύX���ꂽ�ۂɌĂяo����郁�\�b�h���w�肷�邽��
        [SyncVar(hook = nameof(OnNameChanged))]
        public string PlayerName;
        [SyncVar(hook = nameof(OnColorChanged))]
        public Color PlayerColor = Color.white;
        #endregion

        #region//�����T�[�o�[���ł̓���
        public override void OnStartLocalPlayer()
        {
            _synchronizationtext._player = this;

            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = OriginCamPos;

            FloatingInfo.transform.localPosition = OriginPlayerPos;
            FloatingInfo.transform.localScale = OriginPlayerScale;

            string Name = "Player" + Random.Range(100, 999);
            Color Color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            SetUpPlayerCmd(Name, Color);
        }
        #endregion

        #region//�v���C���[�̖��O�\��
        void OnNameChanged(string _OldText, string _NewText)
        {
            PlayerNameText.text = PlayerName;
        }
        #endregion

        #region//�v���C���[�̐F�ς�
        void OnColorChanged(Color _OldColor, Color _NewColor)
        {
            PlayerNameText.color = _NewColor;
            PlayerMaterialClone = new Material(GetComponent<Renderer>().material);
            PlayerMaterialClone.color = _NewColor;
            GetComponent<Renderer>().material = PlayerMaterialClone;
        }
        #endregion

        #region//�N���C�A���g�̃v���C���[�����T�[�o�[�ɓn��
        [Command]
        public void SetUpPlayerCmd(string _playername, Color _playercolor)
        {
            PlayerName = _playername;
            PlayerColor = _playercolor;
            _synchronizationtext.statusText = $"{PlayerNameText} is Participated!";
        }

        [Command]
        public void SendPlayerMessageCmd()
        {
            if (_synchronizationtext)
            {
                _synchronizationtext.statusText = $"{PlayerNameText} is {Random.Range(10, 99)} times say Hello!";
            }
        }

        //���\�b�h�̖��O��Cmd����n�߂Ȃ��ƃr���h�G���[�ɂȂ�炵�������̂Ƃ�����Ȃ�����
        #endregion

        private void Update()
        {
            //�v���C���[���N���C�A���g���ۂ�
            if (!isLocalPlayer)
            {
                FloatingInfo.transform.LookAt(Camera.main.transform);
                return;
            }

            #region//�v���C���[�ړ�
            float MoveX = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerSpeedX;
            float MoveZ = Input.GetAxis("Vertical") * Time.deltaTime * PlayerSpeedZ;

            transform.Rotate(0f, MoveX, 0f);
            transform.Translate(0f, 0f, MoveZ);
            #endregion
        }
    }
}