using System;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
//using System.Windows.Forms;

namespace EBM2x.WPF.device
{
	/// <summary>
	/// Description of DevSerialPrinter.
	/// </summary>
    public class DevSerialPrinter : DevSerialBase
	{
		//----------------------------------------------------------------------------
		// Variables declaration
		//----------------------------------------------------------------------------
		private string strDefaultDeiveName            = string.Empty ; 	// Default Device Name 설정 변수
		private PrinterRecord PRN = null;	// Printer Command Record 변수

		private MemoryStream printBuff = null;
		private string strBarCode      = string.Empty ; 					// BarCode 할 Buffer
		
		public DevSerialPrinter()
		{
			clrPrintBuff();
			PRN = new PrinterRecord();
		}

        /*
		public void SetDeviceName(string valName)
		{
			strDefaultDeiveName = valName;
			// command load
			PRN.toRecord( valName );
		}
        */
		private void addPrintBuff( byte[] value )
		{
			printBuff.Write( value, 0, value.Length );
		}
        public void printPrintBuff(byte[] bytes)
        {
            addPrintBuff(bytes);
        }

        private void clrPrintBuff()
		{
			if( printBuff != null ) printBuff.Dispose();
			printBuff = new MemoryStream();
		}
		
		//----------------------------------------------------------------------------
		// Printer interface implement
		//----------------------------------------------------------------------------
		
		/** Device Open */
		public bool OpenPrinter()
		{
			try
			{
				//setTimeout(500, 5000);
				setTimeout(500, 15000);
                if (SerialOpen())
                {
                    return checkPrinter();
                }
                else return false;
			}
			catch(Exception e)
			{
				return false;
			}
		}

		/** Device Close */
		public bool ClosePrinter()
		{
			return SerialClose();
		}
        public void printLogo(Bitmap bitmap, int width = 512, int threshold = 127)
        {
            byte[] img = new PrintBitmap().GetLogo(bitmap, width, threshold);

            //File.WriteAllBytes("c:/temp/rralogo.bytes", img);

            addPrintBuff(img);
        }

        public void printInit()
		{
			addPrintBuff( PRN.INIT );	
		}
		
		public void printText(string text)
		{
			
			byte[] bytes = Encoding.Default.GetBytes(text);
            // 1B 21 00
            addPrintBuff(PRN.PRINT_ALIGN_LEFT);         // PRINT_ALIGN_LEFT
            addPrintBuff( PRN.PRINT_NORMAL );			// PRINT_NORMAL
			addPrintBuff( bytes );
		}

		public void printWide(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 21 20
			addPrintBuff( PRN.PRINT_2_WIDTH );				// PRINT_EXPEND_WIDTH
			addPrintBuff( bytes );
			addPrintBuff( PRN.PRINT_NORMAL );				// PRINT_NORMAL
		}

		public void printHide(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 21 10
			addPrintBuff( PRN.PRINT_2_HEIGHT );				// PRINT_EXPEND_HEIGHT
			addPrintBuff( bytes );
			addPrintBuff( PRN.PRINT_NORMAL );				// PRINT_NORMAL
		}

		public void printWideHide(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 21 30
			addPrintBuff( PRN.PRINT_2_BOTH );				// PRINT_EXPEND_BOTH
			addPrintBuff( bytes );
			addPrintBuff( PRN.PRINT_NORMAL );				// PRINT_NORMAL
		}

		public void printBold(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 47 01
			addPrintBuff( PRN.PRINT_BOLD_YES );					// PRINT_BOLD
			addPrintBuff( bytes );
			// 1B 47 00
			addPrintBuff( PRN.PRINT_BOLD_NO );					// PRINT_BOLD
		}

		public void printInvert(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 7B 01
			addPrintBuff( PRN.PRINT_REVERSE_YES );				// PRINT_REVERSE
			addPrintBuff( bytes );
			// 1B 7B 00
			addPrintBuff( PRN.PRINT_REVERSE_NO );				// PRINT_REVERSE
		}

		public void printUnderline(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 2D 01
			addPrintBuff( PRN.PRINT_UNDERLINE_YES );				// PRINT_UNDERLINE
			addPrintBuff( bytes );
			// 1B 2D 00
			addPrintBuff( PRN.PRINT_UNDERLINE_NO );				// PRINT_UNDERLINE
		}
		public void SetLogo(string vFileName)
		{
		}
		
		public void printLogo()
		{
			addPrintBuff( PRN.PRINT_BITMAP );
		}

		public void printBarcode(int alignment,string barcode)
		{
			// barcode type : 5(ITF)
			printBarcode(5, alignment, barcode);
		}
		public void printBarcode(string barcode)
		{
			strBarCode = barcode;

            //+JSJ(20180307)
            if (!strBarCode.Equals(string.Empty))
            {
                if (strBarCode.Length == 8)
                {
                    printEAN8(1, strBarCode);
                }
                else if (strBarCode.Length == 13)
                {
                    printEAN13(1, strBarCode);
                }
                else
                {
                    printBarcode(1, strBarCode);
                }
            }
            //-JSJ(20180307)
        }

        public void printCenter(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 61 01
			addPrintBuff( PRN.PRINT_ALIGN_CENTER );			// PRINT_ALIGN_CENTER
			addPrintBuff( bytes );
			// 1B 61 00
			addPrintBuff( PRN.PRINT_ALIGN_LEFT );			// PRINT_ALIGN_LEFT
		}

        public void printWideCenter(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 61 01
			addPrintBuff( PRN.PRINT_ALIGN_CENTER );			// PRINT_ALIGN_CENTER
			addPrintBuff( PRN.PRINT_2_WIDTH );				// PRINT_EXPEND_WIDTH
			addPrintBuff( bytes );
			addPrintBuff( PRN.PRINT_NORMAL );				// PRINT_NORMAL
			// 1B 61 00
			addPrintBuff( PRN.PRINT_ALIGN_LEFT );			// PRINT_ALIGN_LEFT
		}

		public void printWideRight(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 61 01
			addPrintBuff( PRN.PRINT_ALIGN_RIGHT );			// PRINT_ALIGN_CENTER
			addPrintBuff( PRN.PRINT_2_WIDTH );				// PRINT_EXPEND_WIDTH
			addPrintBuff( bytes );
			addPrintBuff( PRN.PRINT_NORMAL );				// PRINT_NORMAL
			// 1B 61 00
			addPrintBuff( PRN.PRINT_ALIGN_LEFT );			// PRINT_ALIGN_LEFT
		}

		public void printHideCenter(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 61 01
			addPrintBuff( PRN.PRINT_ALIGN_CENTER );			// PRINT_ALIGN_CENTER
			addPrintBuff( PRN.PRINT_2_HEIGHT );				// PRINT_EXPEND_WIDTH
			addPrintBuff( bytes );
			addPrintBuff( PRN.PRINT_NORMAL );				// PRINT_NORMAL
			// 1B 61 00
			addPrintBuff( PRN.PRINT_ALIGN_LEFT );			// PRINT_ALIGN_LEFT
		}

        public void printBoldHideCenter(string text)
        {
            byte[] bytes = Encoding.Default.GetBytes(text);
            // 1B 61 01
            addPrintBuff(PRN.PRINT_BOLD_NO);				// PRINT_BOLD_NO
            addPrintBuff(PRN.PRINT_ALIGN_CENTER);			// PRINT_ALIGN_CENTER
            addPrintBuff(PRN.PRINT_2_HEIGHT);				// PRINT_EXPEND_WIDTH
            addPrintBuff(bytes);
            addPrintBuff(PRN.PRINT_NORMAL);				// PRINT_BOLD_NO
            // 1B 61 00
            addPrintBuff(PRN.PRINT_ALIGN_LEFT);			// PRINT_ALIGN_LEFT
        }

		public void printHideRight(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 61 01
			addPrintBuff( PRN.PRINT_ALIGN_RIGHT );			// PRINT_ALIGN_CENTER
			addPrintBuff( PRN.PRINT_2_HEIGHT );				// PRINT_EXPEND_WIDTH
			addPrintBuff( bytes );
			addPrintBuff( PRN.PRINT_NORMAL );				// PRINT_NORMAL
			// 1B 61 00
			addPrintBuff( PRN.PRINT_ALIGN_LEFT );			// PRINT_ALIGN_LEFT
		}

		public void printWideHideCenter(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 61 01
			addPrintBuff( PRN.PRINT_ALIGN_CENTER );			// PRINT_ALIGN_CENTER
			// 1B 21 30
			addPrintBuff( PRN.PRINT_2_BOTH );				// PRINT_EXPEND_BOTH
			addPrintBuff( bytes );
			addPrintBuff( PRN.PRINT_NORMAL );				// PRINT_NORMAL
			// 1B 61 00
			addPrintBuff( PRN.PRINT_ALIGN_LEFT );			// PRINT_ALIGN_LEFT
		}

		public void printWideHideRight(string text)
		{
			byte[] bytes = Encoding.Default.GetBytes(text);
			// 1B 61 01
			addPrintBuff( PRN.PRINT_ALIGN_RIGHT );			// PRINT_ALIGN_CENTER
			// 1B 21 30
			addPrintBuff( PRN.PRINT_2_BOTH );				// PRINT_EXPEND_BOTH
			addPrintBuff( bytes );
			addPrintBuff( PRN.PRINT_NORMAL );				// PRINT_NORMAL
			// 1B 61 00
			addPrintBuff( PRN.PRINT_ALIGN_LEFT );			// PRINT_ALIGN_LEFT
		}

		public void ClearOutput()
		{
			clearOutBuffer();
		}

	private void printBarcode(int kind, int alignment, string barcode)
		{
			
			try
			{
				MemoryStream tempBuff = new MemoryStream();
				byte[] bytes = Encoding.Default.GetBytes(barcode);
				// 1B 61 01 1D 68 32 1D 77 02 1D 48 02 1D 66 01 1D 6B 05
				// 1B 61 01 1D 6B 05
				tempBuff.Write( PRN.PRINT_BARCODE_BEGIN, 0, PRN.PRINT_BARCODE_BEGIN.Length );
				tempBuff.Write( bytes, 0, bytes.Length );
				// 00 1B 61 00
				tempBuff.Write( PRN.PRINT_BARCODE_END, 0, PRN.PRINT_BARCODE_END.Length );

                //Beans.util.Log.SendLog("printer", 0, tempBuff.ToArray());
                //SerialWrite(tempBuff.ToArray());
                
                //+JSJ(20180307)
                //AsyncSerialWrite(tempBuff.ToArray());
                addPrintBuff(tempBuff.ToArray());
                //-JSJ(20180307)

                tempBuff.Dispose();
			}
			catch(Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
			}
		}

		private void printEAN8(int alignment, string barcode)
		{
			byte align = ( alignment >=0 && alignment <= 2 ) ? (byte)alignment: (byte)0x01;
			try
			{
				MemoryStream tempBuff = new MemoryStream();
				byte[] bytes = Encoding.Default.GetBytes(barcode);
				
				tempBuff.Write(new byte[] {0x1B,0x61,align},0,3);
				tempBuff.Write(new byte[] {0x1D,0x68,0x20},0,3);
				tempBuff.Write(new byte[] {0x1D,0x66,0x01},0,3);
				tempBuff.Write(new byte[] {0x1D,0x6B,0x03},0,3);
				tempBuff.Write( bytes, 0, bytes.Length );
				tempBuff.WriteByte(0x00);
				tempBuff.Write(new byte[] {0x1B,0x61,0x00},0,3);

                //SerialWrite(tempBuff.ToArray());

                //+JSJ(20180307)
                //AsyncSerialWrite(tempBuff.ToArray());
                addPrintBuff(tempBuff.ToArray());
                //-JSJ(20180307)

                tempBuff.Dispose();
			}
			catch(Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
			}
		}

		private void printEAN13(int alignment, string barcode)
		{
			byte align = ( alignment >=0 && alignment <= 2 ) ? (byte)alignment: (byte)0x01;
			try
			{
				MemoryStream tempBuff = new MemoryStream();
				byte[] bytes = Encoding.Default.GetBytes(barcode);
				
				tempBuff.Write(new byte[] {0x1B,0x61,(byte)align},0,3);
				tempBuff.Write(new byte[] {0x1D,0x68,0x20},0,3); 
				tempBuff.Write(new byte[] {0x1D,0x66,0x01},0,3);
				tempBuff.Write(new byte[] {0x1D,0x6B,0x02},0,3);
				tempBuff.Write( bytes, 0, bytes.Length );
				tempBuff.WriteByte(0x00);
				tempBuff.Write(new byte[] {0x1B,0x61,0x00},0,3);

                //SerialWrite(tempBuff.ToArray());

                //+JSJ(20180307)
                //AsyncSerialWrite(tempBuff.ToArray());
                addPrintBuff(tempBuff.ToArray());
                //-JSJ(20180307)

                tempBuff.Dispose();
			}
			catch(Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
			}
		}

        public bool print(bool flag)
        {
            return true;
        }

		public bool print()
		{
			bool ret = false;

			while( true )
			{
                try
				{
					checkState();
					
					// buffer print
					//SerialWrite( printBuff.ToArray() );
					AsyncSerialWrite( printBuff.ToArray() );

                    

                    ret = true;
					break;
				}
				catch( Exception e )
				{
                    /*
                    MessageBox messageBox = new MessageBox();
                    messageBox.ShowMessage("message here", e.Message, true);
                    (messageBox as Form).ShowDialog();
                    if (! messageBox.Step) break;
                    messageBox.Dispose();
                    */
				}
			}
			
			strBarCode = string.Empty;
			printBuff.Dispose();
			printBuff = new MemoryStream();

			return ret;
		}

		

        public void cutPaper(bool flag)
        {
        }

        public void cutPaper()
        {
            cutPaper("0");
        }

		public void cutPaper(string space)
		{
			int line = 0;
			string strSpace = string.Empty;
			try{ line = Convert.ToInt32(space);	}
			catch{	}
			
			for(int i=0; i<line; i++)
			{
				strSpace += "0D0A";
			}
			
			while( true )
			{
				try
				{
					checkState();
					
					if(strSpace.Length != 0)
					{
						//SerialWrite( Beans.util.Convert.convHexStr2Bytes(strSpace));
						AsyncSerialWrite(convHexStr2Bytes(strSpace));
					}

					// 0D 0A 0D 0A 0D 0A 0D 0A 1D 56 01
					//SerialWrite( PRN.CUT_PAPER );
					AsyncSerialWrite( PRN.CUT_PAPER );
					break;
				}
				catch(Exception e)
				{
                    /*
                    MessageBox messageBox = new MessageBox();
                    messageBox.ShowMessage("Message Here", e.Message, true);
                    (messageBox as Form).ShowDialog();
                    if (!messageBox.Step) break;
                    messageBox.Dispose();
                    */
				}
			}
		}

		private void checkState()
        {
            if (!Com.IsOpen)
            {
                throw new Exception("프린터가 정상 상태가 아닙니다.");
            }
            //if (!Com.CDHolding)
            //{
            //    throw new Exception("프린터가 정상 상태가 아닙니다.");
            //}
            if (!Com.CtsHolding)
            {
                throw new Exception("프린터가 정상 상태가 아닙니다.");
            }
            if (!Com.DsrHolding)
            {
                throw new Exception("프린터가 정상 상태가 아닙니다.");
            }
//			포스뱅크 프린터 상태체크 커맨드가 달라서 일시적
//			}
//			if( bytes == null )
//			{
//				throw new Exception("프린터를 확인하세요.");으로 체크 안함.
//			추후 옵션화 할것.
//			byte[] sendByte = new byte[] {0x10,0x04,0x02};
//			byte[] bytes = null;
//			try
//			{
//				SerialWrite(sendByte);
//				bytes = SerialRead(1, 1000);
//			} catch (Exception e) {
//				Beans.util.Log.ErrorLog(e.Message);
//				throw new Exception("프린터를 확인하세요.");
//			}
//			if((bytes[0] & 0x20) == 0x20 && (bytes[0] & 0x04) == 0x04)
//			{
//				throw new Exception("프린터가 정상 상태가 아닙니다.");
//			}
//			else if((bytes[0] & 0x20) == 0x20)
//			{
//				throw new Exception("프린터의 용지를 확인 하십시오.");
//			}
//			else if((bytes[0] & 0x04) == 0x04)
//			{
//				throw new Exception("프린터 커버가 개방되었습니다.");
//			}
		}


        public bool checkPrinter()
        {
            //if (!Com.IsOpen) return false;
            if (!Com.CtsHolding)
            {
                Com.Close();
                return false;
            }
            if (!Com.DsrHolding)
            {
                Com.Close();
                return false;
            }

            return true;
        }

		public bool OpenDrawer()
		{
			try
			{
				checkState();
				
				// 1B 70 30 80 80
                //SerialWrite( PRN.OPEN_DRAWER );
                AsyncSerialWrite(PRN.OPEN_DRAWER);
                return true;
			}
			catch(Exception e)
			{
                /*
                MessageBox messageBox = new MessageBox();
                messageBox.ShowMessage("돈통열기 실패", e.Message, false);
                (messageBox as Form).ShowDialog();
                messageBox.Dispose();
                return false;
                */
                return false;
			}
		}

        private byte[] convHexStr2Bytes(string hexStr)
        {
            try
            {
                string str = hexStr.Replace(" ", string.Empty);

                if (str.Length % 2 != 0) return new byte[0];

                byte[] bytes = new byte[str.Length / 2];
                for (int i = 0; i < str.Length / 2; i++)
                {
                    bytes[i] = convHexStr2Byte(str.Substring(i * 2, 2));
                }

                return bytes;
            }
            catch
            {
                return new byte[0];
            }
        }

        // HEX String ----> byte[]
        public byte convHexStr2Byte(string hexStr)
        {
            if (hexStr == null || hexStr.Length != 2)
                return 0;

            return System.Convert.ToByte(hexStr, 16);
        }

	}
}
