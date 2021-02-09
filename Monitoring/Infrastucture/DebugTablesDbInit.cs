using System;
using System.Linq;
using ApplicationCore.Models;

namespace Infrastructure
{
    /// <summary>
    /// Класс DebugTablesDbInit используется для инициализации базы данных при первом обращении к ней.
    /// </summary>
    public static class DebugTablesDbInit
    {
        /// <summary>
        /// Добавления  объектов в базу данных при первом обращении к ней.
        /// Для добавления объектов в базу данных в метод Initialize() передается контекст данных. 
        /// </summary>
        /// <param name="context">Контекст данных</param>
        public static void Initialize(TableContext context)
        {
           
        }
    }
}
