using EBM2x.WPF;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(TimeSave))]
namespace EBM2x.WPF
{
    public class TimeSave : ITimeSave
    {
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }
        //[DllImport("kernel32.dll", EntryPoint = "GetSystemTime", SetLastError = true)]
        //public static extern void Win32GetSystemTime(ref SYSTEMTIME time);
        //[DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        //public static extern bool Win32SetSystemTime(ref SYSTEMTIME time);
        [DllImport("kernel32.dll", EntryPoint = "GetLocalTime", SetLastError = true)]
        public static extern void Win32GetLocalTime(ref SYSTEMTIME time);
        [DllImport("kernel32.dll", EntryPoint = "SetLocalTime", SetLastError = true)]
        public static extern bool Win32SetLocalTime(ref SYSTEMTIME time);

        public async Task SetSystemDateTime(string yyyyMMddHHmmssmmm)
        {
            string posTime = yyyyMMddHHmmssmmm.Substring(0, 4) + "/" + yyyyMMddHHmmssmmm.Substring(4, 2) + "/" + yyyyMMddHHmmssmmm.Substring(6, 2) + " " +
                             yyyyMMddHHmmssmmm.Substring(8, 2) + ":" + yyyyMMddHHmmssmmm.Substring(10, 2) + ":" + yyyyMMddHHmmssmmm.Substring(12, 2);

            if (SetSystemDateTime(System.Convert.ToDateTime(posTime)))
            {
            }
            else
            {
            }
        }

        public bool SetSystemDateTime(DateTime dtNew)
        {
            try
            {
                bool bRtv = true;

                if (dtNew != DateTime.MinValue)
                {
                    SYSTEMTIME st = new SYSTEMTIME();
                    Win32GetLocalTime(ref st);

                    string sysDate = st.wYear.ToString() + st.wMonth.ToString().PadLeft(2, '0') + st.wDay.ToString().PadLeft(2, '0');
                    string svrDate = dtNew.ToString("yyyyMMdd");

                    if (sysDate != svrDate)
                    {
                    }

                    st.wYear = (ushort)dtNew.Year;
                    st.wMonth = (ushort)dtNew.Month;
                    st.wDay = (ushort)dtNew.Day;

                    st.wHour = (ushort)dtNew.Hour;
                    st.wMinute = (ushort)dtNew.Minute;
                    st.wSecond = (ushort)dtNew.Second;
                    st.wMilliseconds = (ushort)dtNew.Millisecond;
                    bRtv = Win32SetLocalTime(ref st);  // UTC + 표준시간대(대한민국의 경우 UTC+0)를 설정한다.

                    if (bRtv == false)
                    {
                        int lastError = Marshal.GetLastWin32Error();
                    }

                    bRtv = true;//서버시간설정

                    string nowDateTime = st.wYear.ToString() + "/" + st.wMonth.ToString() + "/" + st.wDay.ToString() + " " + st.wHour.ToString() + ":" + st.wMinute.ToString() + ":" + st.wSecond.ToString();
                }
                return bRtv;
            }
            catch
            {
                return false;
            }
        }
    }
}
