using System;

namespace EBM2x.WPF.device
{
	/// <summary>
	/// Description of DevPrinterCashDrawer.
	/// </summary>
	public class DevPrinterCashDrawer
	{
		private string strDefaultPosName =string.Empty ; // OPOS Default Name 설정 변수

		public DevPrinterCashDrawer()
		{
		}

		public void SetDeviceName(string valName)
		{
			strDefaultPosName = valName;
		}
		public bool OpenCashDrawer()
		{
			return true;
		}

		public bool CloseCashDrawer()
		{
			return true;
		}

		public int OpenDrawer()
		{
            /*
			InputOutput.output.JournalManager journal = new InputOutput.output.JournalManager();
			bool bRet=journal.OpenDrawer();
            if (bRet) return 1; else return -1;
            */
            return 1;
		}
	}
}
