using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class TCPServerTest : MonoBehaviour
{
    //�������g���w��IP�A�h���X
    public string mIpAddress = "192.168.2.100";
    //�|�[�g�ԍ��͓K���@�������N���C�A���g�ƍ��킹�邱��
    public int mPortNumber = 2001;

    private TcpListener mListener;
    private TcpClient mClient;

    // �\�P�b�g�ڑ������A�ҋ@
    public void Start()
    {
        var ip = IPAddress.Parse(mIpAddress);
        mListener = new TcpListener(ip, mPortNumber);
        mListener.Start();
        //�R�[���o�b�N�ݒ�@�������̓R�[���o�b�N�֐��ɓn�����
        mListener.BeginAcceptSocket(DoAcceptTcpClientCallback, mListener);
    }

    // �N���C�A���g����̐ڑ�����
    private void DoAcceptTcpClientCallback(IAsyncResult ar)
    {
        //�n���ꂽ���̂����o��
        var listener = (TcpListener)ar.AsyncState;
        mClient = listener.EndAcceptTcpClient(ar);
        print("Connect: " + mClient.Client.RemoteEndPoint);

        // �ڑ������l�Ƃ̃l�b�g���[�N�X�g���[�����擾
        var stream = mClient.GetStream();
        var reader = new StreamReader(stream, Encoding.UTF8);

        // �ڑ����؂��܂ő���M���J��Ԃ�
        while (mClient.Connected)
        {
            while (!reader.EndOfStream)
            {
                // ��s���̕�������󂯎��
                print(reader.ReadLine());
            }

            // �N���C�A���g�̐ڑ����؂ꂽ��
            if (mClient.Client.Poll(1000, SelectMode.SelectRead) && (mClient.Client.Available == 0))
            {
                print("Disconnect: " + mClient.Client.RemoteEndPoint);
                mClient.Close();
                break;
            }
        }
    }

    // �I������
    protected virtual void OnApplicationQuit()
    {
        if (mListener != null) mListener.Stop();
        if (mClient != null) mClient.Close();
    }
}
