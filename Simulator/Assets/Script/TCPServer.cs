using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPServer : SingletonMonoBehaviour<TCPServer>
{
    private TcpListener server;
    private Thread serverThread;
    private bool isRunning;

    // 변수 선언
    public float M_s = 0.0f;
    public float M_u = 0.0f ;
    public float K_s = 0.0f;
    public float K_t = 0.0f;
    public float c1 = 0.0f;
    public float c2 = 0.0f;
    public float sampling_time = 0.0f;
    public float total_time = 0.0f;


    public Dictionary<string, float> parameters = new Dictionary<string, float>();
    void Start()
    {
        StartServer();
    }

    void OnApplicationQuit()
    {
        StopServer();
    }

    private void StartServer()
    {
        serverThread = new Thread(new ThreadStart(RunServer));
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    private void StopServer()
    {
        isRunning = false;
        server.Stop();
        serverThread.Abort();
    }

    private void RunServer()
    {
        try
        {
            server = new TcpListener(IPAddress.Any, 9999);
            server.Start();
            isRunning = true;
            Debug.Log("서버 시작됨. 클라이언트 연결 대기 중...");

            while (isRunning)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log("받은 메시지: " + message);

                // 메시지 파싱
                ParseMessage(message);
                StoreParameters();

                stream.Close();
                client.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("서버 오류: " + e.Message);
        }
    }

    private void ParseMessage(string message)
    {
        string[] parameters = message.Split(',');

        foreach (string parameter in parameters)
        {
            string[] keyValue = parameter.Split('=');
            if (keyValue.Length == 2)
            {
                string key = keyValue[0].Trim();
                string value = keyValue[1].Trim();

                switch (key)
                {
                    case "M_s":
                        M_s = float.Parse(value);
                        break;
                    case "M_u":
                        M_u = float.Parse(value);
                        break;
                    case "K_s":
                        K_s = float.Parse(value);
                        break;
                    case "K_t":
                        K_t = float.Parse(value);
                        break;
                    case "c1":
                        c1 = float.Parse(value);
                        break;
                    case "c2":
                        c2 = float.Parse(value);
                        break;
                    case "sampling_time":
                        sampling_time = float.Parse(value);
                        break;
                    case "total_time":
                        total_time = float.Parse(value);
                        break;
                    default:
                        Debug.LogWarning("알 수 없는 키: " + key);
                        break;
                }
            }
        }

        //Debug.Log($"수신된 값들: M_s={M_s}, M_u={M_u}, K_s={K_s}, K_t={K_t}, c1={c1}, c2={c2}, sampling_time={sampling_time}, total_time={total_time}");
    }

    private void StoreParameters()
    {
        parameters["m1"] = M_s;
        parameters["m2"] = M_u;
        parameters["k1"] = K_s;
        parameters["k2"] = K_t;
        parameters["c1"] = c1;
        parameters["c2"] = c2;
        parameters["totalTime"] = total_time;
        parameters["samplingTime"] = sampling_time;

        foreach (var param in parameters)
        {
            Debug.Log($"Parameter {param.Key}: {param.Value}");
        }
    }
}


