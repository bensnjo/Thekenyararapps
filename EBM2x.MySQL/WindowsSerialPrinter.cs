using EBM2x.Database.Master;
using EBM2x.Database.MasterEbm2x;
using EBM2x.Dependency;
using EBM2x.Models.journal;
using EBM2x.RraSdc;
using EBM2x.UI;
using EBM2x.UI.Resource;
using EBM2x.WPF;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows;
using ZXing;
using ZXing.Common;
using Brush = System.Drawing.Brush;
using Pen = System.Drawing.Pen;

[assembly: Xamarin.Forms.Dependency(typeof(WindowsSerialPrinter))]
namespace EBM2x.WPF
{
    public class WindowsSerialPrinter : IBlueToothService
    {
        private bool IsReprint;
        private Font printFont;
        private TransactionSalesModel salesModel;
        private Models.journal.JournalModel journal1;
        private Models.journal.JournalModel journal2;
        private Models.journal.JournalModel journal3;
        private Models.journal.JournalModel journal4;
        private Models.journal.JournalModel journal5;
        private int salesModelIndex = 0;
        private int pageCount = 0;

        private static device.DevSerialPrinter printer = null;

        public bool IsAndoid()
        {
            return false;
        }
        public string GetSmsMessage(Models.journal.JournalModel journal)
        {
            string message = "";
            foreach (JournalString node in journal.JournalStringList)
            {
                if (node.Type.Equals("qrcode"))
                {
                    message = RraSdcService.RECEIPT_URL + "/common/link/ebm/receipt/indexEbmReceipt?rcptNo=" + node.Data;
                    break;
                }
            }
            return message;
        }

        public bool SendSMS(string phoneNumber, string message)
        {
            return true;
        }

        /// <summary>
        /// We have to use local device Bluetooth adapter.
        /// BondedDevices returns BluetoothDevice collection anyway I need to take just device name.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetDeviceList()
        {
            return null;
        }
        public string GetDeviceName()
        {
            return "windows";
        }

        // JINIT_파리미터를 List에서 JournalModel로 변경,
        // public async Task PrintJournal(List<JournalString> JournalStringList, bool isBarcode, bool isReprint)
        public void PrintJournal(string port, int baudRate, Models.journal.JournalModel journal, bool isReprint)
        {
            this.IsReprint = isReprint;

            // 20201207
            // port = "USB";
            if (port.Equals("USB"))
            {
                PrintJournalUSBII(journal, isReprint);
                return;
            }

            string SPACE = "      ";
            // 영수증 사이즈를 58mm로 설정
            if (UIManager.Instance().Is58mmPrinter) SPACE = "";

            bool IsOpenPrinter = false;
            // 프린터 생성
            if (printer == null)
            {
                printer = new device.DevSerialPrinter();
                //printer.setPort("COM15");
                printer.setBaudRate(baudRate);  // 19200
                printer.setParity(System.IO.Ports.Parity.None);
                printer.setDataBits(8);
                printer.setStopBits(System.IO.Ports.StopBits.One);

                if (!string.IsNullOrEmpty(port))
                {
                    printer.setPort(port);
                    if (printer.OpenPrinter())
                    {
                        IsOpenPrinter = true;
                    }
                }
                else
                {
                    for (int i = 1; i < 20; i++)
                    {
                        printer.setPort("COM" + i);
                        if (printer.OpenPrinter())
                        {
                            IsOpenPrinter = true;
                            break;
                        }
                    }
                }
            }

            if (!IsOpenPrinter)
            {
                printer = null;
                return;
            }

            if (UIManager.Instance().PosModel.Environment.EnvPosSetup.PrintingLogo == "Y")
            {
                // logo 출력
                using (var stream = ResourceUtil.GetImageStream("rra_logo.bmp"))
                {
                    Image image = Image.FromStream(stream);
                    Bitmap bitmap = new Bitmap(image);

                    printer.printLogo(bitmap, 420, 128);      // LOGO 출력  // 512
                }
            }

            foreach (JournalString node in journal.JournalStringList)
            {
                if (node.Type.Equals("reprint") && !isReprint)
                {
                    continue;
                }
                else if (node.Type.Equals("reprint") && isReprint)
                {
                    printer.printText(SPACE + "               Copy" + "\n");
                    printer.printText(SPACE + "-----------------------------------" + "\n");
                    continue;
                }

                if (isReprint && !string.IsNullOrEmpty(node.Data) && node.Data.Contains("RECEIPT NUMBER :"))
                {
                    node.Data = node.Data.Replace("NS", "CS");
                    node.Data = node.Data.Replace("NR", "CR");
                }

                // Normal 문자 출력
                if (node.Type.Equals(""))
                {
                    printer.printText(SPACE + node.Data + "\n");
                }
                // 가로 확대
                else if (node.Type.Equals("wide"))
                {
                    printer.printWide(SPACE + node.Data + "\n");
                }
                // 세로 확대
                else if (node.Type.Equals("hide"))
                {
                    printer.printHide(SPACE + node.Data + "\n");
                }
                // 진하게
                else if (node.Type.Equals("bold"))
                {
                    printer.printBold(SPACE + node.Data + "\n");
                }
                // 가로세로 확대
                else if (node.Type.Equals("widehide"))
                {
                    printer.printWideHide(SPACE + node.Data + "\n");
                }
                // 역상 출력
                else if (node.Type.Equals("invert"))
                {
                    printer.printInvert(SPACE + node.Data + "\n");
                }
                // 밑줄 출력
                else if (node.Type.Equals("underline"))
                {
                    printer.printUnderline(SPACE + node.Data + "\n");
                }

                // 중앙 정렬
                else if (node.Type.Equals("center"))
                {
                    printer.printCenter(SPACE + node.Data + "\n");
                }

                // 중앙 정렬 + 가로확대
                else if (node.Type.Equals("widecenter"))
                {
                    printer.printWideCenter(SPACE + node.Data + "\n");
                }

                // 바코드 출력
                else if (node.Type.Equals("barcode"))
                {
                    printer.printBarcode(node.Data);
                }
                else if (node.Type.Equals("qrcode"))
                {
                    PrintQrCode(printer, RraSdcService.RECEIPT_URL + "/common/link/ebm/receipt/indexEbmReceipt?rcptNo=" + node.Data);
                }
                // 영수증 출력 및 Cutting
                else if (node.Type.Equals("cutpaper"))
                {
                    printer.print();
                    printer.cutPaper();
                }

                // 비동기화 출력
                else if (node.Type.Equals("AsyncCut"))
                {
                    printer.print(false);
                    printer.cutPaper();
                }
            }

            //@jacq_20201124 출력 sleep 추가
            System.Threading.Thread.Sleep(1000);

            if (printer != null)
            {
                printer.ClosePrinter();
                printer = null;
            }
        }

        public void PrintQrCode(device.DevSerialPrinter printer, string qrdata)
        {
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
            byte[] sizeQR = { (byte)0x1d, (byte)0x28, (byte)0x6b, (byte)0x03, (byte)0x00, (byte)0x31, (byte)0x43, (byte)0x05 };


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

            printer.printPrintBuff(BarCodeInit);

            // write() simply appends the data to the buffer
            printer.printPrintBuff(modelQR);
            printer.printPrintBuff(sizeQR);
            printer.printPrintBuff(errorQR);
            printer.printPrintBuff(storeQR);

            byte[] buffer = Encoding.UTF8.GetBytes(qrdata);
            printer.printPrintBuff(buffer);
            printer.printPrintBuff(printQR);

            byte[] bufferFeed = Encoding.UTF8.GetBytes("\n");
            printer.printPrintBuff(bufferFeed);
        }

        public void PrintJournalUSBII(Models.journal.JournalModel journal, bool isReprint)
        {
            try
            {
                this.journal1 = journal;
                this.IsReprint = isReprint;

                try
                {
                    if (UIManager.Instance().Is58mmPrinter) printFont = new Font("Consolas", 6, System.Drawing.FontStyle.Bold);
                    else printFont = new Font("Consolas", 9, System.Drawing.FontStyle.Bold);
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(this.JournalPrintPageUSBII);
                    pd.Print();
                }
                finally
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void JournalPrintPageUSBII(object sender, PrintPageEventArgs ev)
        {
            int cellWidth = 18;
            int cellHeight = 18;

            if (UIManager.Instance().Is58mmPrinter)
            {
                cellWidth = 12;
                cellHeight = 12;
            }

            int hIndex = 0;
            if (UIManager.Instance().PosModel.Environment.EnvPosSetup.PrintingLogo == "Y")
            {
                // logo 출력
                using (var stream = ResourceUtil.GetImageStream("rra_logo.bmp"))
                {
                    Image image = Image.FromStream(stream);
                    ev.Graphics.DrawImage(image, (cellWidth * 1) + 3, (cellHeight * (1 + hIndex)) + 3);
                }
                hIndex += 6;
            }

            Brush brush = new SolidBrush(System.Drawing.Color.Black);
            for (int i = 0; i < journal1.JournalStringList.Count; i++, hIndex++)
            {
                if (journal1.JournalStringList[i].Type.Equals("reprint") && !this.IsReprint)
                {
                    continue;
                }
                else if (journal1.JournalStringList[i].Type.Equals("reprint") && this.IsReprint)
                {
                    ev.Graphics.DrawString("               Copy", printFont, brush, (cellWidth * 1) + 3, (cellHeight * (1 + hIndex)) + 3);
                    hIndex++;
                    ev.Graphics.DrawString("-----------------------------------", printFont, brush, (cellWidth * 1) + 3, (cellHeight * (1 + hIndex)) + 3);
                    continue;
                }

                if (this.IsReprint && !string.IsNullOrEmpty(journal1.JournalStringList[i].Data) && journal1.JournalStringList[i].Data.Contains("RECEIPT NUMBER :"))
                {
                    journal1.JournalStringList[i].Data = journal1.JournalStringList[i].Data.Replace("NS", "CS");
                    journal1.JournalStringList[i].Data = journal1.JournalStringList[i].Data.Replace("NR", "CR");
                }

                // 바코드 출력
                if (journal1.JournalStringList[i].Type.Equals("barcode"))
                {
                    // journal1.JournalStringList[i].Data
                    ev.Graphics.DrawString("barcode", printFont, brush, (cellWidth * 1) + 3, (cellHeight * (1 + hIndex)) + 3);
                }
                else if (journal1.JournalStringList[i].Type.Equals("qrcode"))
                {
                    string grData = RraSdcService.RECEIPT_URL + "/common/link/ebm/receipt/indexEbmReceipt?rcptNo=" + journal1.JournalStringList[i].Data;

                    Rectangle rectangle;

                    if (UIManager.Instance().Is58mmPrinter)
                    {
                        rectangle = new Rectangle((cellWidth * 1) + 3, (cellHeight * (1 + hIndex)) + 3, (120), (120));
                    }
                    else
                    {
                        rectangle = new Rectangle((cellWidth * 1) + 63, (cellHeight * (1 + hIndex)) + 3, (120), (120));
                    }

                    Image image = GetQrcodeImage(grData);
                    ev.Graphics.DrawImage(image, rectangle);

                    hIndex += 7;

                    ev.Graphics.DrawString("rra", printFont, brush, (cellWidth * 1) + 3, (cellHeight * (1 + hIndex)) + 3);

                }
                else
                {
                    if (journal1.JournalStringList[i].Data.Length > 35)
                    {
                        ev.Graphics.DrawString(journal1.JournalStringList[i].Data.Substring(0, 35), printFont, brush, (cellWidth * 1) + 3, (cellHeight * (1 + hIndex)) + 3);
                        hIndex++;
                        ev.Graphics.DrawString(journal1.JournalStringList[i].Data.Substring(35), printFont, brush, (cellWidth * 1) + 3, (cellHeight * (1 + hIndex)) + 3);
                    }
                    else
                    {
                        ev.Graphics.DrawString(journal1.JournalStringList[i].Data, printFont, brush, (cellWidth * 1) + 3, (cellHeight * (1 + hIndex)) + 3);
                    }
                }
            }

            ev.HasMorePages = false;
        }

        public Image GetQrcodeImage(string qrData)
        {
            Image qrImage = null;

            ZXing.BarcodeWriter barcodeWriter = new ZXing.BarcodeWriter();
            barcodeWriter.Format = ZXing.BarcodeFormat.QR_CODE;

            barcodeWriter.Options.Width = 256;
            barcodeWriter.Options.Height = 256;

            qrImage = barcodeWriter.Write(qrData);

            return qrImage;
        }

        public void PrintJournalA4(Models.journal.JournalModel journal, bool isReprint)
        {
            try
            {
                this.journal1 = journal;

                try
                {
                    printFont = new Font("Consolas", 10);
                    PrintDocument pd = new PrintDocument();

                    salesModelIndex = 0;

                    pd.PrintPage += new PrintPageEventHandler(this.JournalPrintPage);

                    //PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                    pd.Print();
                }
                finally
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void JournalPrintPage(object sender, PrintPageEventArgs ev)
        {
            int cellWidth = 20;
            int cellHeight = 20;
            Rectangle rectangle;

            using (var stream = ResourceUtil.GetImageStream("EBM2x-128.png"))
            {
                rectangle = new Rectangle((cellWidth * 1), (cellHeight * 1), (cellWidth * 5), (cellHeight * 5));
                Image image = Image.FromStream(stream);
                ev.Graphics.DrawImage(image, rectangle);
            }

            Brush brush = new SolidBrush(System.Drawing.Color.Black);
            Font titleFont = new Font("Consolas", 14);
            Font itemFont = new Font("Consolas", 8);
            for (int i = 0; i < journal1.JournalStringList.Count; i++)
            {
                if (i < 6)
                {
                    if (i == 0)
                    {
                        ev.Graphics.DrawString(journal1.JournalStringList[i].Data, titleFont, brush, (cellWidth * 6) + 3, (cellHeight * (1 + i)));
                    }
                    else
                    {
                        ev.Graphics.DrawString(journal1.JournalStringList[i].Data, itemFont, brush, (cellWidth * 6) + 3, (cellHeight * (1 + i)));
                    }
                }
                else
                {
                    ev.Graphics.DrawString(journal1.JournalStringList[i].Data, printFont, brush, (cellWidth * 1) + 3, (cellHeight * (1 + i)) + 3);
                }
            }

            ev.HasMorePages = false;
        }

        public void PrintInvoiceA4(TransactionSalesModel salesModel,
            Models.journal.JournalModel journal1,
            Models.journal.JournalModel journal2,
            Models.journal.JournalModel journal3,
            Models.journal.JournalModel journal4, bool isReprint,
            Models.journal.JournalModel journal5)
        {
            this.salesModel = salesModel;

            salesModelIndex = 0;
            pageCount = 0;

            try
            {
                this.journal1 = journal1;
                this.journal2 = journal2;
                this.journal3 = journal3;
                this.journal4 = journal4;

                try
                {
                    printFont = new Font("Consolas", 10); //MS Reference Sans Serif
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(this.InvoicePrintPage);
                    pd.Print();
                }
                finally
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InvoicePrintPage(object sender, PrintPageEventArgs ev)
        {
            int cellWidth = 20;
            int cellHeight = 20;

            InvoicePrintPageBase(ev);

            Brush brush = new SolidBrush(System.Drawing.Color.Black);
            Font titleFont = new Font("Consolas", 14);
            Font itemFont = new Font("Consolas", 8);

            for (int i = 0; i < journal1.JournalStringList.Count; i++)
            {
                if (i == 0)
                {
                    ev.Graphics.DrawString(journal1.JournalStringList[i].Data, titleFont, brush, (cellWidth * 6) + 3, (cellHeight * (1 + i)));
                }
                else
                {
                    ev.Graphics.DrawString(journal1.JournalStringList[i].Data, itemFont, brush, (cellWidth * 6) + 3, (cellHeight * (1 + i)) + 3);
                }
            }
            for (int i = 0; i < journal2.JournalStringList.Count; i++)
            {
                ev.Graphics.DrawString(journal2.JournalStringList[i].Data, printFont, brush, (cellWidth * 1) + 3, (cellHeight * (8 + i)) + 3);
            }
            for (int i = 0; i < journal3.JournalStringList.Count; i++)
            {
                if (i == 0)
                {
                    ev.Graphics.DrawString(journal3.JournalStringList[i].Data, titleFont, brush, (cellWidth * 24) + 3, (cellHeight * (3 + i)));
                }
                else
                {
                    ev.Graphics.DrawString(journal3.JournalStringList[i].Data, printFont, brush, (cellWidth * 24) + 3, (cellHeight * (3 + i)) + 3);
                }
            }
            for (int i = 0; i < journal4.JournalStringList.Count; i++)
            {
                ev.Graphics.DrawString(journal4.JournalStringList[i].Data, printFont, brush, (cellWidth * 1) + 3, (cellHeight * (44 + i)) + 3);
            }

            int sign = 1;
            if (!string.IsNullOrEmpty(salesModel.TranRecord.RcptTyCd) && salesModel.TranRecord.RcptTyCd.Equals("R"))
            {
                ev.Graphics.DrawString("REFUND IS APPROVED ONLY FOR", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 8) + 3);
                ev.Graphics.DrawString("ORIGINAL SALES RECEIPT", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 9) + 3);
                sign = -1;
            }
            else
            {
                if (UIManager.Instance().PosModel.Environment.EnvPosSetup.NonVAT)
                {
                    ev.Graphics.DrawString("Iyi nyemezabuguzi yemewe na RRA,", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 8) + 3);
                    ev.Graphics.DrawString("n'ubwo itari iya TVA", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 9) + 3);
                    ev.Graphics.DrawString("This invoice is approved by RRA,", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 10) + 3);
                    ev.Graphics.DrawString("though is not for VAT", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 11) + 3);
                }
            }

            string buffer = string.Format("{0:##,##0.00}", salesModel.TranRecord.TotAmt * sign);
            ev.Graphics.DrawString(buffer, printFont, brush, (cellWidth * 31) + 3, (cellHeight * 45) + 15);
            buffer = string.Format("{0:##,##0.00}", salesModel.TranRecord.TaxblAmtA * sign);
            ev.Graphics.DrawString(buffer, printFont, brush, (cellWidth * 31) + 3, (cellHeight * 47) + 15);
            buffer = string.Format("{0:##,##0.00}", salesModel.TranRecord.TaxblAmtB * sign);
            ev.Graphics.DrawString(buffer, printFont, brush, (cellWidth * 31) + 3, (cellHeight * 49) + 15);

            if (UIManager.Instance().PosModel.Environment.EnvPosSetup.NonVAT)
            {
                buffer = string.Format("{0:##,##0.00}", salesModel.TranRecord.TaxblAmtD * sign);
                ev.Graphics.DrawString(buffer, printFont, brush, (cellWidth * 31) + 3, (cellHeight * 51) + 15);
            }
            else
            {
                buffer = string.Format("{0:##,##0.00}", salesModel.TranRecord.TaxAmtB * sign);
                ev.Graphics.DrawString(buffer, printFont, brush, (cellWidth * 31) + 3, (cellHeight * 51) + 15);
            }

            buffer = string.Format("{0:##,##0.00}", salesModel.TranRecord.TotTaxAmt * sign);
            ev.Graphics.DrawString(buffer, printFont, brush, (cellWidth * 31) + 3, (cellHeight * 53) + 15);

            int addLineHeight = 15;
            for (int i = 0; i < 38; i++)
            {
                int addLine = 0;

                // 2021.03.31
                if (salesModel.ItemRecords.Count <= salesModelIndex) break;

                TrnsSaleItemRecord itemNode = salesModel.ItemRecords[salesModelIndex];

                if (i == 36 && itemNode.ItemNm.Length > 120) break;
                if (i == 37 && itemNode.ItemNm.Length > 30) break;

                ev.Graphics.DrawString(itemNode.ItemCd, itemFont, brush, (cellWidth * 1) + 3, (cellHeight * 14) + 10 + (i * addLineHeight));

                //if ( i > 7 ) {
                //    itemNode.ItemNm = "qwertyui opas dfghjklzxcvb nmqwertyuio pasdfghjkl zxcvbnmqw ertyuiopa sdfghjklzxcv bnmqwertyui opasdfghjklzxcv klzxcvb nmqwertyuio pasdfghjkl zxcvbnmqw ertyuiopa sdfghjklzxcv bnmqwertyui opasdfghjklzxcv";
                //}
                if (itemNode.ItemNm.Length > 30)
                {
                    ev.Graphics.DrawString(itemNode.ItemNm.Substring(0, 30), itemFont, brush, (cellWidth * 8) + 3, (cellHeight * 14) + 10 + (i * addLineHeight));
                    string itemName = itemNode.ItemNm.Substring(30);
                    if (itemName.Length > 30)
                    {
                        ev.Graphics.DrawString(itemName.Substring(0, 30), itemFont, brush, (cellWidth * 8) + 3, (cellHeight * 14) + 10 + ((i + 1) * addLineHeight));
                        string remainItem = itemNode.ItemNm.Substring(60);
                        if (remainItem.Length > 20)
                        {
                            ev.Graphics.DrawString(remainItem.Substring(0, 30), itemFont, brush, (cellWidth * 8) + 3, (cellHeight * 14) + 25 + ((i + 1) * addLineHeight));
                            ev.Graphics.DrawString(remainItem.Substring(30), itemFont, brush, (cellWidth * 8) + 3, (cellHeight * 14) + 25 + ((i + 2) * addLineHeight));
                            addLine = 3;
                        }
                        else
                        {
                            ev.Graphics.DrawString(itemName, itemFont, brush, (cellWidth * 8) + 3, (cellHeight * 14) + 10 + ((i + 1) * addLineHeight));
                            addLine = 1;
                        }
                    }
                    else
                    {
                        ev.Graphics.DrawString(itemName, itemFont, brush, (cellWidth * 8) + 3, (cellHeight * 14) + 10 + ((i + 1) * addLineHeight));
                        addLine = 1;
                    }
                }
                else
                {
                    ev.Graphics.DrawString(itemNode.ItemNm, itemFont, brush, (cellWidth * 8) + 3, (cellHeight * 14) + 10 + (i * addLineHeight));
                }

                ev.Graphics.DrawString(itemNode.Qty.ToString("#,##0"), itemFont, brush, (cellWidth * 18) + 3, (cellHeight * 14) + 10 + (i * addLineHeight));
                ev.Graphics.DrawString(itemNode.TaxTyCd, itemFont, brush, (cellWidth * 21) + 3, (cellHeight * 14) + 10 + (i * addLineHeight));
                buffer = string.Format("{0:##,##0.00}", itemNode.Prc);
                ev.Graphics.DrawString(buffer, itemFont, brush, (cellWidth * 24) + 3, (cellHeight * 14) + 10 + (i * addLineHeight));
                buffer = string.Format("{0:##,##0.00}", itemNode.TotAmt);
                ev.Graphics.DrawString(buffer, itemFont, brush, (cellWidth * 31) + 3, (cellHeight * 14) + 10 + (i * addLineHeight));

                salesModelIndex++;
                i += addLine;
            }
            //salesModelIndex = 38;

            // 페이지 넘김 처리가 필요합니다.
            // 2021.03.31
            if (salesModel.ItemRecords.Count > salesModelIndex)
            {
                pageCount++;
                buffer = "               Continues";
                ev.Graphics.DrawString(buffer, itemFont, brush, (cellWidth * 31) + 3, (cellHeight * (14 + (14 * 2))) + 25);
                ev.HasMorePages = true;
            }
            else
            {
                if (pageCount > 0)
                {
                    buffer = "                     End";
                    ev.Graphics.DrawString(buffer, itemFont, brush, (cellWidth * 31) + 3, (cellHeight * (14 + (14 * 2))) + 25);
                }
                ev.HasMorePages = false;
            }
        }
        private void InvoicePrintPageBase(PrintPageEventArgs ev)
        {
            int cellWidth = 20;
            int cellHeight = 20;
            int leftMargin = ev.MarginBounds.Left;
            int topMargin = ev.MarginBounds.Top;
            Pen blackPen = new Pen(System.Drawing.Color.Black, 1);

            Rectangle rectangle;

            using (var stream = ResourceUtil.GetImageStream("EBM2x-128.png"))
            {
                rectangle = new Rectangle((cellWidth * 1), (cellHeight * 1), (cellWidth * 5), (cellHeight * 5));
                Image image = Image.FromStream(stream);
                ev.Graphics.DrawImage(image, rectangle);
            }

            rectangle = new Rectangle((cellWidth * 1), (cellHeight * 8), (cellWidth * 15), (cellHeight * 3));
            ev.Graphics.DrawRectangle(blackPen, rectangle);

            rectangle = new Rectangle((cellWidth * 24), (cellHeight * 3), (cellWidth * 15), (cellHeight * 5));
            ev.Graphics.DrawRectangle(blackPen, rectangle);

            rectangle = new Rectangle((cellWidth * 1), (cellHeight * 12), (cellWidth * 38), (cellHeight * 2));
            ev.Graphics.DrawRectangle(blackPen, rectangle);

            rectangle = new Rectangle((cellWidth * 1), (cellHeight * 14), (cellWidth * 38), (cellHeight * 30));
            ev.Graphics.DrawRectangle(blackPen, rectangle);

            rectangle = new Rectangle((cellWidth * 8), (cellHeight * 12), (cellWidth * 10), (cellHeight * 32));
            ev.Graphics.DrawRectangle(blackPen, rectangle);

            rectangle = new Rectangle((cellWidth * 21), (cellHeight * 12), (cellWidth * 3), (cellHeight * 32));
            ev.Graphics.DrawRectangle(blackPen, rectangle);

            rectangle = new Rectangle((cellWidth * 24), (cellHeight * 12), (cellWidth * 7), (cellHeight * 32));
            ev.Graphics.DrawRectangle(blackPen, rectangle);

            rectangle = new Rectangle((cellWidth * 24), (cellHeight * 45), (cellWidth * 15), (cellHeight * 2));
            ev.Graphics.DrawRectangle(blackPen, rectangle);
            rectangle = new Rectangle((cellWidth * 24), (cellHeight * 47), (cellWidth * 15), (cellHeight * 2));
            ev.Graphics.DrawRectangle(blackPen, rectangle);
            rectangle = new Rectangle((cellWidth * 24), (cellHeight * 49), (cellWidth * 15), (cellHeight * 2));
            ev.Graphics.DrawRectangle(blackPen, rectangle);
            rectangle = new Rectangle((cellWidth * 24), (cellHeight * 51), (cellWidth * 15), (cellHeight * 2));
            ev.Graphics.DrawRectangle(blackPen, rectangle);
            rectangle = new Rectangle((cellWidth * 24), (cellHeight * 53), (cellWidth * 15), (cellHeight * 2));
            ev.Graphics.DrawRectangle(blackPen, rectangle);
            rectangle = new Rectangle((cellWidth * 24), (cellHeight * 45), (cellWidth * 7), (cellHeight * 10));
            ev.Graphics.DrawRectangle(blackPen, rectangle);

            Brush brush = new SolidBrush(System.Drawing.Color.Black);

            ev.Graphics.DrawString("INVOICE TO", printFont, brush, (cellWidth * 1) + 3, (cellHeight * 7) + 0);

            ev.Graphics.DrawString("Item Code", printFont, brush, (cellWidth * 1) + 3, (cellHeight * 12) + 15);
            ev.Graphics.DrawString("Item Description", printFont, brush, (cellWidth * 8) + 3, (cellHeight * 12) + 15);
            ev.Graphics.DrawString("Qty", printFont, brush, (cellWidth * 18) + 3, (cellHeight * 12) + 15);
            ev.Graphics.DrawString("Tax", printFont, brush, (cellWidth * 21) + 3, (cellHeight * 12) + 15);
            ev.Graphics.DrawString("Unit Price", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 12) + 15);
            ev.Graphics.DrawString("Total Price", printFont, brush, (cellWidth * 31) + 3, (cellHeight * 12) + 15);

            ev.Graphics.DrawString("Total Rwf", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 45) + 15);
            ev.Graphics.DrawString("Total A-EX Rwf", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 47) + 15);
            ev.Graphics.DrawString("Total B-18% Rwf", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 49) + 15);
            if (UIManager.Instance().PosModel.Environment.EnvPosSetup.NonVAT)
            {
                ev.Graphics.DrawString("Total D", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 51) + 15);
            }
            else
            {
                ev.Graphics.DrawString("Total Tax B Rwf", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 51) + 15);
            }
            ev.Graphics.DrawString("Total Tax Rwf", printFont, brush, (cellWidth * 24) + 3, (cellHeight * 53) + 15);
        }
    }
}