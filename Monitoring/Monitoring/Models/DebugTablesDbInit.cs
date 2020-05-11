using System;
using System.Linq;
using Monitoring.Models;

namespace Monitoring
{
    /// <summary>
    /// Класс DebugTablesDbInit используется для инициализации базы данных при первом обращении к ней.
    /// </summary>
    public static class DebugTablesDbInit
    {
        /// <summary>
        /// Метод класса DebugTablesDbInit используется для добавления  объектов в базу данных при первом обращении к ней.
        /// Для добавления объектов в базу данннах в метод Initialize() передается контекст данных. 
        /// </summary>
        /// <param name="context"></param>
        public static void Initialize(TableContext context)
        {
            
        }
    }
}
