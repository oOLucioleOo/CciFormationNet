//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class STREAMS
    {
        public long STREAM_ID { get; set; }
        public long USER_ID { get; set; }
        public string STREAM_LINK { get; set; }
        public System.DateTime STREAM_DATE_START { get; set; }
        public System.DateTime STREAM_DATE_END { get; set; }
    
        public virtual USERS USERS { get; set; }
    }
}
