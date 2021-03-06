﻿using Autodesk.AutoCAD.DatabaseServices;
using Dreambuild.AutoCAD;
using System;
using System.Collections.Generic;
using System.Linq;
using DbSurface = Autodesk.AutoCAD.DatabaseServices.Surface;
using Autodesk.AutoCAD.Geometry;

namespace CAM.Disk3D
{
    [Serializable]
    [TechOperation(1, TechProcessNames.Disk3D, "Гребенка")]
    public class CombTechOperation : TechOperationBase
    {
        public double StepPass { get; set; }

        public double StartPass { get; set; }

        public double Penetration { get; set; }

        public double Delta { get; set; }

        public double StepLong { get; set; }

        public int CuttingFeed { get; set; }

        public double Departure { get; set; }

        public bool IsDepartureOnBorderSection { get; set; }

        public CombTechOperation(ITechProcess techProcess, string caption) : base(techProcess, caption)
        {
        }

        public override void BuildProcessing(ICommandGenerator generator)
        {
            var disk3DTechProcess = (Disk3DTechProcess)TechProcess;

            var offsetSurface = CreateOffsetSurface();

            var matrix = Matrix3d.Rotation(disk3DTechProcess.Angle.ToRad(), Vector3d.ZAxis, Point3d.Origin);
            if (disk3DTechProcess.Angle != 0)
                offsetSurface.TransformBy(matrix);
            var bounds = offsetSurface.GeometricExtents;
            generator.ZSafety = bounds.MinPoint.Z + TechProcess.Thickness.Value + TechProcess.ZSafety;
            generator.ToolLocation.Point += Vector3d.ZAxis * generator.ZSafety;


            //var ray = new Ray { UnitDir = Vector3d.XAxis };
            //var y = StartPass;
            //do
            //{
            //    ray.BasePoint = new Point3d(0, y, 0);

            //    var curves = s.ProjectOnToSurface(point, Vector3d.ZAxis);
            //    curves[0].ColorIndex = 2;
            //    curves[0].AddToCurrentSpace();
            //    y += StepPass;
            //}
            //while (y < boundsModel.MaxPoint.Y);

            //var startTime = System.Diagnostics.Stopwatch.StartNew();
            //var point = new DBPoint();
            //int cnt = 0;
            //for (var y = boundsModel.MinPoint.Y + StartPass; y < boundsModel.MaxPoint.Y; y += StepPass)
            //{
            //    for (var x = boundsModel.MinPoint.X; x < boundsModel.MaxPoint.X; x += StepLong)
            //    {
            //        for (var s = 0; s <= 2; s += 4)
            //        {
            //            point.Position = new Point3d(x, y + s, 0);

            //            //line.StartPoint = new Point3d(x, y, 0);
            //            //line.EndPoint = new Point3d(x, y + 8, 0);

            //            try
            //            {
            //                var curves = offsetSurfaces.ProjectOnToSurface(point, Vector3d.ZAxis);
            //                cnt++;
            //                if (curves.Length == 0)
            //                    Acad.Write("111");
            //                curves[0].ColorIndex = 6;
            //                curves[0].AddToCurrentSpace();
            //            }
            //            catch
            //            {
            //            }
            //        }
            //    }
            //}
            //startTime.Stop();
            //var resultTime = startTime.Elapsed;
            //Acad.Write("timer=" + resultTime);
            //Acad.Write("cnt=" + cnt);


            //var startTime = System.Diagnostics.Stopwatch.StartNew();
            //var line = new Line();
            //int cnt = 0;
            //int err = 0;
            //for (var y = boundsModel.MinPoint.Y + StartPass; y < boundsModel.MaxPoint.Y; y += StepPass)
            //{
            //    for (var x = boundsModel.MinPoint.X; x < boundsModel.MaxPoint.X; x += StepLong)
            //    {
            //        for (var s = 0; s <= 2; s += 4)
            //        {
            //            line.StartPoint = new Point3d(x, y, 0);
            //            line.EndPoint = new Point3d(x, y + 8, 0);

            //            try
            //            {
            //                var curves = offsetSurfaces.ProjectOnToSurface(line, Vector3d.ZAxis);
            //                cnt++;
            //                if (curves.Length == 0)
            //                    Acad.Write("111");
            //                curves[0].ColorIndex = 6;
            //                curves[0].AddToCurrentSpace();
            //            }
            //            catch (Exception ex)
            //            {
            //                err++;
            //            }
            //        }
            //    }
            //}
            //startTime.Stop();
            //var resultTime = startTime.Elapsed;
            //Acad.Write("timer=" + resultTime);
            //Acad.Write("cnt=" + cnt);

            //var contour = NoDraw.Rectang(new Point3d(bounds.MinPoint.X, bounds.MinPoint.Y, 0), new Point3d(bounds.MaxPoint.X, bounds.MaxPoint.Y, 0));
            //var ray = new Ray
            //{
            //    BasePoint = contour.StartPoint,
            //    UnitDir = Vector3d.XAxis.RotateBy(((Disk3DTechProcess)TechProcess).Angle.ToRad(), Vector3d.ZAxis)
            //};
            //ray.BasePoint += ray.UnitDir.GetPerpendicularVector() * StartPass;
            //var crossVector = ray.UnitDir.GetPerpendicularVector() * StepPass;
            //var plane = new Plane();
            //while (true)
            //{
            //    var pc = new Point3dCollection();
            //    ray.IntersectWith(contour, Intersect.ExtendThis, plane, pc, IntPtr.Zero, IntPtr.Zero);
            //    if (pc.Count != 2)
            //        break;

            //        var vector = (points[1] - points[0]).GetNormal() * tactileTechProcess.TactileTechProcessParams.Departure;
            //        var startPoint = points[0] + passDir * s - vector - Vector3d.ZAxis * Depth;
            //        var endPoint = points[1] + passDir * s + vector - Vector3d.ZAxis * Depth;
            //        if (generator.IsUpperTool)
            //            generator.Move(startPoint.X, startPoint.Y, angleC: BuilderUtils.CalcToolAngle(ProcessingAngle.ToRad()));
            //        generator.Cutting(startPoint, endPoint, feed, tactileTechProcess.TactileTechProcessParams.TransitionFeed);

            //    ray.BasePoint += crossVector;
            //}

            var PassList = new List<List<Point3d>>();
            for (var y = bounds.MinPoint.Y + StartPass; y < bounds.MaxPoint.Y - (disk3DTechProcess.IsExactlyEnd ? TechProcess.Tool.Thickness : 0); y += StepPass)
            {
                var points = new Point3dCollection();
                for (var x = bounds.MinPoint.X; x <= bounds.MaxPoint.X; x += StepLong)
                {
                    double z = 0;
                    for (var s = 0; s <= TechProcess.Tool.Thickness; s += 1)
                    {
                        if (y + s > bounds.MaxPoint.Y)
                            break;
                        offsetSurface.RayTest(new Point3d(x, y + s, bounds.MinPoint.Z), Vector3d.ZAxis, 0.0001, out SubentityId[] col, out DoubleCollection par);
                        if (par.Count == 0)
                        {
                            z = 0;
                            break;
                        }
                        z = par[par.Count - 1] > z ? par[par.Count - 1] : z;
                    }
                    if (z > 0)
                    {
                        var point = new Point3d(x, y, bounds.MinPoint.Z + z);
                        var ind = points.Count - 1;
                        if (ind > 0 && points[ind - 1].GetVectorTo(points[ind]).IsCodirectionalTo(points[ind].GetVectorTo(point)))
                            points[ind] = point;
                        else
                            points.Add(point);
                    }
                }
                if (points.Count > 1)
                    PassList.Add(CalcOffsetPoints(points, bounds));
            }
            offsetSurface.Dispose();

            matrix = matrix.Inverse();
            PassList.ForEach(p =>
            {
                var tp = disk3DTechProcess.Angle != 0 ? p.ConvertAll(x => x.TransformBy(matrix)) : p;
                var loc = generator.ToolLocation;
                if (loc.IsDefined && loc.Point.DistanceTo(tp.First()) > loc.Point.DistanceTo(tp.Last()))
                    tp.Reverse();

                BuildPass(generator, tp);
            });
        }

        private List<Point3d> CalcOffsetPoints(Point3dCollection points, Extents3d bounds)
        {
            var pline = new PolylineCurve3d(points);
            var distance = TechProcess.Tool.Diameter / 2;
            var offsetCurves = pline.GetTrimmedOffset(distance, -Vector3d.YAxis, OffsetCurveExtensionType.Fillet);
            var matrix = Matrix3d.Displacement(Vector3d.ZAxis.Negate() * distance);
            offsetCurves.ForEach(p => p.TransformBy(matrix));

            var firstPoint = offsetCurves[0].StartPoint;
            var departurePoint = new Point3d((IsDepartureOnBorderSection ? firstPoint.X : bounds.MinPoint.X) - Departure, firstPoint.Y, firstPoint.Z);
            var offsetPoints = new List<Point3d> { departurePoint, firstPoint };

            switch (offsetCurves[0])
            {
                case LineSegment3d line:
                    offsetPoints.Add(line.EndPoint);
                    break;
                case CircularArc3d arc:
                    offsetPoints.AddRange(GetArcPoints(arc));
                    break;
                case CompositeCurve3d curve:
                    curve.GetCurves().ForEach(p =>
                    {
                        if (p is CircularArc3d arc)
                            offsetPoints.AddRange(GetArcPoints(arc));
                        else
                            offsetPoints.Add(p.EndPoint);
                    });
                    break;
                default:
                    throw new Exception($"Полученный тип кривой не может быть обработан {offsetCurves[0].GetType()}");
            }

            var lastPoint = offsetPoints.Last();
            offsetPoints.Add(new Point3d((IsDepartureOnBorderSection ? lastPoint.X : bounds.MaxPoint.X) + Departure, lastPoint.Y, lastPoint.Z));
            return offsetPoints;

            IEnumerable<Point3d> GetArcPoints(CircularArc3d arc)
            {
                var num = (int)((arc.EndAngle - arc.StartAngle) / (Math.PI / 36)) + 4;
                return arc.GetSamplePoints(num).Skip(1).Select(p => p.Point);
            }
        }

        private DbSurface CreateOffsetSurface()
        {
            DbSurface unionSurface = null;
            foreach (var dBObject in TechProcess.ProcessingArea.Select(p => p.ObjectId.QOpenForRead()))
            {
                DbSurface surface;
                switch (dBObject)
                {
                    case DbSurface sf:
                        surface = sf.Clone() as DbSurface;
                        break;
                    case Region region:
                        surface = new PlaneSurface();
                        ((PlaneSurface)surface).CreateFromRegion(region);
                        break;
                    default:
                        throw new Exception($"Объект типа {dBObject.GetType()} не может быть обработан (1)");
                }
                if (unionSurface == null)
                    unionSurface = surface;
                else
                {
                    var res = unionSurface.BooleanUnion(surface);
                    if (res != null)
                    {
                        unionSurface.Dispose();
                        unionSurface = res;
                    }
                    surface.Dispose();
                }
            }
            var offsetSurface = DbSurface.CreateOffsetSurface(unionSurface, Delta) as DbSurface;
            unionSurface.Dispose();
            return offsetSurface;
        }

        private void BuildPass(ICommandGenerator generator, List<Point3d> points)
        {
            if (TechProcess.MachineType == MachineType.ScemaLogic) //Settongs.IsFrontPlaneZero
                points = points.ConvertAll(p => new Point3d(p.X, p.Y + TechProcess.Tool.Thickness.Value, p.Z));
            var z = generator.ZSafety - TechProcess.ZSafety;
            bool isComplete;
            do
            {
                isComplete = true;
                z -= Penetration;
                var point0 = Algorithms.NullPoint3d;
                var point = Algorithms.NullPoint3d;

                foreach (var pt in points)
                {
                    var p = pt;
                    if (z > p.Z)
                    {
                        p = new Point3d(p.X, p.Y, z);
                        isComplete = false;
                    }
                    if (generator.IsUpperTool)
                        generator.Move(p.X, p.Y, angleC: ((Disk3DTechProcess)TechProcess).Angle);

                    if (point.IsNull())
                    {
                        if (generator.ToolLocation.Point != p)
                            generator.GCommand(CommandNames.Penetration, 1, point: p, feed: TechProcess.PenetrationFeed);
                    }
                    else if (point0 != point && point != p && !point0.GetVectorTo(point).IsCodirectionalTo(point.GetVectorTo(p)))
                    {
                        generator.GCommand(CommandNames.Cutting, 1, point: point, feed: CuttingFeed);
                        point0 = point;
                    }
                    if (point0.IsNull())
                        point0 = p;
                    point = p;
                }
                generator.GCommand(CommandNames.Cutting, 1, point: point, feed: CuttingFeed);
                points.Reverse();
            }
            while (!isComplete);

            generator.Uplifting();
        }

        //    var startTime = System.Diagnostics.Stopwatch.StartNew();
        //    for (var y = bounds.MinPoint.Y + StartPass; y < bounds.MaxPoint.Y; y += StepPass)
        //    {
        //        var lines = new List<Line>();
        //        Line line = null;
        //        var point0 = Algorithms.NullPoint3d;
        //        for (var x = bounds.MinPoint.X; x <= bounds.MaxPoint.X; x += StepLong)
        //        {
        //            double z = 0;
        //            for (var s = 0; s <= TechProcess.Tool.Thickness; s += 1)
        //            {
        //                if (y + s > bounds.MaxPoint.Y)
        //                    break;
        //                offsetSurfaces.RayTest(new Point3d(x, y + s, bounds.MinPoint.Z), Vector3d.ZAxis, 0.0001, out SubentityId[] col, out DoubleCollection par);
        //                if (par.Count == 0)
        //                {
        //                    z = 0;
        //                    break;
        //                }
        //                else if (par[0] > z)
        //                    z = par[0];
        //            }
        //            if (z > 0)
        //            {
        //                var point = new Point3d(x, y, bounds.MinPoint.Z + z);
        //                if (!point0.IsNull())
        //                {
        //                    if (line != null)
        //                    {
        //                        if (point0.GetVectorTo(point).IsCodirectionalTo(line.Delta))
        //                            line.Extend(false, point);
        //                        else
        //                        {
        //                            lines.Add(line);
        //                            line.ColorIndex = 3;
        //                            line.AddToCurrentSpace();
        //                            line = null;
        //                        }
        //                    }
        //                    if (line == null)
        //                    {
        //                        line = NoDraw.Line(point0, point);
        //                        if (lines.Count == 0)
        //                            line.Extend(-Departure);
        //                    }
        //                }
        //                point0 = point;
        //            }
        //        }
        //        if (line != null)
        //        {
        //            line.ColorIndex = 3;
        //            line.Extend(line.EndParam + Departure);
        //            line.AddToCurrentSpace();
        //        }
        //        var ptc = new Point3dCollection(pts);
        //        var pl = new PolylineCurve3d(ptc);
        //        pl.SetControlPointAt(2, pl.EndPoint + Vector3d.XAxis * 5);


        //        var cc = pl.GetTrimmedOffset(2, Vector3d.XAxis, OffsetCurveExtensionType.Fillet);
        //    }
        //    startTime.Stop();
        //    var resultTime = startTime.Elapsed;
        //    Acad.Write("timer=" + resultTime);

        //}


        /*
        var surfaceDict = new Dictionary<DbSurface, Curve>();
        var projectCurves = new List<Curve>();
        var vertLines = new List<Line>();

        var ray = new Ray
        {
            UnitDir = Vector3d.YAxis,
            BasePoint = new Point3d(boundsModel.MinPoint.X + (boundsModel.MaxPoint.X - boundsModel.MinPoint.X) / 2, 0, 0)
        };
        foreach (var surface in surfaces)
        { 
            var curves = surface.ProjectOnToSurface(ray, Vector3d.ZAxis);
            if (curves.Length > 1)
                throw new Exception($"Объект не может быть обработан (2)");
            if (curves.Length == 0)
                continue;
            if (curves[0] is Line l && l.StartPoint.Z.Round(6) == 0 && l.EndPoint.Z.Round(6) == 0)
                continue;
            if (curves[0] is Line line && line.StartPoint.Y.Round(6) == line.EndPoint.Y.Round(6))
                vertLines.Add(line);
            else
                projectCurves.Add((Curve)curves[0]);
        }
        var plines = new List<Polyline>();
        while (projectCurves.Any())
        {
            var point = projectCurves.SelectMany(p => p.GetStartEndPoints()).OrderBy(p => p.Y).First();
            var ptc = new Point3dCollection();

            //var pline = new Polyline();
            //var i = 0;
            while (projectCurves.SingleOrDefault(p => p.HasPoint(point)) is Curve curve)
            {
                if (curve is Arc arc)
                {
                    int divs = (int)(arc.Length / 5 + 10);
                    for (int i = 0; i < divs; i++)
                        ptc.Add(arc.GetPointAtParameter(arc.StartParam + arc.TotalAngle * (point == arc.StartPoint ? i : divs - i) / divs));
                        //ptc.Add(arc.GetPointAtParameter(arc.StartParam + arc.TotalAngle * (point == arc.StartPoint ? i : divs - i) / divs));
                }
                else
                    ptc.Add(point);

                //var bulge = curve is Arc arc? Algorithms.GetArcBulge(arc, point) : 0;
                //pline.AddVertexAt(i++, new Point2d(point.Y, point.Z), bulge, 0, 0);
                point = curve.NextPoint(point);
                projectCurves.Remove(curve);
            }
            ptc.Add(point);

            for (int i = 1; i < ptc.Count; i++)
            {
                var ll = NoDraw.Line(ptc[i - 1], ptc[i]);
                ll.ColorIndex = 6;
                ll.AddToCurrentSpace();
                //Draw.Line(new Point3d(ptc[i - 1].Y, ptc[i - 1].Z, 0), new Point3d(ptc[i].Y, ptc[i].Z, 0));
            }
            //return;

            //pline.AddVertexAt(i, new Point2d(point.Y, point.Z), 0, 0, 0);
            //pline.ColorIndex = 6;
            //pline.AddToCurrentSpace();

            //var pline1 = (Curve)pline.GetOffsetCurvesGivenPlaneNormal(Vector3d.ZAxis, -2)[0];
            //pline1.ColorIndex = 0;

            //var seg = pline.GetArcSegmentAt(0);
            //var pts = pline.GetPolylineFitPoints().ToArray();
            //var ptc = new Point3dCollection(pts);
            var pl = new PolylineCurve3d(ptc);
            //pl.SetControlPointAt(2, pl.EndPoint + Vector3d.XAxis * 5);


            var cc = pl.GetTrimmedOffset(2, Vector3d.XAxis, OffsetCurveExtensionType.Fillet);
            if (cc[0] is CompositeCurve3d cc3d)
                cc = cc3d.GetCurves();

                var pline1 = new Polyline();
                //int i = 0;
                foreach (var item in cc)
                {
                    Curve curve = null;
                    if (item is LineSegment3d ls)
                        curve = NoDraw.Line(ls.StartPoint, ls.EndPoint);
                    if (item is CircularArc3d ca)
                    {
                        //System.Diagnostics.Debug.WriteLine(ca.Center.Z + " : " + ca.StartPoint.Z);
                        var angle = (ca.Center - ca.StartPoint).GetAngleTo(Vector3d.YAxis);
                        curve = new Arc(ca.Center, ca.Normal, ca.Radius, angle, angle + ca.EndAngle);
                        //arc.StartAngle = (arc.Center - arc.StartPoint).GetAngleTo(Vector3d.YAxis);
                        //arc.EndAngle = arc.StartAngle + ca.EndAngle;
                        //curve = arc;
                    }
                    //var bulge = curve is Arc arc ? Algorithms.GetArcBulge(arc, item.StartPoint) : 0;
                    //pline1.AddVertexAt(i++, item.StartPoint.ToPoint2d(), bulge, 0, 0);
                    curve.ColorIndex = item is LineSegment3d ? 3 : 6;
                    curve.AddToCurrentSpace();
                }
                //pline1.AddVertexAt(i, new Point2d(cc.Last().EndPoint.Y, cc.Last().EndPoint.Z), 0, 0, 0);
                //pline1.ColorIndex = 3;
                //pline1.AddToCurrentSpace();

                //foreach (var item in cc)
                //{
                //    if (item is LineSegment3d ls)
                //        Draw.Line(ls.StartPoint, ls.EndPoint);
                //    if (item is CircularArc3d ca)
                //        Draw.ArcFromGeometry(ca);
                //}
                //cc.ForEach(p => Draw.Line(p.StartPoint, p.EndPoint));

                //var ln = NoDraw.Line(new Point3d(3.5, 0, 0), new Point3d(3.5, 1000, 0));

                //ptc = new Point3dCollection();
                //ln.IntersectWith(pline1, Intersect.OnBothOperands, ptc, IntPtr.Zero, IntPtr.Zero);
                //if (ptc.Count > 0)
                //{
                //    Draw.Circle(ptc[0], 1);
                //    ln.AddToCurrentSpace();
                //}
            }
            //DBObjectCollection col = new DBObjectCollection();
            //((Polyline)pline1).Explode(col);
            //var ar = col[0] as Arc;
            //pline1.AddToCurrentSpace();

            //PolylineCurve2d pp;
            //pp.GetTrimmedOffset

        var posY = StartPass - StepPass;
        surface = null;
        ray.UnitDir = Vector3d.XAxis;
        ray.BasePoint = new Point3d(StartPass, 0, 0);
        double thick = TechProcess.Tool.Thickness.Value;
        double? changeFlagPosition = null;
        Curve profile = null;
        double endProfile = 0;
        while ((posY += StepPass) < boundsModel.MaxPoint.Y)
        {
            ray.BasePoint = new Point3d(0, posY, 0);
            if (surface == null)
            {
                GetSurface();
                if (surface == null)
                    continue;
            }
            var line = GetProjectCurve();
            if (line == null)
                continue;

           // var z = line.StartPoint.Z + Delta / Math.Cos();
            if (profile is Arc arc && arc.EndPoint.Y > posY && arc.Center.Y > posY && arc.Center.Y < posY + thick)
            {
                ray.BasePoint = new Point3d(0, arc.Center.Y, 0);
                //z = GetProjectCurve().StartPoint.Z;
            }
            if (posY + thick < endProfile)
            {
                ray.BasePoint = new Point3d(0, posY + thick, 0);
                var z2 = GetProjectCurve().StartPoint.Z;
            }
            else
            {
                ray.BasePoint = new Point3d(0, endProfile, 0);
                var z3 = GetProjectCurve().StartPoint.Z;
                ray.BasePoint = new Point3d(0, posY + thick, 0);
                GetSurface();
                if (surface != null)
                {
                    //var z3 = GetProjectCurve().StartPoint.Z;

                }
            }
                if (generator.IsUpperTool)
                generator.Move(line.StartPoint.X, line.StartPoint.Y, angleC: 0);

            generator.Cutting(line, CuttingFeed, 200);
        }
        ray.Dispose();

        void GetSurface()
        {
            surface = surfaceDict.Keys.FirstOrDefault(p => p.ProjectOnToSurface(ray, Vector3d.ZAxis).Length == 1);
            if (surface != null)
            {
                profile = surfaceDict[surface];
                endProfile = profile.GetStartEndPoints().Max(p => p.Y);
            }
        }

        Line GetProjectCurve()
        {
            var curves = surface.ProjectOnToSurface(ray, Vector3d.ZAxis);
            if (curves.Length == 0)
            {
                GetSurface();
                if (surface == null)
                    return null;
                curves = surface.ProjectOnToSurface(ray, Vector3d.ZAxis);
            }
            var line = curves[0] as Line;
            if (line == null)
                throw new Exception($"Объект не может быть обработан (5)");
            return line;
        }

        double GetZ(double y1, double y2)
        {
            if (profile is Line line)
            {
                var dy = line.Delta.Z < 0 ? y1 : y2 - line.StartPoint.Y;
                return (dy * line.Delta.Z + Delta * line.Length) / line.Delta.Y;
            }
            if (profile is Arc arc)
            {
                if (arc.EndPoint.Y > arc.StartPoint.Y && arc.Center.Y >= y1 && arc.Center.Y <= y2)
                    return arc.Center.Z + arc.Radius + Delta;
                var y0 = arc.StartPoint.Y < arc.Center.Y ^ arc.StartPoint.Z < arc.Center.Z ? y2 : y1;
                var z = Math.Sqrt(arc.Radius * arc.Radius - (arc.Center.Y - y0) * (arc.Center.Y - y0));
                return arc.Center.Z + (arc.EndPoint.Y > arc.StartPoint.Y ? 1 : -1) * z + Delta * arc.Radius / z;
            }
            throw new Exception($"Объект не может быть обработан (6)");
        }
    }
        */

        //void Test()
        //{
        //    var line = NoDraw.Line(new Point3d(0, 500, 0), new Point3d(0, 1000, 0));
        //    line.AddToCurrentSpace();

        //    var l1 = (Curve)line.GetOffsetCurves(100)[0];
        //    l1.Extend(-100);
        //    l1.AddToCurrentSpace();

        //    l1 = (Curve)line.GetOffsetCurves(200)[0];
        //    l1.Extend(100);
        //    l1.AddToCurrentSpace();

        //    l1 = (Curve)line.GetOffsetCurves(300)[0];
        //    l1.Extend(-0.5);
        //    l1.AddToCurrentSpace();

        //    l1 = (Curve)line.GetOffsetCurves(400)[0];
        //    l1.Extend(0);
        //    l1.AddToCurrentSpace();

        //    l1 = (Curve)line.GetOffsetCurves(500)[0];
        //    l1.Extend(0.5);
        //    l1.AddToCurrentSpace();

        //    l1 = (Curve)line.GetOffsetCurves(600)[0];
        //    l1.Extend(1);
        //    l1.AddToCurrentSpace();

        //    l1 = (Curve)line.GetOffsetCurves(700)[0];
        //    l1.Extend(2);
        //    l1.AddToCurrentSpace();
        //}
    }
}
