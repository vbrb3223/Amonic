//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace amonic
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class СамолетыEnt : DbContext
    {
        public СамолетыEnt()
            : base("name=СамолетыEnt")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Аэропорты> Аэропорты { get; set; }
        public virtual DbSet<Билеты> Билеты { get; set; }
        public virtual DbSet<Информация_о_пользователе> Информация_о_пользователе { get; set; }
        public virtual DbSet<Офисы> Офисы { get; set; }
        public virtual DbSet<Оформление_билетов> Оформление_билетов { get; set; }
        public virtual DbSet<Пассажиры> Пассажиры { get; set; }
        public virtual DbSet<Попытки_входа> Попытки_входа { get; set; }
        public virtual DbSet<Рейсы> Рейсы { get; set; }
        public virtual DbSet<Самолеты> Самолеты { get; set; }
        public virtual DbSet<Страны> Страны { get; set; }
    }
}
