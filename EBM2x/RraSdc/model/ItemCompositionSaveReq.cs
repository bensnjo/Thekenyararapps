using System;
using System.Collections.Generic;

namespace EBM2x.RraSdc.model
{
    public class ItemCompositionSaveReq {
	    public string tin { get; set; }  // TIN
	    public string bhfId { get; set; }  // Branch ID
	    public string itemCd { get; set; }  // Item Code
	    public string cpstItemCd { get; set; }  // 
	    public double cpstQty { get; set; }  // 
        public string regrId { get; set; }  //   ����� ���̵�
        public string regrNm { get; set; }  //    ����� ��
        public string modrId { get; set; }  //   ������ ���̵�
        public string modrNm { get; set; }  //    ������ ��
    }
}
