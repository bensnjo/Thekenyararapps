using System;

namespace EBM2x.WPF.device
{
	/// <summary>
	/// Description of PrinterRecord.
	/// </summary>
	public class PrinterRecord
	{
		//----------------------------------------------------------------------------
		// Variables declaration
		//----------------------------------------------------------------------------

		private string fieldInit              = string.Empty; // 프린터 초기값
		private string fieldPrintBitmap       = string.Empty; // 로고 이미지
		
		private string fieldPrintbarcodeBegin = string.Empty; // 바코드 시작
		private string fieldPrintbarcodeEnd   = string.Empty; // 바코드 종료
		
		private string fieldPrintNormal		  = string.Empty; // 정상		
		private string fieldPrintExpandWidth  = string.Empty; // 가로 확대
		private string fieldPrintNormalWidth  = string.Empty; // 가로 축소
		private string fieldPrintExpandHeight = string.Empty; // 세로 확대
		private string fieldPrintNormalHeight = string.Empty; // 세로 축소
		private string fieldPrintExpandBoth   = string.Empty; // 가로세로 확대
		private string fieldPrintNormalBoth   = string.Empty; // 가로세로 축소
		private string fieldPrintBoldYes      = string.Empty;	// BOLD YES
		private string fieldPrintBoldNo       = string.Empty;	// BOLD NO
		private string fieldPrintReverseYes   = string.Empty;	// REVERSE YES
		private string fieldPrintReverseNo    = string.Empty;	// REVERSE NO
		private string fieldPrintUnderlineYes = string.Empty; // UNDERLINE YES
		private string fieldPrintUnderlineNo  = string.Empty; // UNDERLINE NO
		private string fieldPrintLeft		  = string.Empty; // LEFT ALIGNMENT
		private string fieldPrintCenter       = string.Empty; // CENTER ALIGNMENT
		private string fieldPrintRight        = string.Empty; // RIGHT ALIGNMENT
		
		private string fieldCutPaper          = string.Empty; // 용지 자르기
		private string fieldOpenDrawer        = string.Empty; // 연결된 돈통 열기

		public PrinterRecord()
		{
			clear();
		}
		
		//----------------------------------------------------------------------------
		// Get/Set Properties
		//----------------------------------------------------------------------------

		public byte[] INIT {
			get { return convHexStr2Bytes(fieldInit); }
		}		
		public string getInit()
		{
			return fieldInit;
		}
		public void setInit(string value)
		{
			fieldInit = value;
		}
		
		public byte[] PRINT_BITMAP {
			get { return convHexStr2Bytes(fieldPrintBitmap); }
		}
		public string getPrintBitmap()
		{
			return fieldPrintBitmap;
		}
		public void setPrintBitmap(string value)
		{
			fieldPrintBitmap = value;
		}

		public byte[] PRINT_BARCODE_BEGIN {
			get { return convHexStr2Bytes(fieldPrintbarcodeBegin); }
		}
		public string getPrintBarcodeBegin()
		{
			return fieldPrintbarcodeBegin;
		}
		public void setPrintBarcodeBegin(string value)
		{
			fieldPrintbarcodeBegin = value;
		}
		
		public byte[] PRINT_BARCODE_END {
			get { return convHexStr2Bytes(fieldPrintbarcodeEnd); }
		}
		public string getPrintBarcodeEnd()
		{
			return fieldPrintbarcodeEnd;
		}
		public void setPrintBarcodeEnd(string value)
		{
			fieldPrintbarcodeEnd = value;
		}
		
		public byte[] PRINT_NORMAL {
			get { return convHexStr2Bytes(fieldPrintNormal); }
		}
		public string getPrintNormal()
		{
			return fieldPrintNormal;
		}
		public void setPrintNormal(string value)
		{
			fieldPrintNormal = value; 
		}
		
		public byte[] PRINT_2_WIDTH {
			get { return convHexStr2Bytes(fieldPrintExpandWidth); }
		}
		public string getPrintExpandWidth()
		{
			return fieldPrintExpandWidth;
		}
		public void setPrintExpandWidth(string value)
		{
			fieldPrintExpandWidth = value; 
		}
		
		public byte[] PRINT_1_WIDTH {
			get { return convHexStr2Bytes(fieldPrintNormalWidth); }
		}
		public string getPrintNormalWidth()
		{
			return fieldPrintNormalWidth;
		}
		public void setPrintNormalWidth(string value)
		{
			fieldPrintNormalWidth = value; 
		}
		
		public byte[] PRINT_2_HEIGHT {
			get { return convHexStr2Bytes(fieldPrintExpandHeight); }
		}
		public string getPrintExpandHeight()
		{
			return fieldPrintExpandHeight;
		}
		public void setPrintExpandHeight(string value)
		{
			fieldPrintExpandHeight = value; 
		}
		
		public byte[] PRINT_1_HEIGHT {
			get { return convHexStr2Bytes(fieldPrintNormalHeight); }
		}
		public string getPrintNormalHeight()
		{
			return fieldPrintNormalHeight;
		}
		public void setPrintNormalHeight(string value)
		{
			fieldPrintNormalHeight = value; 
		}
		
		public byte[] PRINT_2_BOTH {
			get { return convHexStr2Bytes(fieldPrintExpandBoth); }
		}
		public string getPrintExpandBoth()
		{
			return fieldPrintExpandBoth;
		}
		public void setPrintExpandBoth(string value)
		{
			fieldPrintExpandBoth = value; 
		}
		
		public byte[] PRINT_1_BOTH {
			get { return convHexStr2Bytes(fieldPrintNormalBoth); }
		}
		public string getPrintNormalBoth()
		{
			return fieldPrintNormalBoth;
		}
		public void setPrintNormalBoth(string value)
		{
			fieldPrintNormalBoth = value; 
		}

		public byte[] PRINT_BOLD_YES {
			get { return convHexStr2Bytes(fieldPrintBoldYes); }
		}
		public string getPrintBoldYes()
		{
			return fieldPrintBoldYes;
		}
		public void setPrintBoldYes(string value)
		{
			fieldPrintBoldYes = value; 
		}
		public byte[] PRINT_BOLD_NO {
			get { return convHexStr2Bytes(fieldPrintBoldNo); }
		}
		public string getPrintBoldNo()
		{
			return fieldPrintBoldNo;
		}
		public void setPrintBoldNo(string value)
		{
			fieldPrintBoldNo = value; 
		}


		public byte[] PRINT_REVERSE_YES {
			get { return convHexStr2Bytes(fieldPrintReverseYes); }
		}
		public string getPrintReverseYes()
		{
			return fieldPrintReverseYes;
		}
		public void setPrintReverseYes(string value)
		{
			fieldPrintReverseYes = value; 
		}
		public byte[] PRINT_REVERSE_NO {
			get { return convHexStr2Bytes(fieldPrintReverseNo); }
		}
		public string getPrintReverseNo()
		{
			return fieldPrintReverseNo;
		}
		public void setPrintReverseNo(string value)
		{
			fieldPrintReverseNo = value; 
		}

		public byte[] PRINT_UNDERLINE_YES {
			get { return convHexStr2Bytes(fieldPrintUnderlineYes); }
		}
		public string getPrintUnderlineYes()
		{
			return fieldPrintUnderlineYes;
		}
		public void setPrintUnderlineYes(string value)
		{
			fieldPrintUnderlineYes = value; 
		}
		public byte[] PRINT_UNDERLINE_NO {
			get { return convHexStr2Bytes(fieldPrintUnderlineNo); }
		}
		public string getPrintUnderlineNo()
		{
			return fieldPrintUnderlineNo;
		}
		public void setPrintUnderlineNo(string value)
		{
			fieldPrintUnderlineNo = value; 
		}

		public byte[] PRINT_ALIGN_LEFT {
			get { return convHexStr2Bytes(fieldPrintLeft); }
		}
		public string getPrintLeft()
		{
			return fieldPrintLeft;
		}
		public void setPrintLeft(string value)
		{
			fieldPrintLeft = value; 
		}

		public byte[] PRINT_ALIGN_CENTER {
			get { return convHexStr2Bytes(fieldPrintCenter); }
		}
		public string getPrintCenter()
		{
			return fieldPrintCenter;
		}
		public void setPrintCenter(string value)
		{
			fieldPrintCenter = value; 
		}

		public byte[] PRINT_ALIGN_RIGHT {
			get { return convHexStr2Bytes(fieldPrintRight); }
		}
		public string getPrintRight()
		{
			return fieldPrintRight;
		}
		public void setPrintRight(string value)
		{
			fieldPrintRight = value; 
		}

		public byte[] CUT_PAPER {
			get { return convHexStr2Bytes(fieldCutPaper); }
		}
		public string getCutPaper()
		{
			return fieldCutPaper;
		}
		public void setCutPaper(string value)
		{
			fieldCutPaper = value; 
		}

		public byte[] OPEN_DRAWER {
			get { return convHexStr2Bytes(fieldOpenDrawer); }
		}
		public string getOpenDrawer()
		{
			return fieldOpenDrawer;
		}
		public void setOpenDrawer(string value)
		{
			fieldOpenDrawer = value; 
		}
		
		//----------------------------------------------------------------------------
		// I/O 
		//----------------------------------------------------------------------------

		public void clear()
		{
			setInit             ( "1B 40" );                                                
			//setPrintBitmap      ( string.Empty );
            setPrintBitmap("1C 70 01 30");
            setPrintBarcodeBegin("1B 61 01 1D 68 32 1D 77 02 1D 48 02 1D 66 01 1D 6B 05");
			setPrintBarcodeEnd  ( "00 1B 61 00" );
			setPrintNormal      ( "1B 21 00" );
			setPrintExpandWidth ( "1B 21 20" ); 
			setPrintNormalWidth ( "1B 21 00" ); 
			setPrintExpandHeight( "1B 21 10" ); 
			setPrintNormalHeight( "1B 21 00" ); 
			setPrintExpandBoth  ( "1B 21 30" ); 
			setPrintNormalBoth  ( "1B 21 00" );
			setPrintBoldYes     ( "1B 47 01" );
			setPrintBoldNo      ( "1B 47 00" );
			setPrintReverseYes  ( "1B 7B 01" );
			setPrintReverseNo   ( "1B 7B 00" );
			setPrintUnderlineYes( "1B 2D 01" );
			setPrintUnderlineNo ( "1B 2D 00" );
			setPrintLeft        ( "1B 61 00" );
			setPrintCenter      ( "1B 61 01" );
			setPrintRight       ( "1B 61 02" );
			setCutPaper         ( "0D 0A 0D 0A 0D 0A 0D 0A 1D 56 01" );
			setOpenDrawer       ( "1B 70 30 80 80" );
		}
		
        /*
		public void toRecord(string valModel) 
		{
			DataFile.env.ini.PosPrinterINI ini = DataFile.env.ini.PosPrinterINI.Instance;
			ini.toRecord(valModel, this);
		}
        */

		public void toRecord( PrinterRecord record )
		{
			setInit             ( record.getInit             () );
			setPrintBitmap      ( record.getPrintBitmap      () );
			setPrintBarcodeBegin( record.getPrintBarcodeBegin() );
			setPrintBarcodeEnd  ( record.getPrintBarcodeEnd  () );
			setPrintNormal      ( record.getPrintNormal      () );
			setPrintExpandWidth ( record.getPrintExpandWidth () );
			setPrintNormalWidth ( record.getPrintNormalWidth () );
			setPrintExpandHeight( record.getPrintExpandHeight() );
			setPrintNormalHeight( record.getPrintNormalHeight() );
			setPrintExpandBoth  ( record.getPrintExpandBoth  () );
			setPrintNormalBoth  ( record.getPrintNormalBoth  () );
			setPrintBoldYes     ( record.getPrintBoldYes     () );
			setPrintBoldNo      ( record.getPrintBoldNo      () );
			setPrintReverseYes  ( record.getPrintReverseYes  () );
			setPrintReverseNo   ( record.getPrintReverseNo   () );
			setPrintUnderlineYes( record.getPrintUnderlineYes() );
			setPrintUnderlineNo ( record.getPrintUnderlineNo () );
			setPrintLeft        ( record.getPrintLeft        () );
			setPrintCenter      ( record.getPrintCenter      () );
			setPrintRight       ( record.getPrintRight       () );
			setCutPaper         ( record.getCutPaper         () );
			setOpenDrawer       ( record.getOpenDrawer       () );
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
