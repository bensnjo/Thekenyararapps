using System;
using System.Collections.Generic;

namespace EBM2x.RraSdc.model
{
    public class StockMasterSaveReq {
	    public string tin { get; set; }  // TIN
	    public string bhfId { get; set; }  // Branch ID
	    public string itemCd { get; set; }  // Item Code
	    public double rsdQty { get; set; }  // Remain Quantity
        public string regrId { get; set; }  //   ����� ���̵�
        public string regrNm { get; set; }  //    ����� ��
        public string modrId { get; set; }  //   ������ ���̵�
        public string modrNm { get; set; }  //    ������ ��
    }
}
