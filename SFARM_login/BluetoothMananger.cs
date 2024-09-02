using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

public class BluetoothManager
{
    private static BluetoothManager _instance;
    private static readonly object _lock = new object();
    private BluetoothClient _bluetoothClient;
    private BluetoothClient _connectedClient;
    private const string DEVICE_NAME = "SFARM3";  // Bluetooth 장치 이름

    public event Action<string> DataReceived;

    private BluetoothManager()
    {
        _bluetoothClient = new BluetoothClient();
        ConnectToDevice();
    }

    public static BluetoothManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new BluetoothManager();
                }
                return _instance;
            }
        }
    }

    private void ConnectToDevice()
    {
        BluetoothDeviceInfo[] devices = _bluetoothClient.DiscoverDevices().ToArray();
        BluetoothDeviceInfo device = devices.FirstOrDefault(d => d.DeviceName == DEVICE_NAME);

        if (device != null)
        {
            BluetoothAddress address = device.DeviceAddress;
            BluetoothEndPoint endPoint = new BluetoothEndPoint(address, BluetoothService.SerialPort);
            _connectedClient = new BluetoothClient();
            _connectedClient.Connect(endPoint);

            // 데이터 수신을 위한 스레드 생성
            Thread receiveThread = new Thread(ReceiveData);
            receiveThread.Start(_connectedClient);
        }
        else
        {
            Console.WriteLine("Bluetooth 장치를 찾을 수 없습니다.");
        }
    }

    private async void ReceiveData(object obj)
    {
        BluetoothClient client = (BluetoothClient)obj;

        try
        {
            while (true)
            {
                byte[] buffer = new byte[256];
                int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string data = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string pattern = @"Soil_moisture:\d+, Temp:[\d\.]+, Humid:[\d\.]+, 조도:\d+, Water Level:\d+";

                    Match match = Regex.Match(data, pattern);

                    if (match.Success)
                    {
                        //.WriteLine(match.Value);
                        string result = match.Value;
                        DataReceived?.Invoke(result);
                        Debug.WriteLine("Received: " + data);
                    }

                    // 데이터 수신 이벤트 호출
                await Task.Delay(5000);      
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("데이터 수신 중 오류 발생: " + ex.Message);
        }
        finally
        {
            client.Close();
        }
    }

    public void SendData(string data)
    {
        if (_connectedClient != null && _connectedClient.Connected)
        {
            try
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(data);
                _connectedClient.GetStream().Write(buffer, 0, buffer.Length);
                Console.WriteLine("Sent: " + data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("데이터 전송 중 오류 발생: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Bluetooth 장치와 연결되어 있지 않습니다.");
        }
    }
    public void Disconnect()
    {
        if (_connectedClient != null)
        {
            _connectedClient.Close();
            _connectedClient = null;
        }
    }
    public void Dispose()
    {
        Disconnect();
        _bluetoothClient.Close();
    }

    public BluetoothClient GetConnectedClient()
    {
        return _connectedClient;
    }
}
