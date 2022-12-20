using System;
using System.Collections.Generic;

namespace EBM2x.RraSdc.model
{
    public class ImportItemUpdateReq {
	    public string tin { get; set; }  // TIN
	    public string bhfId { get; set; }  // Branch ID
	    public string taskCd { get; set; }  // Task Code
        public string hsCd { get; set; }
        public string dclDe { get; set; }  // Declaration Date
	    public int itemSeq { get; set; }  // Item Sequence
	    public string itemClsCd { get; set; }  // Item Classification Code
	    public string itemCd { get; set; }  // Item Code
	    public string imptItemSttsCd { get; set; }  // Import Item Status Code
	    public string remark { get; set; }  // Remark
        public string regrId { get; set; }  //   ����� ���̵�
        public string regrNm { get; set; }  //    ����� ��
        public string modrId { get; set; }  //   ������ ���̵�
        public string modrNm { get; set; }  //    ������ ��
    }
}
