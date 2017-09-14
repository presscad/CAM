﻿//using Autodesk.AutoCAD.DatabaseServices;
//using Autodesk.AutoCAD.Geometry;
//using Autodesk.AutoCAD.Runtime;
using CAM.Commands;
using CAM.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAM.UI.Test
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

            var techProcessService = new TechProcessService();
            techProcessView.SetTechProcessService(techProcessService);

            //var context = new Context();
            //var techProcessList = new List<TechProcess>();
            //var treeNodeCollection = techProcessView.TreeNodeCollection;

            //var techProcessParams = new TechProcessParams();
            //var sawingLineTechOperationParamsDefault = new SawingTechOperationParams();
            //var sawingArcTechOperationParamsDefault = new SawingTechOperationParams();
            //var techOperationFactory = new SawingTechOperationFactory(techProcessParams, sawingLineTechOperationParamsDefault, sawingArcTechOperationParamsDefault);

            //var acad = new AcadGateway();
            //var createTechOperationCommand = new CreateTechOperationCommand(context, techOperationFactory, treeNodeCollection, acad.GetSelectedCurves);
            //var createTechProcessCommand = new CreateTechProcessCommand(context, techProcessList, treeNodeCollection, createTechOperationCommand);

            //createTechProcessCommand.Execute();
            //createTechOperationCommand.Execute();

            //var removeTechOperationCommand = new RemoveTechOperationCommand(context, techProcessList, treeNodeCollection);
            //var moveUpTechOperationCommand = new MoveUpTechOperationCommand(context, treeNodeCollection);
            //var moveDownTechOperationCommand = new MoveDownTechOperationCommand(context, treeNodeCollection);
            //var selectTechOperationCommand = new SelectTechOperationCommand(context, techProcessView.treeView);
            //var setTechOperationCommand = new SetTechOperationCommand(context, techProcessList, techProcessView.treeView);

            //techProcessView.RegisterCommands(createTechProcessCommand, createTechOperationCommand, removeTechOperationCommand, moveUpTechOperationCommand, moveDownTechOperationCommand,
            //    selectTechOperationCommand, setTechOperationCommand);

            //TreeNode[] nodes = new TreeNode[1];
            //nodes[0] = new TreeNode("Изделие1", new TreeNode[2] { new TreeNode("Распил по прямой"), new TreeNode("Распил по дуге") });

            //techProcessView.treeView.Nodes.AddRange(nodes);
        }
    }
}
