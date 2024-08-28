using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class TCPServerTest : MonoBehaviour
{
    //自分自身を指すIPアドレス
    public string mIpAddress = "192.168.2.100";
    //ポート番号は適当　ただしクライアントと合わせること
    public int mPortNumber = 2001;

    private TcpListener mListener;
    private TcpClient mClient;

    // ソケット接続準備、待機
    public void Start()
    {
        var ip = IPAddress.Parse(mIpAddress);
        mListener = new TcpListener(ip, mPortNumber);
        mListener.Start();
        //コールバック設定　第二引数はコールバック関数に渡される
        mListener.BeginAcceptSocket(DoAcceptTcpClientCallback, mListener);
    }

    // クライアントからの接続処理
    private void DoAcceptTcpClientCallback(IAsyncResult ar)
    {
        //渡されたものを取り出す
        var listener = (TcpListener)ar.AsyncState;
        mClient = listener.EndAcceptTcpClient(ar);
        print("Connect: " + mClient.Client.RemoteEndPoint);

        // 接続した人とのネットワークストリームを取得
        var stream = mClient.GetStream();
        var reader = new StreamReader(stream, Encoding.UTF8);

        // 接続が切れるまで送受信を繰り返す
        while (mClient.Connected)
        {
            while (!reader.EndOfStream)
            {
                // 一行分の文字列を受け取る
                print(reader.ReadLine());
            }

            // クライアントの接続が切れたら
            if (mClient.Client.Poll(1000, SelectMode.SelectRead) && (mClient.Client.Available == 0))
            {
                print("Disconnect: " + mClient.Client.RemoteEndPoint);
                mClient.Close();
                break;
            }
        }
    }

    // 終了処理
    protected virtual void OnApplicationQuit()
    {
        if (mListener != null) mListener.Stop();
        if (mClient != null) mClient.Close();
    }
}
