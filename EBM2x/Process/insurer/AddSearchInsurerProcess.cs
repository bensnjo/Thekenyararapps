﻿using EBM2x.Database.Master;
using EBM2x.Models;
using EBM2x.Models.ListView;
using EBM2x.Models.signon;
using EBM2x.Models.tran;
using EBM2x.Process.eot;
using EBM2x.Services;

namespace EBM2x.Process.insurer
{
    public class AddSearchInsurerProcess
    {
        public static string excuteProcess(SearchInsurerNode searchInsurerNode, PosModel posModel, InputModel inputModel, InformationModel informationModel)
        {
            posModel.TranModel.TranNode.InsurerNode = searchInsurerNode;
            posModel.TranModel.TranNode.ItemList.ChangeDiscountInsurers(searchInsurerNode.InsurerRate, searchInsurerNode.InsurerCode, searchInsurerNode.InsurerName);
            posModel.TranModel.TranNode.CalculateItemList();
            return StateModel.OP_NEXT;
        }
    }
}
