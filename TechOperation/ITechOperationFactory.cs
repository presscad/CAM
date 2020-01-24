﻿using Autodesk.AutoCAD.DatabaseServices;

namespace CAM
{
	public interface ITechOperationFactory
	{
        ProcessingType ProcessingType { get; }

        /// <summary>
        /// Получает параметры по-умолчанию для тех операции
        /// </summary>
        /// <returns></returns>
		object GetTechOperationParams();

        /// <summary>
        /// Создает технологическую операцию
        /// </summary>
        /// <param name="techProcess"></param>
        /// <param name="curve">Графическая кривая представляющая область обработки</param>
        /// <returns>Технологическая операцию</returns>
        ITechOperation[] Create(TechProcess techProcess, Curve[] curves);
	}
}