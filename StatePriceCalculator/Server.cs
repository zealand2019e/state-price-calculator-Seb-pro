using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace StatePriceCalculator
{
    class Server
    {
        public static void Start()
        {
            //Creating an instance of TCPlistener class that listen on a specified port
            TcpListener serverSocket = new TcpListener(7777);

            //Start server
            Console.WriteLine("Waiting for a connection...");
            serverSocket.Start();

            //Establish a TCP connection and accept all pending connection request
            TcpClient connectionSocket = serverSocket.AcceptTcpClient();
            Console.WriteLine("Connection established or enter c to close the connection");

            //Using the client method
            DoClient(connectionSocket);

            //Closing the TCP listener
            serverSocket.Stop();
            Console.WriteLine("Server Stop ");
        }

        //Client method to handle the client 
        public static void DoClient(TcpClient connectionSocket)
        {
            //Creating a stream of data, that can both been read, and write from a byte stream
            NetworkStream ns = connectionSocket.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; //Will auto flush





            //Making a while loop that display the enter message from the user
            while (true)
            {
                sw.WriteLine("you are connected press c to disconnect\nEnter have many item and the price and which state with an separator");

                string message = sr.ReadLine();

                //If the user enters c the connection will close down
                if (message.ToLower() == "c")
                {
                    break;
                }

                Console.WriteLine("Received Message: " + message);
                if (message != null)
                {
                    //Brugerens input
                    int items = Convert.ToInt32(message.Split(" ")[0]);

                    //Brugerens input
                    double price = Convert.ToDouble(message.Split(" ")[1]);


                    string state = message.Split(" ")[2];


                    //totalt pris
                    double total = items * price;

                    //Udregen rabat
                    double discount = 0;
                    //rabatten er 3% hvis prisen er over 1000 men under 5000
                    if (total >= 1000 && total < 5000)
                    {
                        discount = 3;

                    }
                    //rabatten er 5% hvis prisen er over 5000 men under 7000
                    if (total >= 5000 && total < 7000)
                    {
                        discount = 5;
                    }
                    //rabatten er 7% hvis prisen er over 7000 men under 10000
                    if (total >= 7000 && total < 10000)
                    {
                        discount = 7;

                    }
                    //rabatten er 10% hvis prisen er over 10000 men under 50000
                    if (total >= 10000 && total < 50000)
                    {
                        discount = 10;

                    }
                    //rabatten er 3% hvis prisen er over 50000
                    if (total >= 50000)
                    {
                        discount = 50;
                    }

                    //Udrenger den nye pris total prisen skal minus med rabaten som er det total beløb / 100 og gange med rabat procenten
                    double priceWithDiscount = total - (discount * (total / 100));

                    //Viser hvad prisen er med rabat og hvor mange % man har fået i rabat
                    string displayDiscount = " You're discount is " + discount + "%";

                    switch (state)
                    {
                        case "UT":
                            priceWithDiscount *= 1.0685;
                            break;
                        case "NV":
                            priceWithDiscount *= 1.08;
                            break;
                        case "TX":
                            priceWithDiscount *= 1.0625;
                            break;
                        case "AL":
                            priceWithDiscount *= 1.04;
                            break;
                        case "CA":
                            priceWithDiscount *= 1.0825;
                            break;
                    }
                    //udkommet af brugerens input
                    Console.WriteLine();
                    sw.WriteLine(priceWithDiscount + displayDiscount);

                }


            }

            sw.WriteLine("c");
            //Closing the stream of data and close the TCP connection
            ns.Close();
            Console.WriteLine("Net stream closed");
            connectionSocket.Close();
            Console.WriteLine("Connection socket closed");
        }
    }
}
