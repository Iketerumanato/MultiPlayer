using UnityEngine;
using Mirror;

namespace QuickStart
{
    public class Player : NetworkBehaviour
    {
        Vector3 OriginCamPos = new (0f, 0f, 0f);
        [SerializeField] float PlayerSpeedX = 110f;
        [SerializeField] float PlayerSpeedZ = 4f;

        public override void OnStartLocalPlayer()
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = OriginCamPos;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            float MoveX = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerSpeedX;
            float MoveZ = Input.GetAxis("Vertical") * Time.deltaTime * PlayerSpeedZ;

            transform.Rotate(0f, MoveX, 0f);
            transform.Translate(0f, 0f, MoveZ);
        }
    }
}