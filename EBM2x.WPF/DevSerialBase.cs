using System;
using System.IO;
using System.IO.Ports;

namespace EBM2x.WPF.device
{
	/// <summary>
	/// Description of DevSerialBase.
	/// </summary>
	public class DevSerialBase
	{
		//----------------------------------------------------------------------------
		// Variables declaration
		//----------------------------------------------------------------------------

        private SerialPort com = null;

        public SerialPort Com
        {
            get { return com; }
            set { com = value; }
        }
		SerialDataReceivedEventHandler dataReceivedEventHandler = null;

		public DevSerialBase()
		{
			com = new SerialPort();
		}

		//----------------------------------------------------------------------------
		// Serial interface implement
		//----------------------------------------------------------------------------
		
		public bool SerialOpen()
		{
			try
			{
				if(!com.IsOpen)
				{
					com.WriteBufferSize = 10240;
					com.RtsEnable = true;
					com.DtrEnable = true;
					com.Open();
				}
				return true;
			}
			catch (Exception e)
			{
				throw new Exception(e.ToString());
			}
		}
		public bool SerialClose()
		{
			try
			{
				if(com.IsOpen)
				{
					com.Close();
					com.Dispose();
				}
				return true;
			}
			catch(Exception e)
			{
				throw new Exception(e.ToString());
			}
		}

		public string SerialRead()
		{
			string strBuff = string.Empty;
			System.DateTime dtStart;
			int nTimeOut = com.ReadTimeout;
			
			dtStart = DateTime.Now;
			if(nTimeOut == 0x00) nTimeOut = 500;

			while(true)
			{
				if( dtStart.AddMilliseconds(nTimeOut) <= DateTime.Now )
				{
					return strBuff;
				}

				try
				{
					if(com.BytesToRead > 0)
					{
                        int nBytesToRead = com.BytesToRead;
                        byte[] buffer = new byte[nBytesToRead];
                        com.Read(buffer, 0, nBytesToRead);
						strBuff += System.Text.Encoding.Default.GetString(buffer);
						if(buffer[buffer.Length - 1] == 0x0D)
							return strBuff;
					}
					else
					{
						System.Threading.Thread.Sleep(100);
					}
				}
				catch(TimeoutException)
				{
					return strBuff;
				}
				catch(Exception e)
				{
					throw new Exception("SerailRead Exception", e);
				}
			}
		}

        // 고정 스캐너 전용
        public string SerialScanRead()
        {
            string strBuff = string.Empty;
            System.DateTime dtStart;
            int nTimeOut = com.ReadTimeout;

            dtStart = DateTime.Now;
            if (nTimeOut == 0x00) nTimeOut = 500;

            while (true)
            {
                if (dtStart.AddMilliseconds(nTimeOut) <= DateTime.Now)
                {
                    return strBuff;
                }

                try
                {
                    if (com.BytesToRead > 0)
                    {
                        int nBytesToRead = com.BytesToRead;
                        byte[] buffer = new byte[nBytesToRead];
                        com.Read(buffer, 0, nBytesToRead);
                        strBuff += System.Text.Encoding.Default.GetString(buffer);

                        if (buffer[buffer.Length - 1] == 0x0D ||
                            buffer[buffer.Length - 1] == 0x0A)
                            return strBuff;
                    }
                    else
                    {
                    }
                }
                catch (TimeoutException)
                {
                    return strBuff;
                }
                catch (Exception e)
                {
                    throw new Exception("SerailRead Exception", e);
                }
            }
        }

        public byte[] SerialRead(int nMaxLen, int nTimeOut)
		{
			byte[] btRcvBuff = new byte[1000];
			int i = 0;
			System.DateTime dtStart;
			
			dtStart = DateTime.Now;
			if(nTimeOut == 0x00) nTimeOut = 500;
			com.ReadTimeout = nTimeOut;

			// 쓰레기 값 제거
			com.DiscardInBuffer();

			while(true)
			{
				if( dtStart.AddMilliseconds(nTimeOut) <= DateTime.Now )
				{
					return null;
				}
				try
				{
					if(com.BytesToRead > 0)
					{
						btRcvBuff[i++] = (byte)com.ReadByte();

						// 최대값 까지 READ 하며 return
						if(i >= nMaxLen || i >= 1000)
						{
							com.DiscardInBuffer();
							return btRcvBuff;
						}
					}
					else
					{
						System.Threading.Thread.Sleep(10);
					}
				}
				catch(TimeoutException)
				{
					return btRcvBuff;
				}
				catch(Exception e)
				{
					throw new Exception("SerailRead Exception", e);
				}
			}
		}
		
		public void SerialWrite(string stData)
		{
			try
			{
				com.Write(stData);
			}
			catch(Exception e)
			{
				throw new Exception("SerialWrite Exception", e);
			}
		}

		public void SerialWrite(byte[] btData)
		{
			try
			{
				com.Write(btData, 0, btData.Length);
			}
			catch(Exception e)
			{
				throw new Exception("SerialWrite Exception", e);
			}
		}
		
		// AsyncCallback처리
		public void AsyncSerialWrite(byte[] btData)
		{
			try{
                com.BaseStream.BeginWrite(btData, 0, btData.Length, null, null);
			}
			catch(Exception e){
				throw new Exception("AsyncSerialWrite Exception", e);
			}
		}

		public void setPort(int intPort)
		{
			this.com.PortName = "COM" + intPort.ToString();
		}
		public void setPort(string strPort)
		{
			this.com.PortName = strPort;
		}

		public void setBaudRate(int intBaudRate)
		{
			this.com.BaudRate = intBaudRate;
		}
		public void setBaudRate(string strBaudRate)
		{
			this.com.BaudRate = int.Parse(strBaudRate);
		}

		public void setParity(System.IO.Ports.Parity parity)
		{
			this.com.Parity = parity;
		}
		public void setParity(string strParity)
		{
			this.com.Parity = (Parity)Enum.Parse(typeof(Parity), strParity);
		}
		
		public void setDataBits(int intDataBits)
		{
			this.com.DataBits = intDataBits;
		}
		public void setDataBits(string strDataBits)
		{
			this.com.DataBits = int.Parse(strDataBits);
		}

		public void setStopBits(StopBits stopBits)
		{
			this.com.StopBits = stopBits;
		}
		public void setStopBits(string strStopBits)
		{
			this.com.StopBits = (StopBits)Enum.Parse(typeof(StopBits), strStopBits);
		}

		public void setTimeout(int inTime, int OutTime)
		{
			com.ReadTimeout  = inTime;
			com.WriteTimeout = OutTime;
		}
		
		public void clearOutBuffer()
		{
			com.DiscardOutBuffer();
		}
		
		public void clearInBuffer()
		{
            try
            {
                com.DiscardInBuffer();
            }
            catch (Exception e)
            {
            }
		}
		
		public void addEventHandler(SerialDataReceivedEventHandler handler)
		{
            try
            {
                removeEventHandler();
                this.dataReceivedEventHandler = new SerialDataReceivedEventHandler(handler);
                com.DataReceived += dataReceivedEventHandler;
            }
            catch (Exception e)
            {
            }
		}

		public void removeEventHandler()
		{
			if( this.dataReceivedEventHandler != null )
			{
				com.DataReceived -= dataReceivedEventHandler;
				dataReceivedEventHandler = null;
			}
		}
	}
}
