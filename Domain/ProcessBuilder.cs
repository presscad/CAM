﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;

namespace CAM.Domain
{
    /// <summary>
    /// Генератор команд процесса обработки
    /// </summary>
    public class ScemaLogicProcessBuilder
    {
        //private static Dictionary<string, string> CommandCodes =
        //{
        //    [
        //}

	    private readonly TechProcessParams _techProcessParams;
	    private readonly Curve _curve;
	    private Point3d _currentPoint;
	    private Corner _corner;
	    private double _startIndent;
	    private double _endIndent;
	    private double _compensation;

	    public List<ProcessCommand> Commands { get; } = new List<ProcessCommand>();

	    public ScemaLogicProcessBuilder(TechProcessParams techProcessParams, Point3d startPoint, Curve curve, Corner corner = Corner.Start)
        {
	        _techProcessParams = techProcessParams;
	        _currentPoint = startPoint;
	        _curve = curve;
	        _corner = corner;
        }

	    private static Curve CreateToolpathCurve(Curve curve, double offset, double z, double startIndent, double endIndent)
	    {
		    var toolpathCurve = curve.GetOffsetCurves(offset, z);

			switch (curve)
		    {
				case Line line:
					line.StartPoint = line.GetPointAtDist(startIndent);
					line.EndPoint = line.GetPointAtDist(line.Length - endIndent);
					break;

			    case Arc arc:
					    arc.StartAngle = arc.StartAngle + startIndent / arc.Radius;
					    arc.EndAngle = arc.EndAngle - endIndent / arc.Radius;
				    break;
		    }

		    return toolpathCurve;
	    }

	    //private double[] CalcToolAngles(Curve curve)
     //   {
     //       var angles = new double[2];
     //       switch (curve)
     //       {
     //           case Line line:
     //               angles[0] = angles[1] = ToDeg((Math.PI * 2 - line.Angle) % Math.PI);
     //               break;
     //           case Arc arc:
					//if (arc.StartAngle < Math.PI / 2 && arc.EndAngle > Math.PI / 2 || arc.StartAngle < Math.PI * 3 / 2 && arc.EndAngle > Math.PI * 3 / 2)
					//	throw new Exception($"Обработка дуги невозможна - дуга пересекает угол 90 или 270 градусов. Текущие углы: начальный {ToDeg(arc.StartAngle)}, конечный {ToDeg(arc.EndAngle)}");

					//angles[(int)Corner.Start] = (arc.StartAngle - Math.PI / 2) % Math.PI == 0 ? 180 : Calc(arc.StartAngle);
     //               angles[(int)Corner.End] = Calc(arc.EndAngle);
     //               break;
     //           default:
     //               throw new Exception($"Неподдерживаемый тип кривой {curve.GetType()}");
     //       }
     //       return angles;

     //       double Calc(double angle) => ToDeg((Math.PI * 2.5 - angle) % Math.PI);
     //   }

	    private static double CalcToolAngle(Curve curve, Corner corner)
        {
			//TODO Обработка дуги невозможна
			//if (arc.StartAngle < Math.PI / 2 && arc.EndAngle > Math.PI / 2 || arc.StartAngle < Math.PI * 3 / 2 && arc.EndAngle > Math.PI * 3 / 2)
			//	throw new Exception($"Обработка дуги невозможна - дуга пересекает угол 90 или 270 градусов. Текущие углы: начальный {ToDeg(arc.StartAngle)}, конечный {ToDeg(arc.EndAngle)}");

			switch (curve)
            {
                case Line line:
                    return ToDeg((Math.PI * 2 - line.Angle) % Math.PI);
                case Arc arc:
	                return corner == Corner.Start
		                ? (arc.StartAngle - Math.PI / 2) % Math.PI == 0 ? 180 : Calc(arc.StartAngle)
		                : Calc(arc.EndAngle);
                default:
                    throw new Exception($"Неподдерживаемый тип кривой {curve.GetType()}");
            }
            double Calc(double angle) => ToDeg((Math.PI * 2.5 - angle) % Math.PI);
        }

	    //public void SetTool(double x, double y, double c)
	    //{
	    //    string name = _currentPoint == Point3d.Origin ? CommandNames.InitialMove : CommandNames.Fast;
	    //    Line line = null;
	    //    var z = _techProcessParams.ZSafety;
	    //    var destPoint = new Point3d(x, y, z);
	    //    if (_currentPoint != Point3d.Origin)
	    //    CreateCommand(name, "0", line, "XYC", 0, x, y, c);
	    //    CreateCommand(name, "0", line, "XYZ", 0, x, y, z);
	    //    if (_currentPoint == Point3d.Origin)
	    //    {
	    //        name = "Setup";
	    //        CreateCommand(name, "97;7");
	    //        CreateCommand(name, "97;8");
	    //        CreateCommand(name, "97;3", feed: _techProcessParams.Frequency);
	    //    }
	    //    else
	    //    {
	    //        line = new Line(_currentPoint, destPoint);
	    //        line.ObjectId = new ObjectId(name);
	    //        Entities.Add(line);
	    //    }
	    //    CreateCommand("Setup", "28", axis: "XYCZ");  // цикл
	    //    _currentPoint = destPoint;
	    //}

	    private void CreateCommand(string name, string code, Curve toolpathAcadObject= null, string axis = null, int? feed = null, double? param3 = null, double? param4 = null, double? param5 = null, double? param6 = null) 
            => Commands.Add(new ProcessCommand(name, code, toolpathAcadObject, axis, feed?.ToString(), GetParam(param3), GetParam(param4), GetParam(param5), GetParam(param6)));

	    private static string GetParam(double? value, string paramName = null) => value.HasValue ? $" {paramName}{value:0.####}" : null;

	    /// <summary>
	    /// Опускание
	    /// </summary>
	    /// <param name="z"></param>
	    //public void Descent(double z) => MoveZ(CommandNames.Descent, z, _techProcessParams.PenetrationRate);

	    /// <summary>
	    /// Поднятие
	    /// </summary>
	    public List<ProcessCommand> Completion()
	    {
		    Move(CommandNames.Uplifting, "0", "XYZ", 0, z: _techProcessParams.ZSafety);
		    return Commands;
	    }

	    /// <summary>
        /// Заглубление
        /// </summary>
        /// <param name="z"></param>
        //public void Penetration(double z) => Move(CommandNames.Penetration, "1", "XYCZ", _techProcessParams.PenetrationRate, z: z);

	    private void Move(string name, string code, string axis, int feed, double? x = null, double? y = null, double? c = null, double? z = null)
	    {
		    // TODO проверка совпадения точек
		    var destPoint = new Point3d(x ?? _currentPoint.X, y ?? _currentPoint.Y, z ?? _currentPoint.Z);
		    var line = _currentPoint != Point3d.Origin
			    ? new Line(_currentPoint, destPoint) {ObjectId = new ObjectId(name)}
			    : null;
		    CreateCommand(name, code, line, axis, feed, destPoint.X, destPoint.Y, destPoint.Z);
		    _currentPoint = destPoint;
	    }

	    /// <summary>
        /// Рабочий ход
        /// </summary>
        /// <param name="z"></param>
        /// <param name="offset"></param>
        /// <param name="feed"></param>
        public void Cutting(int feed, double z = 0, double offset = 0)
        {
	        var toolpathCurve = CreateToolpathCurve(_curve, offset + _compensation, z, _startIndent, _endIndent);
	        if (!Commands.Any())
		        InitMove(toolpathCurve);

	        Move(CommandNames.Penetration, "1", "XYCZ", _techProcessParams.PenetrationRate, toolpathCurve.GetPoint(_corner).X, toolpathCurve.GetPoint(_corner).Y, z: z);

			toolpathCurve.ObjectId = new ObjectId($"{_curve.ObjectId.Key} обработка");
	        _corner = _corner.Swap();
            _currentPoint = toolpathCurve.GetPoint(_corner);

            switch (toolpathCurve)
            {
                case Line _:
                    CreateCommand(CommandNames.Cutting, "1", toolpathCurve, "XYCZ", feed, _currentPoint.X, _currentPoint.Y, CalcToolAngle(toolpathCurve, _corner), _currentPoint.Z);
                    break;
                case Arc arc:
                    var code = _corner == Corner.Start ? "3" : "2"; // TODO ?
                    CreateCommand(CommandNames.Cutting, code, toolpathCurve, "XYCZ", feed, _currentPoint.X, _currentPoint.Y, arc.Center.X, arc.Center.Y);
                    break;
                default:
                    throw new Exception($"Неподдерживаемый тип кривой {toolpathCurve.GetType()}");
            }
        }

	    private void InitMove(Curve toolpathCurve)
	    {
		    var name = _currentPoint == Point3d.Origin ? CommandNames.InitialMove : CommandNames.Fast;
		    var destPoint = _corner == Corner.Start ? toolpathCurve.StartPoint : toolpathCurve.EndPoint;

		    CreateCommand(name, "0", null, "XYC", 0, destPoint.X, destPoint.Y, CalcToolAngle(toolpathCurve, _corner));
		    Move(name, "0", "XYZ", 0, destPoint.X, destPoint.Y, _techProcessParams.ZSafety);

		    if (name == CommandNames.InitialMove)
		    {
			    name = "Setup";
			    CreateCommand(name, "97;7");
			    CreateCommand(name, "97;8");
			    CreateCommand(name, "97;3", feed: _techProcessParams.Frequency);
		    }
		    CreateCommand("Setup", "28", axis: "XYCZ");  // цикл
	    }

		private static double ToDeg(double angle) => angle * 180 / Math.PI;

	    public double CalcCompensation(Side outerSide, double depth)
	    {
		    var hasToolOffset = false;
		    var d = 0D;

		    switch (_curve)
		    {
			    case Line line:
					hasToolOffset = (line.Angle > 0 && line.Angle <= Math.PI) ^ (outerSide == Side.Left);
					break;
			    case Arc arc:
				    hasToolOffset = (arc.StartAngle >= 0.5 * Math.PI && arc.StartAngle < 1.5 * Math.PI) ^ (outerSide == Side.Right);
				    if (outerSide == Side.Left)
					    d = arc.Radius - Math.Sqrt(arc.Radius * arc.Radius - depth * (_techProcessParams.Tool.Diameter - depth));
				    break;
		    }
		    double sign = outerSide == Side.Left ^ _curve is Arc ? 1 : -1;

		    return _compensation = sign * ((hasToolOffset ? _techProcessParams.Tool.Thickness : 0) + d);
	    }

	    public double CalcIndent(bool isExactlyBegin, bool isExactlyEnd, int depth)
	    {
			const int cornerIndentIncrease = 5;

			var indent = Math.Sqrt(depth * (_techProcessParams.Tool.Diameter - depth)) + cornerIndentIncrease;
		    _startIndent = isExactlyBegin ? indent : 0;
		    _endIndent = isExactlyEnd ? indent : 0;

		    return _startIndent + _endIndent;
	    }
    }


    ///// <summary>
    ///// Генератор команд процесса обработки
    ///// </summary>
    //public class ProcessBuilder
    //{
    //    //private static Dictionary<string, string> CommandCodes =
    //    //{
    //    //    [
    //    //}

    //    private Point3d _currentPoint = Point3d.Origin;
    //    private int _currentFeed;
    //    private TechProcessParams _techProcessParams;

    //    public List<Curve> Entities { get; } = new List<Curve>();

    //    public List<ProcessCommand> Commands { get; } = new List<ProcessCommand>();

    //    public ProcessBuilder(TechProcessParams techProcessParams)
    //    {
    //        _techProcessParams = techProcessParams;
    //    }

    //    private void Move(string name = null, string code, double? x = null, double? y = null, double? z = null, double? c = null, int? feed = null)
    //    {
    //        var destPoint = new Point3d(x ?? _currentPoint.X, y ?? _currentPoint.Y, z ?? _currentPoint.Z);
    //        // TODO проверка совпадения точек
    //        var line = new Line(_currentPoint, destPoint);
    //        line.ObjectId = new ObjectId("перемещение");
    //        Commands.Add(new ProcessCommand(name, code, line, "XYCZ", GetParam(x), GetParam(y), GetParam(c), GetParam(z)));
    //        _currentPoint = destPoint;
    //        Entities.Add(line);
    //    }

    //    private void Set(string name = null, double? x = null, double? y = null, double? z = null, double? c = null, int? feed = null)
    //    {
    //        var destPoint = new Point3d(x ?? _currentPoint.X, y ?? _currentPoint.Y, z ?? _currentPoint.Z);
    //        // TODO проверка совпадения точек
    //        var line = new Line(_currentPoint, destPoint);
    //        line.ObjectId = new ObjectId("перемещение");
    //        Commands.Add(new ProcessCommand(name, "0", line, "XYC", "0", GetParam(x), GetParam(y), GetParam(c)));
    //        Commands.Add(new ProcessCommand(name, "0", line, "XYZ", "0", GetParam(x), GetParam(y), GetParam(z)));
    //        _currentPoint = destPoint;
    //        _currentFeed = feed ?? _currentFeed;
    //        Entities.Add(line);
    //    }

    //    //private void Move(string name = null, double? x = null, double? y = null, double? z = null, int? feed = null)
    //    //{
    //    //    var destPoint = new Point3d(x ?? _currentPoint.X, y ?? _currentPoint.Y, z ?? _currentPoint.Z);
    //    //    // TODO проверка совпадения точек
    //    //    var line = new Line(_currentPoint, destPoint);
    //    //    line.ObjectId = new ObjectId("перемещение");
    //    //    //var par = new List<string>();
    //    //    //if (x.HasValue) par.Add($"X{x:0.####}");
    //    //    //if (y.HasValue) par.Add($"Y{y:0.####}");
    //    //    //if (z.HasValue) par.Add($"Z{z:0.####}");
    //    //    //if (feed.HasValue) par.Add($"F{feed:0.####}");
    //    //    //            Commands.Add(new ProcessCommand(name, "G1", line, par.ToArray()));
    //    //    //            Commands.Add(new ProcessCommand(name, CommandCode.G1, line, GetParam("X", x), GetParam("Y", y), GetParam("Z", z), GetParam("F", feed)));
    //    //    Commands.Add(new ProcessCommand(name, CommandCode.G1, line, GetParam("X", x), GetParam("Y", y), GetParam("Z", z), GetParam("F", feed)));
    //    //    Commands.Add(new ProcessCommand("XYC", point[vertex.Index()].X, point[vertex.Index()].Y, angle[vertex.Index()], obj.ToString());
    //    //    Commands.Add(new ProcessCommand("XYZ", point[vertex.Index()].X, point[vertex.Index()].Y, ProcessOptions.ZSafety, obj.ToString());
    //    //    _currentPoint = destPoint;
    //    //    _currentFeed = feed ?? _currentFeed;
    //    //    Entities.Add(line);
    //    //}

    //    private string GetParam(double? value, string paramName = null) => value.HasValue ? $" {paramName}{value:0.####}" : null;

    //    //private void MoveZ(string name, double z, int feed = 0) => Move(name: name, z: z, feed: feed);

    //    /// <summary>
    //    /// Быстрая подача
    //    /// </summary>
    //    /// <param name="x"></param>
    //    /// <param name="y"></param>
    //    public void Move(double x, double y)
    //    {
    //        if (_currentPoint == Point3d.Origin)
    //        {
    //            _currentPoint = new Point3d(x, y, _techProcessParams.ZSafety);
    //            Move(CommandNames.InitialMove, x, y));
    //        }
    //        else
    //            MoveXYZ(x, y, _currentPoint.Z, CommandNames.Fast);
    //    }

    //    /// <summary>
    //    /// Опускание
    //    /// </summary>
    //    /// <param name="z"></param>
    //    public void Descent(double z) => MoveZ(CommandNames.Descent, z);

    //    /// <summary>
    //    /// Поднятие
    //    /// </summary>
    //    public void Uplifting() => MoveZ(CommandNames.Uplifting, _techProcessParams.ZSafety);

    //    /// <summary>
    //    /// Заглубление
    //    /// </summary>
    //    /// <param name="z"></param>
    //    public void Penetration(double z) => MoveZ(CommandNames.Penetration, z, _techProcessParams.PenetrationRate);

    //    /// <summary>
    //    /// Рабочий ход
    //    /// </summary>
    //    /// <param name="curve"></param>
    //    /// <param name="dz"></param>
    //    /// <param name="compensation"></param>
    //    /// <param name="startIndent"></param>
    //    /// <param name="endIndent"></param>
    //    /// <param name="feed"></param>
    //    public void Cutting(Curve curve, double dz = 0, double compensation = 0, double startIndent = 0, double endIndent = 0, int feed = 0)
    //    {
    //        var toolpathCurve = curve.GetOffsetCurves(dz);
    //        toolpathCurve.ObjectId = new ObjectId($"{curve.ObjectId.Key} обработка");
    //        if (toolpathCurve.StartPoint == _currentPoint)
    //            _currentPoint = toolpathCurve.EndPoint;
    //        else if (toolpathCurve.EndPoint == _currentPoint)
    //            _currentPoint = toolpathCurve.StartPoint;
    //        else
    //            throw new Exception("Несоответствие текущей позиции и рассчитанной траектории");

    //        Commands.Add(new ProcessCommand(CommandNames.Cutting, CommandCode.G1, toolpathCurve));
    //        _currentPoint = destPoint;
    //        _currentFeed = feed ?? _currentFeed;
    //        Entities.Add(line);

    //        ProcessActions.Add(new ProcessAction($"{CommandNames.Cutting} F{feed}", _groupName, toolpathCurve, feed));
    //        Entities.Add(toolpathCurve);
    //    }
    //}

}