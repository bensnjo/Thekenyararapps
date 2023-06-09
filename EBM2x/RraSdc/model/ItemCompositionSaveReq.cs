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
        public string regrId { get; set; }  //   등록자 아이디
        public string regrNm { get; set; }  //    등록장 명
        public string modrId { get; set; }  //   수정자 아이디
        public string modrNm { get; set; }  //    수정자 명
    }
}
