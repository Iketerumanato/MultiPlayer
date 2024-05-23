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

        #region//サーバー上で同期させる変数
        //hookはSyncVarの値が変更された際に呼び出されるメソッドを指定するため
        [SyncVar(hook = nameof(OnNameChanged))]
        public string PlayerName;
        [SyncVar(hook = nameof(OnColorChanged))]
        public Color PlayerColor = Color.white;
        #endregion

        #region//同じサーバー内での同期
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

        #region//プレイヤーの名前表示
        void OnNameChanged(string _OldText, string _NewText)
        {
            PlayerNameText.text = PlayerName;
        }
        #endregion

        #region//プレイヤーの色変え
        void OnColorChanged(Color _OldColor, Color _NewColor)
        {
            PlayerNameText.color = _NewColor;
            PlayerMaterialClone = new Material(GetComponent<Renderer>().material);
            PlayerMaterialClone.color = _NewColor;
            GetComponent<Renderer>().material = PlayerMaterialClone;
        }
        #endregion

        #region//クライアントのプレイヤー情報をサーバーに渡す
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

        //メソッドの名前はCmdから始めないとビルドエラーになるらしいが今のところ問題なさそう
        #endregion

        private void Update()
        {
            //プレイヤーがクライアントか否か
            if (!isLocalPlayer)
            {
                FloatingInfo.transform.LookAt(Camera.main.transform);
                return;
            }

            #region//プレイヤー移動
            float MoveX = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerSpeedX;
            float MoveZ = Input.GetAxis("Vertical") * Time.deltaTime * PlayerSpeedZ;

            transform.Rotate(0f, MoveX, 0f);
            transform.Translate(0f, 0f, MoveZ);
            #endregion
        }
    }
}