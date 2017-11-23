﻿using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Domain
{
    /// <summary>
    /// Распиловка по дуге
    /// </summary>
    public class SawingArcTechOperation : SawingTechOperation
    {
        /// <summary>
        /// Обрабатываемая область
        /// </summary>
        //public ArcProcessingArea processingArea { get; }

       
        public SawingArcTechOperation(TechProcess techProcess, ArcProcessingArea processingArea, SawingTechOperationParams techOperationParams)
             : base(techProcess, processingArea, techOperationParams)
        {
        }

        public override void BuildProcessing()
        {
            base.BuildProcessing();
        }
    }
}
