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
    using System.Collections.Generic;
    
    public partial class Попытки_входа
    {
        public int id_попытки { get; set; }
        public System.DateTime Дата_входа { get; set; }
        public System.TimeSpan Время_входа { get; set; }
        public System.TimeSpan Время_выхода { get; set; }
        public System.TimeSpan Время_в_системе { get; set; }
        public string Причина_выхода { get; set; }
        public int id_пользователя { get; set; }
    
        public virtual Информация_о_пользователе Информация_о_пользователе { get; set; }
    }
}
