using System;
using System.Collections.Generic;

namespace EBM2x.RraSdc.model
{
    public class BhfCustSaveReq {
	    public string tin { get; set; }  // TIN
	    public string bhfId { get; set; }  // Branch ID
	    public string custNo { get; set; }  // Customer Number
	    public string custTin { get; set; }  // Customer TIN
	    public string custNid { get; set; }  // Customer National ID
	    public string custNm { get; set; }  // Customer Name
	    public string custBhfId { get; set; }  // Customer Branch Office ID
	    public string adrs { get; set; }  // Address
	    public string telNo { get; set; }  // Contact
        public string email { get; set; }  // �̸���
        public string faxNo { get; set; }  // �ѽ� ��ȣ
        public string useYn { get; set; }  // ��� ����
        public string remark { get; set; }  //  ���

        public string regrId { get; set; }  //   ����� ���̵�
        public string regrNm { get; set; }  //    ����� ��
        public string modrId { get; set; }  //   ������ ���̵�
        public string modrNm { get; set; }  //    ������ ��

    }
}
