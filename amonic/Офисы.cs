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
    
    public partial class Офисы
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Офисы()
        {
            this.Информация_о_пользователе = new HashSet<Информация_о_пользователе>();
        }
    
        public int id_офиса { get; set; }
        public string Наименование_офиса { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Информация_о_пользователе> Информация_о_пользователе { get; set; }
    }
}
