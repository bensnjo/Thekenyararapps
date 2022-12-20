﻿using EBM2x.Datafile.regitotal;
using EBM2x.Datafile.trlog;
using EBM2x.Models;
using EBM2x.Models.tran;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBM2x.Process.eot
{
    public class HoldEndOfTransaction
    {
        public static string excuteProcess(PosModel posModel, InputModel inputModel, InformationModel informationModel)
        {
           
            posModel.RegiTotal.RegiHeader.UpdateDate = string.Empty;
            posModel.TranInformation.DateTime = posModel.RegiTotal.RegiHeader.UpdateDate
                + posModel.RegiTotal.RegiHeader.UpdateTime;

            posModel.RegiTotal.RegiHeader.UpdateDateTime();
            posModel.TranInformation.DateTime = posModel.RegiTotal.RegiHeader.UpdateDate + posModel.RegiTotal.RegiHeader.UpdateTime;
            posModel.TranModel.TranInformation.initialize(posModel.TranInformation);

            
            TrHoldWriter.write(posModel);

            
            posModel.RegiTotal.RegiHeader.HoldCount = TrHoldReader.GetCount();

            
            RegiTotalWriter.write(posModel);
            
            PosModelInitialize.excuteProcess(posModel, inputModel, informationModel);

            return StateModel.OP_FAR;
        }
    }
}
