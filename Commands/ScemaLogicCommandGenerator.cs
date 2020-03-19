﻿using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace CAM.Commands
{
    /// <summary>
    /// Генератор команд для станка типа ScemaLogic
    /// </summary>
    [MachineType(MachineType.ScemaLogic)]
    public class ScemaLogicCommandGenerator : CommandGeneratorBase
    {
        protected override void StartMachineCommands(string caption)
        {
            //Command($"; ScemaLogic \"{caption}\"");
            //Command($"; DATE {DateTime.Now}");

            Command("98;;;;;;;;");
            Command("97;2;;1;;;;;");
            Command("17;;XYCZ;;;;;;");
            Command("28;;XYCZ;;;;;;");
        }

        protected override void StopMachineCommands()
        {
            Command("97;30;;;;;;;", "Конец");
        }

        protected override void SetToolCommands(int toolNo, double angleA = 0)
        {
            Command($"97;6;;{toolNo};;;;;", "Инструмент№");
        }

        protected override void StartEngineCommands()
        {
            Command("97;7;;;;;;;", "Охлаждение");
            Command("97;8;;;;;;;", "Охлаждение");
            Command($"97;3;;{_frequency};;;;;", "Шпиндель");
            //CreateCommand(CommandNames.Cycle, 28, axis: "XYCZ");
        }

        protected override void StopEngineCommands()
        {
            Command("97;9;;;;;;;", "Охлаждение откл.");
            Command("97;10;;;;;;;");
            Command("97;5;;;;;;;", "Шпиндель откл.");
        }

        protected override string GCommandText(int gCode, string paramsString, Point3d point, Curve curve, double? angleC, double? angleA, int? feed)
        {
            var text = $"{point.X.Round(4)}; {point.Y.Round(4)}; ";
            if (gCode == 0)
                return angleC.HasValue ? $"0; XYC; {text} {angleC.Value.Round(4)};;" : $"0; XYZ; {text}{point.Z.Round(4)};;";

            if (curve is Arc arc)
            {
                gCode = point == arc.EndPoint ? 2 : 3;
                text += $"{(arc.Center.X - _originX).Round(4)}; {(arc.Center.Y - _originY).Round(4)};";
            }
            else
                text += $"{point.Z.Round(4)};;";

            return $"{gCode}; XYCZ; {feed ?? _feed}; {text}";
        }
    }
}
