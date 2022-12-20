﻿using EBM2x.Models;
using EBM2x.Models.config;
using EBM2x.Models.Preset;
using EBM2x.Models.tran;
using EBM2x.Utils;
using System;

namespace EBM2x.Process.tran
{
    public class AddPresetItemProcess
    {
        public static string excuteProcess(PresetItemNode presetItemNode, PosModel posModel, InputModel inputModel, InformationModel informationModel)
        {
            EnvPosSetup envPosSetup = posModel.Environment.EnvPosSetup;
            ItemNode itemnode = AddBarcodeItemProcess.GetItemNode(posModel, envPosSetup.GblTaxIdNo, presetItemNode.ItemCode);
            if (itemnode != null)
            {
                if (itemnode.UseExpiration.Equals("Y") && !string.IsNullOrEmpty(itemnode.ExpirationDate))
                {
                    DateTime ExpirationDate = Common.DateFormat(itemnode.ExpirationDate);
                    TimeSpan TS = ExpirationDate - DateTime.Now;

                    int diffDay = TS.Days; 
                    if (diffDay < 0)
                    {
                        informationModel.AlertTitle = "";
                        informationModel.AlertMessage = "There is an expired product. Please Check.";
                        return StateModel.OP_ALERT;
                    }
                }

                itemnode.TranFlag = posModel.TranModel.TranNode.TranFlag;
                itemnode.Sign = posModel.TranModel.TranNode.Sign;
                if(presetItemNode.Price > 0) itemnode.Price = presetItemNode.Price;
                if(presetItemNode.Quantity > 0) itemnode.Quantity = presetItemNode.Quantity;

                int index = posModel.TranModel.TranNode.ItemList.FindItem(itemnode);
                if (index >= 0)
                {
                    posModel.TranModel.TranNode.ItemList.SetCurrent(index);
                    if (ChangeQuantityProcess.checkStockProcess(posModel, posModel.TranModel.TranNode.ItemList.Get(index), 1))
                    {
                        posModel.TranModel.TranNode.ItemList.Get(index).Quantity += 1;
                    }
                    else
                    {
                        informationModel.SetWarningMessage("Greater than current stock.");
                        informationModel.AlertTitle = "";
                        informationModel.AlertMessage = "Greater than current stock.";
                        return StateModel.OP_ALERT;
                    }
                }
                else
                {
                    posModel.TranModel.TranNode.ItemList.Add(itemnode);
                }
                posModel.TranModel.TranNode.CalculateItemList();
                return StateModel.OP_NEXT;
            }
            else
            {
                informationModel.SetWarningMessage("It is an unregistered product. Check the PRESET!!");
                return StateModel.OP_RETRY;
            }
        }
    }
}
