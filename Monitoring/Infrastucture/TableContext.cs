using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Models;

namespace Infrastructure
{
    /// <summary>
    ///  Контекст данных - класс, унаследованный от класса Microsoft.EntityFrameworkCore.DbContext. Используется
    ///  для взаимодействия  с базой данных через Entity Framework.
    /// </summary>
    public class TableContext : DbContext
    {
        /// <summary>
        /// Представляет собой коллекцию объектов, которая сопоставляется с определенной таблицей в базе данных.
        /// </summary>
        public DbSet<LogObject> Logs { get; set; }
        /// <summary>
        /// Представляет собой коллекцию объектов, которая сопоставляется с определенной таблицей в базе данных.
        /// </summary>
        public DbSet<MetricItem> Metrics { get; set; }

        /// <summary>
        /// С помощью вызова Database.EnsureCreated() по определению моделей будет создаваться база данных (если она отсутствует).
        /// </summary>
        /// <param name="options">Через параметр options в конструктор контекста данных передаются настройки контекста.</param>
        public TableContext(DbContextOptions<TableContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
