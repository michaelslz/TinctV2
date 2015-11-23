using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tinct.Net.Communication.Interface;

namespace Tinct.Net.Communication.Connect
{
    public class TinctConnect : ITinctConnect
    {
        private TcpListener server = null;



        public event EventHandler<TinctConnectReceiveArgs> ReceiveRestMessage;
        public event EventHandler<TinctConnectReceiveArgs> AfterReceiveMessage;
        public event EventHandler<TinctConnectReceiveArgs> ConnectMessage;


        private void RaiseAfterReceiveMessage(TinctConnectReceiveArgs e)
        {
            if (AfterReceiveMessage != null)
            {
                AfterReceiveMessage(this, e);
            }
        }

        private void RaiseReceiveRestMessage(TinctConnectReceiveArgs e)
        {
            if (ReceiveRestMessage != null)
            {
                ReceiveRestMessage(this, e);
            }
        }

        private void RaiseConnectMessage(TinctConnectReceiveArgs e)
        {
            if (ConnectMessage != null)
            {
                ConnectMessage(this, e);
            }
        }

        public bool Connect(string machineName, int port)
        {
            try
            {
                string message = "Tinct";
                TcpClient client = new TcpClient(machineName, port);
                Byte[] data = System.Text.Encoding.Unicode.GetBytes(message);
                NetworkStream stream = client.GetStream();
                //send
                stream.Write(data, 0, data.Length);
                data = new Byte[256];
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.Unicode.GetString(data, 0, bytes);

                stream.Close();
                client.Close();
                if (responseData == "Tinct")
                {
                    return true;
                }
            }
            catch (ArgumentNullException e)
            {
                //log
                Console.WriteLine(e.Message);
                return false;
            }
            catch (SocketException e)
            {
                //log
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public void SendMessage(string machineName, int port, string message, bool checkconnect)
        {
            if (checkconnect)
            {
                if (!Connect(machineName, port))
                {
                    Console.WriteLine("can not connect {0}", machineName);
                    return;
                    //throw new Exception("please check netlog ");
                }
            }

            try
            {
                TcpClient client = new TcpClient(machineName, port);
                Byte[] data = System.Text.Encoding.Unicode.GetBytes(message);
                NetworkStream stream = client.GetStream();
                //send
                stream.Write(data, 0, data.Length);
                //data = new Byte[256];
                // String responseData = String.Empty;
                //Int32 bytes = stream.Read(data, 0, data.Length);
                // responseData = System.Text.Encoding.Unicode.GetString(data, 0, bytes);

                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                //log
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                //log
            }

        }


        public bool StartSlaveServer(int port)
        {
            try
            {
                IPAddress myIPAddress = null;
                IPAddress[] myIPAddresses = (IPAddress[])Dns.GetHostAddresses(Dns.GetHostName());
                foreach (var add in myIPAddresses)
                {
                    try
                    {
                        long a = add.ScopeId;
                    }
                    catch
                    {
                        myIPAddress = add;
                    }
                }
                if (myIPAddress == null)
                {
                    //log
                    return false;
                }
                Console.WriteLine("IPAddress is {0}", myIPAddress.ToString());
                server = new TcpListener(myIPAddress, port);
                server.Start();
               
               
       
                while (true)
                {
               
                    Console.Write("Waiting for a connection... ");
                    List<byte> messagebytes = new List<byte>();
                    messagebytes.Clear();
                
            
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    NetworkStream stream = client.GetStream();


                    String data = null;
                    Byte[] bytes = new Byte[256];
                    StringBuilder messagebuilder = new StringBuilder();
                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.Unicode.GetString(bytes, 0, i);

                        if (data == "Tinct")
                        {
                            byte[] msg = System.Text.Encoding.Unicode.GetBytes(data);
                            stream.Write(msg, 0, msg.Length);
                        }
                        messagebuilder.Append(data);
                       // messagebytes.AddRange(bytes);
                       bytes = new Byte[256];
                    }
                    string message = messagebuilder.ToString();
                   // string message = System.Text.Encoding.Unicode.GetString(messagebytes.ToArray());

                    if (message.IndexOf('\0') != -1)
                    {
                        message = message.Substring(0, message.IndexOf('\0'));
                    }
                    TinctConnectReceiveArgs args = new TinctConnectReceiveArgs();
                    args.ReceivedMessage = message;
                    if (message.Trim() != "Tinct")
                    {
                        RaiseAfterReceiveMessage(args);
                    }
                    else
                    {
                        RaiseConnectMessage(args);
                    }
                    stream.Close(); 
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                //log
                return false;
            }
            finally
            {
                server.Stop();
            }

        }

        public bool StartMasterServer(int port)
        {
            try
            {
                IPAddress myIPAddress = null;
                IPAddress[] myIPAddresses = (IPAddress[])Dns.GetHostAddresses(Dns.GetHostName());
                foreach (var add in myIPAddresses)
                {
                    try
                    {
                        long a = add.ScopeId;
                    }
                    catch
                    {
                        myIPAddress = add;
                    }
                }
                if (myIPAddress == null)
                {
                    //log
                    return false;
                }
                Console.WriteLine("IPAddress is {0}", myIPAddress.ToString());
                server = new TcpListener(myIPAddress, port);
                server.Start();
                List<byte> messagebytes = new List<byte>();
        
                String data = null;
                while (true)
                {
                    messagebytes.Clear();
                    Console.Write("Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    data = null;
                    Byte[] bytes = new Byte[256];

                    NetworkStream stream = client.GetStream();
                    StringBuilder messagebuilder = new StringBuilder();
                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.Unicode.GetString(bytes, 0, i);

                        if (data == "Tinct")
                        {
                            byte[] msg = System.Text.Encoding.Unicode.GetBytes(data);
                            stream.Write(msg, 0, msg.Length);
                        }
                        messagebuilder.Append(data);
                        // messagebytes.AddRange(bytes);
                        bytes = new Byte[256];
                    }
                    string message = messagebuilder.ToString();
                    // string message = System.Text.Encoding.Unicode.GetString(messagebytes.ToArray());
                    if (message.IndexOf('\0') != -1)
                    {
                        message = message.Substring(0, message.IndexOf('\0'));
                    }
                    TinctConnectReceiveArgs args = new TinctConnectReceiveArgs();
                    args.ReceivedMessage = message;
                    if (message.Trim() != "Tinct")
                    {
                        RaiseReceiveRestMessage(args);
                    }

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                //log
                return false;
            }
            finally
            {
                server.Stop();
            }

        }


        public void EndServer()
        {
            if (server != null)
            {
                server.Stop();
            }
        }

    }
}
