using System;
using System.Collections.Generic;

namespace EBM2x.RraSdc.model
{
    public class TrnsPurchaseSalesRes {
	    public string resultCd { get; set; }  // Result Code
	    public string resultMsg { get; set; }  // Result Message
	    public string resultDt { get; set; }  // Result Date
	    public TrnsPurchaseSalesResData data { get; set; }  // Result Date
    }
	public class TrnsPurchaseSalesResData {
		public List<TrnsPurchaseSales> saleList { get; set; }  // Code Class List
	}

	public class TrnsPurchaseSales {
        public string spplrTin { get; set; }  // Customer TIN
        public string spplrNm { get; set; }  // Customer Name
        public string spplrBhfId { get; set; }  // Customer Branch ID
		public long spplrInvcNo { get; set; }  // Invoice Number
		//public long orgInvcNo { get; set; }  // Original Invoice Number
		//public string salesTyCd { get; set; }  // Sales Type Code
		public string rcptTyCd { get; set; }  // Receipt Type Code
		public string pmtTyCd { get; set; }  // Payment Type Code
		public string cfmDt { get; set; }  // Validated Date
		public string salesDt { get; set; }  // Sale Date
		public string stockRlsDt { get; set; }  // Stock Released Date
		//public string cnclReqDt { get; set; }  // Cancel Requested Date
		//public string cnclDt { get; set; }  // Canceled Date
		//public string rfdDt { get; set; }  // Refunded Date
		public int totItemCnt { get; set; }  // Total Item Count
		public double taxblAmtA { get; set; }  // Taxable Amount A
		public double taxblAmtB { get; set; }  // Taxable Amount B
		public double taxblAmtC { get; set; }  // Taxable Amount C
		public double taxblAmtD { get; set; }  // Taxable Amount D
		public int taxRtA { get; set; }  // Tax Rate A
		public int taxRtB { get; set; }  // Tax Rate B
		public int taxRtC { get; set; }  // Tax Rate C
		public int taxRtD { get; set; }  // Tax Rate D
		public double taxAmtA { get; set; }  // Tax Amount A
		public double taxAmtB { get; set; }  // Tax Amount B
		public double taxAmtC { get; set; }  // Tax Amount C
		public double taxAmtD { get; set; }  // Tax Amount D
		public double totTaxblAmt { get; set; }  // Total Taxable Amount
		public double totTaxAmt { get; set; }  // Total Tax Amount
		public double totAmt { get; set; }  // Total Amount
		public string remark { get; set; }  // remark

		public List<TrnsPurchaseSalesItem> itemList { get; set; }
    }

	public class TrnsPurchaseSalesItem {
		public int itemSeq { get; set; }  // Item Sequence Number
		public string itemCd { get; set; }  // Item Code
		public string itemClsCd { get; set; }  // Item Classification Code
		public string itemNm { get; set; }  // Item Name
		public string bcd { get; set; }  // Bar code
		public string pkgUnitCd { get; set; }  // Packaging Unit Code
		public double pkg { get; set; }  // Package
		public string qtyUnitCd { get; set; }  // Quantity Unit Code
		public double qty { get; set; }  // Quantity
		public double prc { get; set; }  // Unit Price
		public double splyAmt { get; set; }  // Supply Amount
		public double dcRt { get; set; }  // Discount Rate
		public double dcAmt { get; set; }  // Discount Amount
		public string taxTyCd { get; set; }  // Taxation Type Code
		public double taxblAmt { get; set; }  // Taxable Amount
		public double taxAmt { get; set; }  // Tax Amount
		public double totAmt { get; set; }  // Total Amount
	}
}
