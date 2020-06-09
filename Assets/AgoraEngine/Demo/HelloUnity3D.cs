using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using TMPro;
using UnityEngine.SceneManagement;
#if (UNITY_2018_3_OR_NEWER)
using UnityEngine.Android;
#endif
using agora_gaming_rtc;

public class HelloUnity3D : MonoBehaviour
{
    public InputField mChannelNameInputField;
    public Text mShownMessage;
    public Text versionText;
    public Button joinChannel;
    public Button leaveChannel;
    public Button muteButton;
    public Button telephoneButton;
    public TMP_Dropdown telephoneNumbers;
    public TMP_Dropdown networkList;
    public Button callButton;
    public Button updateButton;

    public GameObject callButtonObject;
    

    public GameObject recieve;

    string channelName = string.Empty;

    private IRtcEngine mRtcEngine = null;
    private int port = 11000;
    private int portHost = 12000;
    private string address = BrowserLoad.address;

    string firstChannel = "firstChanel";
    string secondChannel = "secondChannel";
    string thirdChannel = "thirdChannel";
    string fourthChannel = "fourthChannel";

    public static bool isCall = false;
    public static string caller = string.Empty;
    public static string phone = string.Empty;
    public GameObject callMenu;
    public TextMeshProUGUI text;
    public GameObject sound;

    public bool isUseChannel = false;

    public Color32 greenCall = new Color32(25, 125, 42, 255);
    public Color32 redCall = new Color32(186, 27, 0, 255);

    // PLEASE KEEP THIS App ID IN SAFE PLACE
    // Get your own App ID at https://dashboard.agora.io/
    // After you entered the App ID, remove ## outside of Your App ID
    [SerializeField]
    private string appId = "540f5bb09c42431a804febeda07ca380";

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        muteButton.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
#if (UNITY_2018_3_OR_NEWER)
			if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
			{
			
			} 
			else 
			{
				Permission.RequestUserPermission(Permission.Microphone);
			}
#endif
        //joinChannel.onClick.AddListener(JoinChannel);
        //leaveChannel.onClick.AddListener(LeaveChannel);
        muteButton.onClick.AddListener(MuteButtonTapped);
        telephoneButton.onClick.AddListener(ReserveTelephoneNumb);
        callButton.onClick.AddListener(Call);
        updateButton.onClick.AddListener(CreateUserlist);

        mRtcEngine = IRtcEngine.GetEngine(appId);
        versionText.GetComponent<Text>().text = "Version : " + getSdkVersion();

        mRtcEngine.OnJoinChannelSuccess += (string channelName, uint uid, int elapsed) =>
        {
            string joinSuccessMessage = string.Format("joinChannel callback uid: {0}, channel: {1}, version: {2}", uid, channelName, getSdkVersion());
            Debug.Log(joinSuccessMessage);
            mShownMessage.GetComponent<Text>().text = (joinSuccessMessage);
            muteButton.enabled = true;
        };

        mRtcEngine.OnLeaveChannel += (RtcStats stats) =>
        {
            string leaveChannelMessage = string.Format("onLeaveChannel callback duration {0}, tx: {1}, rx: {2}, tx kbps: {3}, rx kbps: {4}", stats.duration, stats.txBytes, stats.rxBytes, stats.txKBitRate, stats.rxKBitRate);
            Debug.Log(leaveChannelMessage);
            mShownMessage.GetComponent<Text>().text = (leaveChannelMessage);
            muteButton.enabled = false;
            // reset the mute button state
            if (isMuted)
            {
                MuteButtonTapped();
            }
        };

        mRtcEngine.OnUserJoined += (uint uid, int elapsed) =>
        {
            string userJoinedMessage = string.Format("onUserJoined callback uid {0} {1}", uid, elapsed);
            Debug.Log(userJoinedMessage);
        };

        mRtcEngine.OnUserOffline += (uint uid, USER_OFFLINE_REASON reason) =>
        {
            string userOfflineMessage = string.Format("onUserOffline callback uid {0} {1}", uid, reason);
            Debug.Log(userOfflineMessage);
        };

        mRtcEngine.OnVolumeIndication += (AudioVolumeInfo[] speakers, int speakerNumber, int totalVolume) =>
        {
            if (speakerNumber == 0 || speakers == null)
            {
                Debug.Log(string.Format("onVolumeIndication only local {0}", totalVolume));
            }

            for (int idx = 0; idx < speakerNumber; idx++)
            {
                string volumeIndicationMessage = string.Format("{0} onVolumeIndication {1} {2}", speakerNumber, speakers[idx].uid, speakers[idx].volume);
                Debug.Log(volumeIndicationMessage);
            }
        };

        mRtcEngine.OnUserMutedAudio += (uint uid, bool muted) =>
        {
            string userMutedMessage = string.Format("onUserMuted callback uid {0} {1}", uid, muted);
            Debug.Log(userMutedMessage);
        };

        mRtcEngine.OnWarning += (int warn, string msg) =>
        {
            string description = IRtcEngine.GetErrorDescription(warn);
            string warningMessage = string.Format("onWarning callback {0} {1} {2}", warn, msg, description);
            Debug.Log(warningMessage);
        };

        mRtcEngine.OnError += (int error, string msg) =>
        {
            string description = IRtcEngine.GetErrorDescription(error);
            string errorMessage = string.Format("onError callback {0} {1} {2}", error, msg, description);
            Debug.Log(errorMessage);
        };

        mRtcEngine.OnRtcStats += (RtcStats stats) =>
        {
            string rtcStatsMessage = string.Format("onRtcStats callback duration {0}, tx: {1}, rx: {2}, tx kbps: {3}, rx kbps: {4}, tx(a) kbps: {5}, rx(a) kbps: {6} users {7}",
                stats.duration, stats.txBytes, stats.rxBytes, stats.txKBitRate, stats.rxKBitRate, stats.txAudioKBitRate, stats.rxAudioKBitRate, stats.userCount);
            Debug.Log(rtcStatsMessage);

            int lengthOfMixingFile = mRtcEngine.GetAudioMixingDuration();
            int currentTs = mRtcEngine.GetAudioMixingCurrentPosition();

            string mixingMessage = string.Format("Mixing File Meta {0}, {1}", lengthOfMixingFile, currentTs);
            Debug.Log(mixingMessage);
        };

        mRtcEngine.OnAudioRouteChanged += (AUDIO_ROUTE route) =>
        {
            string routeMessage = string.Format("onAudioRouteChanged {0}", route);
            Debug.Log(routeMessage);
        };

        mRtcEngine.OnRequestToken += () =>
        {
            string requestKeyMessage = string.Format("OnRequestToken");
            Debug.Log(requestKeyMessage);
        };

        mRtcEngine.OnConnectionInterrupted += () =>
        {
            string interruptedMessage = string.Format("OnConnectionInterrupted");
            Debug.Log(interruptedMessage);
        };

        mRtcEngine.OnConnectionLost += () =>
        {
            string lostMessage = string.Format("OnConnectionLost");
            Debug.Log(lostMessage);
        };

        mRtcEngine.SetLogFilter(LOG_FILTER.INFO);

        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_COMMUNICATION);

        // mRtcEngine.SetChannelProfile (CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        // mRtcEngine.SetClientRole (CLIENT_ROLE.BROADCASTER);

        CreateTelephoneList();
        CreateUserlist();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isCall)
        {
            CallMenuEnable();
            isCall = false;
        }
    }

    public void CallMenuEnable()
    {
        callButtonObject.SetActive(false);
        callMenu.SetActive(true);
        text.text = $"Дзвінок від ({caller}){phone}";
        sound.GetComponent<AudioSource>().Play();
    }

    public void CancelCall()
    {
        callButtonObject.SetActive(true);
        sound.GetComponent<AudioSource>().Stop();
        callMenu.SetActive(false);
    }

    public void AcceptCall()
    {
        callButtonObject.SetActive(true);
        string tmp = CheckChannel(caller, phone);
        JoinChannel(tmp);
        sound.GetComponent<AudioSource>().Stop();    
        callMenu.SetActive(false);
        
    }

    public void Recieve()
    {
        recieve.SetActive(true);
    }

    public string CheckChannel(string caller, string phone)
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "showChannel, " + caller + ", " + phone;
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

    public string ToCheck(string text)
    {
        string str = text[1].ToString() + text[2].ToString() + text[3].ToString() + text[4].ToString() + ", " + text[6] + text[7];
        return str;
    }
    public string CheckChannelForLeave()
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "showChannel, " + GameManagerScript.currentPhoneUse;
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


    public string CheckChannel()
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "showChannel, " + ToCheck(networkList.captionText.text);
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

    public string CheckAllChanels()
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "showAll";
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

    public void BackScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Call()
    {
        string temp = CheckChannel();
        if (!isUseChannel)
        {
            if (!temp.Equals(string.Empty))
            {
                channelName = temp;
                JoinChannel(channelName);
            }
            else if (CheckAllChanels().Equals(string.Empty))
            {
                channelName = firstChannel;
                JoinChannel(channelName);
            }
            else
            {
                string msg = CheckAllChanels();
                if (msg.Contains(firstChannel))
                {
                    if (msg.Contains(secondChannel))
                    {
                        if (msg.Contains(thirdChannel))
                        {
                            if (msg.Contains(fourthChannel))
                            {

                            }
                            else
                            {
                                channelName = fourthChannel;
                                JoinChannel(channelName);
                            }
                        }
                        else
                        {
                            channelName = thirdChannel;
                            JoinChannel(channelName);
                        }
                    }
                    else
                    {
                        channelName = secondChannel;
                        JoinChannel(channelName);
                    }
                }
                else
                {
                    channelName = firstChannel;
                    JoinChannel(channelName);
                }
            }
            
            CallToIPAddress(GetIPAddress());
        }
        else
        {
            
            LeaveChannel();
            
        }
    }

    public void CallToIPAddress(string address)
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), portHost);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "call, " + GameManagerScript.tknumb + ", " + telephoneNumbers.captionText.text;
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
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public string GetIPAddress()
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "getIp, " + ToCheck(networkList.captionText.text);
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

            string ip = builder.ToString();

            // закрываем сокет
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            return ip;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return string.Empty;
        }
    }

    public void ChannelUse()
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "ch, " + GameManagerScript.tknumb + ", " + GameManagerScript.currentPhoneUse + ", " + channelName;
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
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    public void ChannelUnUse()
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "unCh, " + GameManagerScript.tknumb + ", " + GameManagerScript.currentPhoneUse;
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
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    public void UnUseTelephone()
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "unUse, "+ GameManagerScript.tknumb + ", " + GameManagerScript.currentPhoneUse;
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
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    public void UseTelephone()
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "use, " + GameManagerScript.tknumb + ", " + telephoneNumbers.captionText.text + ", " + GetLocalIPAddress();
            GameManagerScript.currentPhoneUse = telephoneNumbers.captionText.text;
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
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
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

    public void ReserveTelephoneNumb()
    {
        if (!GameManagerScript.currentPhoneUse.Equals(""))
        {
            UnUseTelephone();
        }
        UseTelephone();
    }

    public void CreateUserlist()
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "inNetwork";
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

            string[] temp = builder.ToString().Split(';');

            networkList.options.Clear();
            networkList.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;

            for (int i = 0; i < temp.Length - 1; i++)
            {
                networkList.options.Add(new TMP_Dropdown.OptionData(temp[i]));
            }

            networkList.GetComponentInChildren<TextMeshProUGUI>().text = networkList.options[0].text;

            // закрываем сокет
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void CreateTelephoneList()
    {
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            string message = "telephonesNumbers, " + GameManagerScript.tknumb;
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

            string[] temp = builder.ToString().Split(';');

            telephoneNumbers.options.Clear();
            telephoneNumbers.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;

            for (int i = 0; i < temp.Length - 1; i++)
            {
                telephoneNumbers.options.Add(new TMP_Dropdown.OptionData(temp[i]));
            }

            telephoneNumbers.GetComponentInChildren<TextMeshProUGUI>().text = telephoneNumbers.options[0].text;

            // закрываем сокет
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void JoinChannel(string channel)
    {
        

        Debug.Log(string.Format("I join"));

        if (string.IsNullOrEmpty(channel))
        {
            return;
        }
        ChannelUse();
        isUseChannel = true;
        callButton.GetComponent<Image>().color = redCall;
        mRtcEngine.JoinChannel(channel, "extra", 0);

       
    }

    public void LeaveChannel()
    {
        // int duration = mRtcEngine.GetAudioMixingDuration ();
        // int current_duration = mRtcEngine.GetAudioMixingCurrentPosition ();

        // IAudioEffectManager effect = mRtcEngine.GetAudioEffectManager();
        // effect.StopAllEffects ();
        ChannelUnUse();
        callButton.GetComponent<Image>().color = greenCall;
        isUseChannel = false;
        mRtcEngine.LeaveChannel();
        string channelName = mChannelNameInputField.text.Trim();
        Debug.Log(string.Format($"I leave {channelName}"));
        
    }

    void OnApplicationQuit()
    {
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();
        }
    }


    public string getSdkVersion()
    {
        string ver = IRtcEngine.GetSdkVersion();
        if (ver == "2.9.1.45")
        {
            ver = "2.9.2";  // A conversion for the current internal version#
        }
        else
        {
            if (ver == "2.9.1.46")
            {
                ver = "2.9.2.1";  // A conversion for the current internal version#
            }
        }
        return ver;
    }


    bool isMuted = false;
    void MuteButtonTapped()
    {
        string labeltext = isMuted ? "Mute" : "Unmute";
        Text label = muteButton.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.text = labeltext;
        }
        isMuted = !isMuted;
        mRtcEngine.EnableLocalAudio(!isMuted);
    }
}
