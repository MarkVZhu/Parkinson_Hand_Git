using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
public class UDPReceive : Singleton<UDPReceive>
{
	Thread receiveThread;
	[HideInInspector]public UdpClient client;
	public int port = 5040;
	public bool startReceiving = true;
	public bool printToConsole = false;
	public string data;
	
	public void Start()
	{
		receiveThread = new Thread(new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}
	
	//FIXME:娱乐模式回到主菜单再开始娱乐模式报错
	private void ReceiveData()
	{
		client = new UdpClient(port);
		while (startReceiving)
		{
			try
			{
			IPEndPoint anylP = new IPEndPoint(IPAddress.Any, 0);
			byte[] dataByte = client.Receive(ref anylP);
			data = Encoding.UTF8.GetString(dataByte);
			if (printToConsole){ print(data);}
			}
			
			catch(Exception err)
			{
				print(err.ToString());
			}
		}
	}
}



 

