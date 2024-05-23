using UnityEngine;
using TMPro;
using Mirror;

namespace QuickStart
{
    public class SynchronizationText : NetworkBehaviour
    {
        [SerializeField] TMP_Text CanvasStatusText;
        public Player _player;

        [SyncVar(hook = nameof(OnStatusTextChanged))]
        public string statusText;

        void OnStatusTextChanged(string _Oldtext,string _Newtext)
        {
            CanvasStatusText.text = statusText;
        }

        public void SendMessage()
        {
            _player?.SendPlayerMessageCmd();
        }
    }
}