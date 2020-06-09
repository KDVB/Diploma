using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.UI;

public class CallReciever : MonoBehaviour
{

    string caller = string.Empty;
    public GameObject sound;
    public GameObject callMenu;
    public TextMeshProUGUI text;

    private int port = 11000; // порт сервера
    private string address = BrowserLoad.address;
    Thread listener;

    // Start is called before the first frame update
    void Start()
    {
        listener = new Thread(CreateNewListen);
        listener.Start();
    }

    private void OnDisable()
    {
        listener.Abort();
    }

    public void CreateNewListen()
    {
        int port = 12000;
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(GetLocalIPAddress()), port);

        // создаем сокет
        Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            // связываем сокет с локальной точкой, по которой будем принимать данные
            listenSocket.Bind(ipPoint);

            // начинаем прослушивание
            listenSocket.Listen(10);
            while (true)
            {
                Socket handler = listenSocket.Accept();
                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байтов
                byte[] data = new byte[256]; // буфер для получаемых данных

                do
                {
                    bytes = handler.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (handler.Available > 0);

                Debug.Log(builder.ToString());

                string message = "";
                if (builder.ToString().Contains("call"))
                {
                    HelloUnity3D.caller = builder.ToString().Split(',')[1].Trim();
                    HelloUnity3D.phone = builder.ToString().Split(',')[2].Trim();
                    HelloUnity3D.isCall = true;
                    message = "ready";
                }


                // отправляем ответ

                data = Encoding.Unicode.GetBytes(message);
                handler.Send(data);
                // закрываем сокет
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    public string CheckChannel(string abonent)
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "show, " + abonent;
            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);

            // получаем ответ
            data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            do
            {
                bytes = socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (socket.Available > 0);


            // закрываем сокет
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            return builder.ToString();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return string.Empty;
        }

    }
}
