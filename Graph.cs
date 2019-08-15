﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using CAM.Domain;
using Dreambuild.AutoCAD;
using System;
using System.Collections.Generic;

namespace CAM
{
    /// <summary>
    /// Методы для работы с графикой
    /// </summary>
    public static class Graph
    {
        internal static double DegToRad(double angle) => angle * Math.PI / 180;

        public static double Length(this Curve curve)
        {
            switch (curve)
            {
                case Line line:
                    return line.Length;

                case Arc arc:
                    return arc.Length;

                default:
                    throw new ArgumentException($"Тип кривой не поддерживается: {curve.GetType()}");
            }
        }

        public static Vector2d GetTangent(this Curve curve, Point3d point = default)
        {
            switch (curve)
            {
                case Line line:
                    return new Vector2d(line.Delta.X, line.Delta.Y).GetNormal();

                case Arc arc:
                    return new Vector2d(point.X - arc.Center.X, point.Y - arc.Center.Y).GetNormal().GetPerpendicularVector();

                default:
                    throw new ArgumentException($"Тип кривой не поддерживается: {curve.GetType()}");
            }
        }

        public static Point3d GetPoint(this Curve curve, Corner corner) => corner == Corner.Start ? curve.StartPoint : curve.EndPoint;

        public static Corner GetCorner(this Curve curve, Point3d point) =>
            point == curve.StartPoint ? Corner.Start : (point == curve.EndPoint ? Corner.End : throw new ArgumentException("Некорректный параметр метода GetCorner"));

        public static bool HasPoint(this Curve curve, Point3d point) => point == curve.StartPoint || point == curve.EndPoint;

        public static Point3d NextPoint(this Curve curve, Point3d point) =>
            point == curve.StartPoint ? curve.EndPoint : (point == curve.EndPoint ? curve.StartPoint : throw new ArgumentException("Некорректный параметр метода NextPoint"));

        public static void SetPoint(this Curve curve, Corner corner, Point3d point)
        {
            if (corner == Corner.Start)
                curve.StartPoint = point;
            else
                curve.EndPoint = point;
        }

        public static Vector2d GetVector2d(this Line line) => new Vector2d(line.Delta.X, line.Delta.Y);

        public static double AngleRad(this Line line) => Math.Round(line.Angle, 6);

        public static double AngleDeg(this Line line) => Math.Round(line.Angle * 180 / Math.PI, 6);

        public static void CreateHatch(List<Curve> contour, int sign)
        {
            var start = contour.Count > 1
                ? contour[1].HasPoint(contour[0].EndPoint) ? contour[0].StartPoint : contour[0].EndPoint
                : contour[0].StartPoint;

            var polyline = new Polyline();
            var next = start;
            var i = 0;
            foreach (var curve in contour)
            {
                var bulge = curve is Arc ? Algorithms.GetArcBulge(curve as Arc, next) : 0;
                polyline.AddVertexAt(i++, next.ToPoint2d(), bulge, 0, 0);
                next = curve.NextPoint(next);
            }
            polyline.Closed = next == start;
            Polyline offsetPolyline = null;
            var offset = sign * 40;
            if (!polyline.Closed)
            {
                polyline.AddVertexAt(i, next.ToPoint2d(), 0, 0, 0);
                offsetPolyline = polyline.GetOffsetCurves(offset)[0] as Polyline;
                offsetPolyline.ReverseCurve();
                polyline.JoinPolyline(offsetPolyline);
                polyline.SetBulgeAt(polyline.NumberOfVertices - 1, 0);
                polyline.Closed = true;
                offsetPolyline = null;
            }
            else
                offsetPolyline = polyline.GetOffsetCurves(offset)[0] as Polyline;

            using (var doclock = Acad.Document.LockDocument())
            using (var trans = Acad.Database.TransactionManager.StartTransaction())
            {
                var space = (BlockTableRecord)trans.GetObject(Acad.Database.CurrentSpaceId, OpenMode.ForWrite, false);
                space.AppendEntity(polyline);
                trans.AddNewlyCreatedDBObject(polyline, true);
                if (offsetPolyline != null)
                {
                    space.AppendEntity(offsetPolyline);
                    trans.AddNewlyCreatedDBObject(offsetPolyline, true);
                }
                var hatch = new Hatch();
                space.AppendEntity(hatch);
                trans.AddNewlyCreatedDBObject(hatch, true);

                hatch.LayerId = Acad.GetHatchLayerId();
                hatch.SetDatabaseDefaults();
                hatch.Normal = new Vector3d(0, 0, 1);
                hatch.Elevation = 0.0;
                hatch.Associative = false;
                hatch.PatternScale = 4;
                hatch.SetHatchPattern(HatchPatternType.PreDefined, "ANSI31");
                //hatch.PatternAngle = angle; // PatternAngle has to be after SetHatchPattern(). This is AutoCAD .NET SDK violating Framework Design Guidelines, which requires properties to be set in arbitrary order.
                hatch.HatchStyle = HatchStyle.Outer;
                hatch.AppendLoop(HatchLoopTypes.External, new ObjectIdCollection(new[] { polyline.ObjectId }));
                if (offsetPolyline != null)
                    hatch.AppendLoop(HatchLoopTypes.External, new ObjectIdCollection(new[] { offsetPolyline.ObjectId }));

                hatch.EvaluateHatch(true);

                polyline.Erase();
                offsetPolyline?.Erase();

                trans.Commit();
            }
        }
    }
}