using System;
using System.Collections.Generic;

namespace EBM2x.RraSdc.model
{
    public class BhfInsuranceSaveReq {
	    public string tin { get; set; }  // TIN
	    public string bhfId { get; set; }  // Branch ID
	    public string isrccCd { get; set; }  // Insurance Code
	    public string isrccNm { get; set; }  // Insurance Name
	    public int isrcRt { get; set; }  // Premium Rate
	    public string useYn { get; set; }  // Used / UnUsed
        public string regrId { get; set; }  //   ����� ���̵�
        public string regrNm { get; set; }  //    ����� ��
        public string modrId { get; set; }  //   ������ ���̵�
        public string modrNm { get; set; }  //    ������ ��
    }
}
