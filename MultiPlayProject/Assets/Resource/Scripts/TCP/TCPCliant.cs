using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        //�T�[�o�[�ɑ��M����f�[�^����͂��Ă��炤
        Console.WriteLine("���������͂��AEnter�L�[�������Ă��������B");
        var sendMsg = Console.ReadLine();
        //�������͂���Ȃ��������͏I��
        if (sendMsg == null || sendMsg.Length == 0) return;

        //�T�[�o�[��IP�A�h���X�i�܂��́A�z�X�g���j�ƃ|�[�g�ԍ�
        var ipOrHost = "192.168.2.100";
        //string ipOrHost = "localhost";
        var port = 2001;

        //TcpClient���쐬���A�T�[�o�[�Ɛڑ�����
        var tcp = new TcpClient(ipOrHost, port);
        Console.WriteLine("�T�[�o�[({0}:{1})�Ɛڑ����܂���({2}:{3})�B",
            ((IPEndPoint)tcp.Client.RemoteEndPoint).Address,
            ((IPEndPoint)tcp.Client.RemoteEndPoint).Port,
            ((IPEndPoint)tcp.Client.LocalEndPoint).Address,
            ((IPEndPoint)tcp.Client.LocalEndPoint).Port);

        //NetworkStream���擾����
        var ns = tcp.GetStream();

        //�ǂݎ��A�������݂̃^�C���A�E�g��10�b�ɂ���
        //�f�t�H���g��Infinite�ŁA�^�C���A�E�g���Ȃ�
        //(.NET Framework 2.0�ȏオ�K�v)
        ns.ReadTimeout = 10000;
        ns.WriteTimeout = 10000;

        //�T�[�o�[�Ƀf�[�^�𑗐M����
        //�������Byte�^�z��ɕϊ�
        var sendBytes = Encoding.UTF8.GetBytes(sendMsg + '\n');
        //�f�[�^�𑗐M����
        ns.Write(sendBytes, 0, sendBytes.Length);
        Console.WriteLine(sendMsg);

        //����
        ns.Close();
        tcp.Close();
        Console.WriteLine("�ؒf���܂����B");
        Console.ReadLine();
    }
}