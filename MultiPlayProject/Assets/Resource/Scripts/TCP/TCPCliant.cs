using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        //サーバーに送信するデータを入力してもらう
        Console.WriteLine("文字列を入力し、Enterキーを押してください。");
        var sendMsg = Console.ReadLine();
        //何も入力されなかった時は終了
        if (sendMsg == null || sendMsg.Length == 0) return;

        //サーバーのIPアドレス（または、ホスト名）とポート番号
        var ipOrHost = "192.168.2.100";
        //string ipOrHost = "localhost";
        var port = 2001;

        //TcpClientを作成し、サーバーと接続する
        var tcp = new TcpClient(ipOrHost, port);
        Console.WriteLine("サーバー({0}:{1})と接続しました({2}:{3})。",
            ((IPEndPoint)tcp.Client.RemoteEndPoint).Address,
            ((IPEndPoint)tcp.Client.RemoteEndPoint).Port,
            ((IPEndPoint)tcp.Client.LocalEndPoint).Address,
            ((IPEndPoint)tcp.Client.LocalEndPoint).Port);

        //NetworkStreamを取得する
        var ns = tcp.GetStream();

        //読み取り、書き込みのタイムアウトを10秒にする
        //デフォルトはInfiniteで、タイムアウトしない
        //(.NET Framework 2.0以上が必要)
        ns.ReadTimeout = 10000;
        ns.WriteTimeout = 10000;

        //サーバーにデータを送信する
        //文字列をByte型配列に変換
        var sendBytes = Encoding.UTF8.GetBytes(sendMsg + '\n');
        //データを送信する
        ns.Write(sendBytes, 0, sendBytes.Length);
        Console.WriteLine(sendMsg);

        //閉じる
        ns.Close();
        tcp.Close();
        Console.WriteLine("切断しました。");
        Console.ReadLine();
    }
}