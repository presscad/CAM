﻿using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Dreambuild.AutoCAD;
using System;
using System.Collections.Generic;

namespace CAM
{
    /// <summary>
    /// Генератор команд для станка типа Krea
    /// </summary>
    public class DonatoniCommandGenerator
    {
        private int _startRangeIndex;
        private const int UpperZ = 100;
        public ToolInfo ToolInfo;
        private int _GCode;
        private int _feed;

        private readonly Dictionary<string, Color> Colors = new Dictionary<string, Color>()
        {
            [CommandNames.Cutting] = Color.FromColor(System.Drawing.Color.Green),
            [CommandNames.Uplifting] = Color.FromColor(System.Drawing.Color.Blue),
            [CommandNames.Penetration] = Color.FromColor(System.Drawing.Color.Yellow),
            [CommandNames.Fast] = Color.FromColor(System.Drawing.Color.Crimson),
            [CommandNames.Transition] = Color.FromColor(System.Drawing.Color.Yellow)
        };

        public List<ProcessCommand> Commands { get; } = new List<ProcessCommand>();

        public Point3d ToolPosition => ToolInfo.Point;

        public bool EngineStarted => ToolPosition.Z < UpperZ;

        public void StartRange() => _startRangeIndex = Commands.Count;

        public List<ProcessCommand> GetRange() => Commands.GetRange(_startRangeIndex, Commands.Count - _startRangeIndex);

        /// <summary>
        /// Запуск станка
        /// </summary>
        /// <param name="toolNumber"></param>
        public void StartMachine(string caption, int toolNumber)
        {
            ToolInfo.Point = new Point3d(0, 0, UpperZ);

            CreateCommand($"; Donatoni \"{caption}\"");
            CreateCommand($"; DATE {DateTime.Now}");

            //if (Settings.Machine == MachineKind.Krea)
            //CreateCommand("(UAO,E30)");
            //CreateCommand("(UIO,Z(E31))");

            CreateCommand("%300");
            CreateCommand("RTCP=1");
            CreateCommand("G600 X0 Y-2500 Z-370 U3800 V0 W0 N0");
            CreateCommand("G601");
            CreateCommand("G0 G53 Z0.0");
            CreateCommand("G0 G53 A0");
            CreateCommand("G64");
            CreateCommand("G154O10");
        }

        internal void SetTool(int toolNo)
        {
            if (ToolInfo.Index != toolNo)
            {
                ToolInfo.Index = toolNo;
                CreateCommand($"T{toolNo}");
                CreateCommand("M6");
                CreateCommand("G172 T1 H1 D1");
                CreateCommand("M300");
            }
        }

        /// <summary>
        /// Остановка станка
        /// </summary>
        public void StopMachine()
        {
            CreateCommand("M5");              // выключение шпинделя                       
            //if (Settings.Machine == MachineKind.Krea)
            //{
            //    AddLine("M9 M10");          // выключение воды
            //    AddLine("G0 G79 Z(@ZUP)");  // подъем в верхнюю точку
            //}
            CreateCommand("M9");
            CreateCommand("G61");
            CreateCommand("G153");
            CreateCommand(";G0 G53 Z0");
            CreateCommand("SETMSP=1");
            CreateCommand("G0 G53 Z0 ");
            CreateCommand("G0 G53 A0 C0");
            CreateCommand("G0 G53 X0 Y0");
            CreateCommand("M30");

            int number = 0;
            Commands.ForEach(p => p.Number = ++number);
        }

        /// <summary>
        /// Первый подвод
        /// </summary>
        /// <param name="point"></param>
        /// <param name="angleC"></param>
        /// <param name="frequency"></param>
        public void InitialMove(double x, double y, double z, double angleC, int frequency) //, int feed)
        {
            CreateCommand("T1");
            CreateCommand("M6");
            CreateCommand("G172 T1 H1 D1");
            CreateCommand("M300");
            //G17 плоскость

            CreateCommand(CommandNames.InitialMove, 0, x: x, y: y, angleC: angleC);
            CreateCommand(CommandNames.InitialMove, 0, z: z);

            //CreateCommand($"G1 A0 F{feed}");
            //CreateCommand($"G1 Z{Round(point.Z)} F{feed}");

            CreateCommand("M8"); // M7 ?
            CreateCommand($"M3 S{frequency}");
        }

        /// <summary>
        /// Быстрая подача
        /// </summary>
        /// <param name="line"></param>
        /// <param name="angle"></param>
        //public void Fast(Line line, double angleC, double angleA = 0)
        //{
        //    CreateGCommand(CommandNames.Fast, 0, "XYZCA", line.EndPoint, angleC, line, angleA: angleA);
        //    //CreateCommand($"G0 Z{Round(line.EndPoint.Z)}", endPoint: line.EndPoint, toolAngle: angleC);
        //    //CreateCommand(CommandNames.Fast, 0, toolpathCurve: line, endPoint: line.EndPoint, toolAngle: angleC);
        //}

        //public void CommandG0(string name, Point3d? point = null, double? x = null, double? y = null, double? z = null, double? angleC = null, double? angleA = null)
        //{
        //    CreateGCommand(name, 0, "XYZCA", point: point, x: x, y: y, z: z, angleC: angleC, angleA: angleA);
        //}

        //public void CommandG1(string name, int feed, Point3d? point = null, double? x = null, double? y = null, double? z = null, double? angleC = null, double? angleA = null, Curve curve = null)
        //{
        //    CreateGCommand(name, 1, "XYZCA", point: point, x: x, y: y, z: z, angleC: angleC, angleA: angleA, curve: curve, feed: feed);
        //}

        /// <summary>
        /// Поднятие
        /// </summary>
        /// <param name="line"></param>
        //public void Uplifting(Line line, double angle) => CreateGCommand(CommandNames.Uplifting, 0, "XYZC", line.EndPoint, angle, line);

        /// <summary>
        /// Рез
        /// </summary>
        //public void Cutting(string name, Curve curve, int feed, Point3d point, double angle)
        //{
        //    switch (curve)
        //    {
        //        case Line _:
        //            CreateGCommand(name, 1, "XYZCF", point, angle, curve, feed);
        //            break;
        //        case Arc arc:
        //            var code = point == curve.StartPoint ? 2 : 3;
        //            CreateGCommand(name, code, "XYZCF", point, angle, curve, feed);
        //            break;
        //        default:
        //            throw new Exception($"Неподдерживаемый тип кривой {curve.GetType()}");
        //    }
        //}

        //private void CreateCommand(string name, int gCode, int? mCode = null, string axis = null, int? feed = null, double? x = null, double? y = null,
        //    double? param1 = null, double? param2 = null, Curve toolpathCurve = null, Point3d? endPoint = null, double? toolAngle = null)
        //    => Commands.Add(new ProcessCommand
        //    {
        //        Name = name,
        //        GCode = gCode.ToString(),
        //        MCode = mCode?.ToString(),
        //        Axis = axis,
        //        Feed = feed.ToString(),
        //        X = Round(x),
        //        Y = Round(y),
        //        Param1 = Round(param1),
        //        Param2 = Round(param2),
        //        ToolpathCurve = toolpathCurve,
        //        EndPoint = endPoint,
        //        ToolAngle = toolAngle
        //    });

        public void CreateCommand(string text, string name = null) => Commands.Add(new ProcessCommand
        {
            Name = name,
            Text = text,
            ToolInfo = ToolInfo
        });

        //private CommandParams _commandParams = new CommandParams();

        public void CreateCommand(string name, int gCode, string paramsString = null, Point3d? point = null, double? x = null, double? y = null, double? z = null, 
            double? angleC = null, double? angleA = null, Curve curve = null, int? feed = null)
        {
            if (point == null)
                point = new Point3d(x ?? ToolInfo.Point.X, y ?? ToolInfo.Point.Y, z ?? ToolInfo.Point.Z);

            var commandText = $"G{gCode}{Format("X", point.Value.X, ToolInfo.Point.X)}{Format("Y", point.Value.Y, ToolInfo.Point.Y)}{Format("Z", point.Value.Z, ToolInfo.Point.Z)}" +
                $"{Format("C", angleC, ToolInfo.AngleC)}{Format("A", angleA, ToolInfo.AngleA)}{Format("F", feed, _feed)}";

            if (name != CommandNames.InitialMove)
            {
                if (curve == null && (point.Value - ToolInfo.Point).Length > 1)
                    curve = NoDraw.Line(ToolInfo.Point, point.Value);
                if (curve != null)
                    curve.Color = Colors[name];
            }

            _GCode = gCode;
            ToolInfo.Set(point.Value, angleC, angleA);
            _feed = feed ?? _feed;

            Commands.Add(new ProcessCommand
            {
                Name = name,
                Text = commandText,
                ToolpathCurve = curve,
                ToolInfo = ToolInfo
            });

            string Format(string label, double? value, double oldValue) =>
                 (paramsString == null || paramsString.Contains(label)) && (value.GetValueOrDefault(oldValue) != oldValue || (_GCode != gCode))
                        ? $" {label}{value.GetValueOrDefault(oldValue).Round(4)}"
                        : null;
        }
    }
}

    //private string Format(string @params, string label, double value, ref double oldValue)
    //{
    //    var text = @params.Contains(label) && value != oldValue ? $" {label}{value.Round(4)}" : "";
    //    oldValue = value;
    //    return text;
    //}

    //private void CreateCommand(string name, int gCode, int? feed = null, Curve toolpathCurve = null, Point3d? endPoint = null, double? toolAngle = null) => Commands.Add(new ProcessCommand
    //{
    //    Name = name,
    //    Text = $"G{gCode}{Format('X', endPoint?.X)}{Format('Y', endPoint?.Y)}{Format('Z', endPoint?.Z)}{Format('F', feed)}",
    //    ToolpathCurve = toolpathCurve,
    //    EndPoint = endPoint,
    //    ToolAngle = toolAngle
    //});

    //private static string Format(char label, double? value) => value.HasValue ? $" {label}{Math.Round(value.Value, 4)}" : String.Empty;

    //private static double Round(double value) => Math.Round(value, 4);

    //class CommandParams
    //{
    //    int _g;
    //    double _x = double.NaN, _y = double.NaN, _z = double.NaN, _c = double.NaN, _a = double.NaN, _f = double.NaN;

    //    public string GetParams(int gCode, string @params, ToolPosition tool, double feed)
    //    {
    //        var reset = gCode != _g;
    //        _g = gCode;

    //        return $"G{gCode}{Format("X", tool.Point.X, ref _x)}{Format("Y", tool.Point.Y, ref _y)}{Format("Z", tool.Point.Z, ref _z)}{Format("C", tool.AngleC, ref _c)}{Format("A", tool.AngleA, ref _a)}{Format("F", feed, ref _f)}";

    //        string Format(string label, double value, ref double oldValue)
    //        {
    //            if (@params.Contains(label) && (value != oldValue || reset))
    //            {
    //                oldValue = value;
    //                return $" {label}{value.Round(4)}";
    //            }
    //            return null;
    //        }
    //    }
    //}
//}