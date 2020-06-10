using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

// Socket Listener acts as a server and listens to the incoming   
// messages on the specified port and protocol.  
namespace ServerForVoice
{
    public class Program
    {
        
        static int port = 11000; // порт для приема входящих запросов
        static void Main(string[] args)
        {
            Console.WriteLine("Введіть адресу");

            string serverip = Console.ReadLine();

            List<Station> stations = new List<Station>();

            List<Telephone> telephones = new List<Telephone>();

            stations.Add(new Station("0000"));
            stations.Add(new Station("1111"));

            stations[0].AddTelephone("01");
            stations[0].AddTelephone("02");
            stations[0].AddTelephone("03");

            stations[1].AddTelephone("11");
            stations[1].AddTelephone("12");
            stations[1].AddTelephone("13");

            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(serverip), port);

            Console.WriteLine(ipPoint.Address);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

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

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
                    string message = "";
                    if(builder.ToString().Contains("station"))
                    {
                        string numb = builder.ToString().Split(',')[1].Trim();
                        stations.Add(new Station(numb));
                    }
                    if (builder.ToString().Contains("status"))
                    {
                        string numb = builder.ToString().Split(',')[1].Trim();
                        message = telephones.Where(x => x.Name.Equals(numb)).Select(x => x.Status).First();

                    }
                    else if (builder.ToString().Contains("unUse"))
                    {
                        string numbTelephone = builder.ToString().Split(',')[2].Trim();
                        string numbStation = builder.ToString().Split(',')[1].Trim();
                        UnUsing(stations, numbStation, numbTelephone);
                    }
                    else if (builder.ToString().Contains("use"))
                    {
                        string numb = builder.ToString().Split(',')[2].Trim();
                        string numbStation = builder.ToString().Split(',')[1].Trim();
                        Using(stations, numbStation, numb, builder.ToString().Split(',')[3].Trim());
                    }
                    else if (builder.ToString().Contains("unCall"))
                    {
                        UnCall(stations, builder.ToString().Split(',')[1].Trim(), builder.ToString().Split(',')[2].Trim());
                    }
                    else if (builder.ToString().Contains("call"))
                    {                      
                        Call(stations, builder.ToString().Split(',')[1].Trim(), builder.ToString().Split(',')[2].Trim());
                    }
                    else if (builder.ToString().Contains("unCh"))
                    {
                        ChannelUnUse(stations, builder.ToString().Split(',')[1].Trim(), builder.ToString().Split(',')[2].Trim());
                    }
                    else if (builder.ToString().Contains("ch"))
                    {
                        ChannelUse(stations, builder.ToString().Split(',')[1].Trim(), builder.ToString().Split(',')[2].Trim(), builder.ToString().Split(',')[3].Trim());
                    }
                    else if (builder.ToString().Contains("showChannel"))
                    {
                        message = ShowChannel(stations, builder.ToString().Split(',')[1].Trim(), builder.ToString().Split(',')[2].Trim());
                    }
                    else if (builder.ToString().Contains("showAll"))
                    {
                        message = channels(stations);
                    }
                    else if (builder.ToString().Contains("add"))
                    {
                        AddTelephone(stations, builder.ToString().Split(',')[1].Trim(), builder.ToString().Split(',')[2].Trim());
                    }
                    else if (builder.ToString().Contains("inNetwork"))
                    {
                        message = InNetwork(stations);
                    }
                    else if (builder.ToString().Contains("getIp"))
                    {
                        message = GetIPAddress(stations, builder.ToString().Split(',')[1].Trim(), builder.ToString().Split(',')[2].Trim());
                    }
                    else if (builder.ToString().Contains("telephonesNumbers"))
                    {
                        message = TelephonesNumbers(stations, builder.ToString().Split(',')[1].Trim());
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

        static string GetIPAddress(List<Station> stations, string station, string numb)
        {
            string msg = string.Empty;
            for (int j = 0; j < stations.Count; j++)
            {
                if (stations[j].Numb.Equals(station))
                {
                    for (int i = 0; i < stations[j].telephones.Count; i++)
                    {
                        if (stations[j].telephones[i].Name.Equals(numb))
                        {
                            msg = stations[j].telephones[i].IPAddress;
                            break;
                        }
                    }
                }
            }
            return msg;
        }

        static void AddTelephone(List<Station> stations, string station, string numb)
        {
            for (int j = 0; j < stations.Count; j++)
            {
                if (stations[j].Numb.Equals(station))
                {
                    stations[j].AddTelephone(numb);
                }
            }
        }

        static string channels(List<Station> stations)
        {
            string msg = string.Empty;
            for (int j = 0; j < stations.Count; j++)
            {
                for (int i = 0; i < stations[j].telephones.Count; i++)
                {
                    msg += stations[j].telephones[i].Channel + ", ";
                }
            }
            return msg;
        }

        static string ShowChannel(List<Station> stations, string station, string numb)
        {
            for (int j = 0; j < stations.Count; j++)
            {
                if (stations[j].Numb.Equals(station))
                {
                    for (int i = 0; i < stations[j].telephones.Count; i++)
                    {
                        if (stations[j].telephones[i].Name.Equals(numb))
                        {
                            return stations[j].telephones[i].Channel;
                        }
                    }
                }
            }
            return string.Empty;
        }

        static void ChannelUse(List<Station> stations, string station, string numb, string channelName)
        {
            for (int j = 0; j < stations.Count; j++)
            {
                if (stations[j].Numb.Equals(station))
                {
                    for (int i = 0; i < stations[j].telephones.Count; i++)
                    {
                        if (stations[j].telephones[i].Name.Equals(numb))
                        {
                            stations[j].telephones[i].Channel = channelName;
                            break;
                        }
                    }
                }
            }
        }

        static void ChannelUnUse(List<Station> stations, string station, string numb)
        {
            for (int j = 0; j < stations.Count; j++)
            {
                if (stations[j].Numb.Equals(station))
                {
                    for (int i = 0; i < stations[j].telephones.Count; i++)
                    {
                        if (stations[j].telephones[i].Name.Equals(numb))
                        {
                            stations[j].telephones[i].Channel = string.Empty;
                            break;
                        }
                    }
                }
            }
        }

        static void UnCall(List<Station> stations, string station, string numb)
        {
            for (int j = 0; j < stations.Count; j++)
            {
                if (stations[j].Numb.Equals(station))
                {
                    for (int i = 0; i < stations[j].telephones.Count; i++)
                    {
                        if (stations[j].telephones[i].Name.Equals(numb))
                        {
                            stations[j].telephones[i].Status = "free";
                            break;
                        }
                    }
                }
            }
        }

        static void Call(List<Station> stations, string station, string numb)
        {
            for (int j = 0; j < stations.Count; j++)
            {
                if (stations[j].Numb.Equals(station))
                {
                    for (int i = 0; i < stations[j].telephones.Count; i++)
                    {
                        if (stations[j].telephones[i].Name.Equals(numb))
                        {
                            stations[j].telephones[i].Status = "NotFree";
                            break;
                        }
                    }
                }
            }
        }

        static void UnUsing(List<Station> stations, string station, string numb)
        {
            for (int j = 0; j < stations.Count; j++)
            {
                if (stations[j].Numb.Equals(station))
                {
                    for (int i = 0; i < stations[j].telephones.Count; i++)
                    {
                        if (stations[j].telephones[i].Name.Equals(numb))
                        {
                            stations[j].telephones[i].IsTake = false;
                            stations[j].telephones[i].IPAddress = string.Empty;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        static void Using(List<Station> stations, string station, string numb, string ip)
        {
            for (int j = 0; j < stations.Count; j++)
            {
                if (stations[j].Numb.Equals(station))
                {
                    for (int i = 0; i < stations[j].telephones.Count; i++)
                    {
                        if (stations[j].telephones[i].Name.Equals(numb))
                        {
                            stations[j].telephones[i].IsTake = true;
                            stations[j].telephones[i].IPAddress = ip;
                            break;
                        }
                    }
                }
            }
        }

        static string TelephonesNumbers(List<Station> stations, string station)
        {
            string result = string.Empty;
            for (int j = 0; j < stations.Count; j++)
            {
                if (stations[j].Numb.Equals(station))
                {
                    for (int i = 0; i < stations[j].telephones.Count; i++)
                    {
                        if (!stations[j].telephones[i].IsTake)
                        {
                            result += $"{stations[j].telephones[i].Name};";
                        }
                    }
                }
            }
            return result;
        }

        static string InNetwork(List<Station> stations)
        {
            string result = string.Empty;
            for (int j = 0; j < stations.Count; j++)
            {
                for (int i = 0; i < stations[j].telephones.Count; i++)
                {
                    if (stations[j].telephones[i].IsTake && stations[j].telephones[i].Status.Equals("free"))
                    {
                        result += $"({stations[j].Numb}){stations[j].telephones[i].Name};";
                    }
                }
               
            }
            return result;
        }
    }
}
