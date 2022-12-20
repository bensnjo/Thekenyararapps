using Android.Bluetooth;
using Android.Telephony;
using EBM2x.Database.MasterEbm2x;
using EBM2x.Dependency;
using EBM2x.Droid;
using EBM2x.Models.journal;
using EBM2x.Models.tran;
using EBM2x.RraSdc;
using EBM2x.UI;
using EBM2x.UI.Resource;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(PDABlueToothPrinter))]
namespace EBM2x.Droid
{
    public class PDABlueToothPrinter : IBlueToothService
    {
        string deviceName = "ER-80A";

        const string ESC = "\u001B";
        const string GS = "\u001D";
        const string LineFeed = "\n\n\n";
        const string InitializePrinter = ESC + "@";
        const string BoldOn = ESC + "E" + "\u0001";
        const string BoldOff = ESC + "E" + "\0";
        const string DoubleOn = GS + "!" + "\u0011";  // 2x sized text (double-high + double-wide)
        const string DoubleOff = GS + "!" + "\0";

        byte[] EAN8BarCodeStart = new byte[] { 0x1D, 0x6B, 0x44, 0x08 };
        byte[] EAN13BarCodeStart = new byte[] { 0x1D, 0x6B, 0x43, 0xd };
        byte[] CODE39BarCodeStart = new byte[] { 0x1D, 0x6B, 0x04, 0x43, 0x4f, 0x44, 0x45, 0x20, 0x33, 0x39, 0x00 };
        byte[] TYPE128BarCodeStart = new byte[] { 0x1D, 0x6B, 0x08, 0x7b, 0x42, 0x43, 0x6F, 0x64, 0x65, 0x20, 0x31, 0x32, 0x38, 0x00 };

        public bool IsAndoid()
        {
            return true;
        }
        public string GetSmsMessage(Models.journal.JournalModel journal)
        {
            string message = "";
            foreach (JournalString node in journal.JournalStringList)
            {
                if (node.Type.Equals("qrcode"))
                {
                    message = RraSdcService.RECEIPT_URL + "/common/link/ebm/receipt/indexEbmReceiptData?Data=" + node.Data;
                    break;
                }
            }
            return message;
        }

        public bool SendSMS(string phoneNumber, string message)
        {
            // Get the SMS manager
            SmsManager smsManager = SmsManager.Default;
            // Split text message content (SMS length limit)
            IList<string> divideContents = smsManager.DivideMessage(message);

            foreach (string text in divideContents)
            {
                smsManager.SendTextMessage(phoneNumber, null, text, null, null);
            }

            return true;
        }

        /// <summary>
        /// We have to use local device Bluetooth adapter.
        /// BondedDevices returns BluetoothDevice collection anyway I need to take just device name.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetDeviceList()
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                var btdevice = bluetoothAdapter?.BondedDevices.Select(i => i.Name).ToList();
                return btdevice;
            }
        }
        public string GetDeviceName()
        {
            return deviceName;
        }

        public void PrintJournal(string port, int baudRate, Models.journal.JournalModel journal, bool isReprint)
        {
            // 2021.03.17
            string SPACE = "      ";
            // 영수증 사이즈를 58mm로 설정
            if (UIManager.Instance().Is58mmPrinter) SPACE = "";

            if (!string.IsNullOrEmpty(port)) deviceName = port;

            // 프린터가 지정되어있지 않으면...
            if (string.IsNullOrEmpty(deviceName)) return;

            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                                          where bd?.Name == deviceName
                                          select bd).FirstOrDefault();
                
                if (device == null) return;

                try
                {
                    UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                    using (BluetoothSocket bluetoothSocket = device?.CreateRfcommSocketToServiceRecord(uuid))
                    {
                        try
                        {
                            bluetoothSocket?.Connect();
                        }
                        catch (Exception ex)
                        {
                            return;
                        }

                        string qrData = "";
                        byte[] modelCenter = { 0x1B, 0x61, 0x01, 0x1D, 0x48, 0x02 };
                        bluetoothSocket?.OutputStream.Write(modelCenter, 0, modelCenter.Length);

                        if (UIManager.Instance().PosModel.Environment.EnvPosSetup.PrintingLogo == "Y")
                        {
                            // logo 출력
                            using (var stream = ResourceUtil.GetImageStream("rralogo.bytes"))
                            {
                                byte[] img = ResourceUtil.ReadFully(stream);
                                bluetoothSocket?.OutputStream.Write(img, 0, img.Length);
                            }
                        }

                        byte[] modelLeft = { (byte)0x1b, (byte)0x61, (byte)0x00 };
                        bluetoothSocket?.OutputStream.Write(modelLeft, 0, modelLeft.Length);

                        foreach (JournalString node in journal.JournalStringList)
                        {
                            if (node.Type.Equals("reprint") && !isReprint)
                            {
                                continue;
                            }
                            else if (node.Type.Equals("reprint") && isReprint)
                            {
                                byte[] buffer1 = Encoding.UTF8.GetBytes(SPACE + "        Copy" + "\n");
                                bluetoothSocket?.OutputStream.Write(buffer1, 0, buffer1.Length);
                                byte[] buffer2 = Encoding.UTF8.GetBytes(SPACE + "--------------------------------" + "\n");
                                bluetoothSocket?.OutputStream.Write(buffer2, 0, buffer2.Length);
                                continue;
                            }

                            if (isReprint && !string.IsNullOrEmpty(node.Data) && node.Data.Contains("RECEIPT NUMBER :"))
                            {
                                node.Data = node.Data.Replace("NS", "CS");
                                node.Data = node.Data.Replace("NR", "CR");
                            }
                            
                            if (node.Type.Equals("qrcode"))
                            {
                                qrData = node.Data;
                            }
                            else
                            {
                                byte[] buffer = Encoding.UTF8.GetBytes(SPACE + node.Data + "\n");
                                bluetoothSocket?.OutputStream.Write(buffer, 0, buffer.Length);
                            }
                            //@ychan_20191209 Pda 출력 sleep 추가
                            System.Threading.Thread.Sleep(10);
                        }

                        if (!string.IsNullOrEmpty(qrData))
                        {
                            string qrdata = RraSdcService.RECEIPT_URL + "/common/link/ebm/receipt/indexEbmReceiptData?Data=" + qrData;

                            int store_len = qrdata.Length + 3;
                            byte store_pL = (byte)(store_len % 256);
                            byte store_pH = (byte)(store_len / 256);


                            byte[] BarCodeInit = new byte[] { 0x1B, 0x61, 0x01, 0x1D, 0x48, 0x02 };

                            // QR Code: Select the model
                            //              Hex     1D      28      6B      04      00      31      41      n1(x32)     n2(x00) - size of model
                            // set n1 [49 x31, model 1] [50 x32, model 2] [51 x33, micro qr code]
                            byte[] modelQR = { (byte)0x1d, (byte)0x28, (byte)0x6b, (byte)0x04, (byte)0x00, (byte)0x31, (byte)0x41, (byte)0x32, (byte)0x00 };
                            // QR Code: Set the size of module
                            // Hex      1D      28      6B      03      00      31      43      n
                            // n depends on the printer
                            byte[] sizeQR = { (byte)0x1d, (byte)0x28, (byte)0x6b, (byte)0x03, (byte)0x00, (byte)0x31, (byte)0x43, (byte)0x09 };
                            //          Hex     1D      28      6B      03      00      31      45      n
                            // Set n for error correction [48 x30 -> 7%] [49 x31-> 15%] [50 x32 -> 25%] [51 x33 -> 30%]
                            byte[] errorQR = { (byte)0x1d, (byte)0x28, (byte)0x6b, (byte)0x03, (byte)0x00, (byte)0x31, (byte)0x45, (byte)0x31 };
                            // QR Code: Store the data in the symbol storage area
                            // Hex      1D      28      6B      pL      pH      31      50      30      d1...dk
                            //                        1D          28          6B         pL          pH  cn(49->x31) fn(80->x50) m(48->x30) d1��dk
                            byte[] storeQR = { (byte)0x1d, (byte)0x28, (byte)0x6b, store_pL, store_pH, (byte)0x31, (byte)0x50, (byte)0x30 };
                            // QR Code: Print the symbol data in the symbol storage area
                            // Hex      1D      28      6B      03      00      31      51      m
                            byte[] printQR = { (byte)0x1d, (byte)0x28, (byte)0x6b, (byte)0x03, (byte)0x00, (byte)0x31, (byte)0x51, (byte)0x30 };

                            bluetoothSocket?.OutputStream.Write(BarCodeInit, 0, BarCodeInit.Length);

                            // write() simply appends the data to the buffer
                            bluetoothSocket?.OutputStream.Write(modelQR, 0, modelQR.Length);
                            bluetoothSocket?.OutputStream.Write(sizeQR, 0, sizeQR.Length);
                            bluetoothSocket?.OutputStream.Write(errorQR, 0, errorQR.Length);
                            bluetoothSocket?.OutputStream.Write(storeQR, 0, storeQR.Length);

                            byte[] buffer = Encoding.UTF8.GetBytes(qrdata);
                            bluetoothSocket?.OutputStream.Write(buffer, 0, buffer.Length);
                            bluetoothSocket?.OutputStream.Write(printQR, 0, printQR.Length);

                            byte[] bufferFeed = Encoding.UTF8.GetBytes(LineFeed);
                            bluetoothSocket?.OutputStream.Write(bufferFeed, 0, bufferFeed.Length);

                            //@ychan_20191209 Pda 출력 sleep 추가
                            System.Threading.Thread.Sleep(1000);
                        }

                        bluetoothSocket.Close();
                    }
                }
                catch (Exception exp)
                {
                    return;
                }
            }
        }
        public void PrintJournalA4(Models.journal.JournalModel journal, bool isReprint)
        {
        }

        public void PrintInvoiceA4(TransactionSalesModel salesModel,
            Models.journal.JournalModel journal1,
            Models.journal.JournalModel journal2,
            Models.journal.JournalModel journal3,
            Models.journal.JournalModel journal4, bool isReprint, Models.journal.JournalModel journal5)
        {
        }
    }
}