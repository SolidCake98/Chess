namespace ChessAPI.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Games
    {
        public int id { get; set; }

        [StringLength(255)]
        public string FEN { get; set; }

        [StringLength(10)]
        public string Statuse { get; set; }

        public int? White { get; set; }

        public int? Black { get; set; }

        [StringLength(50)]
        public string ColorMove  { get; set; }

        [JsonIgnore]
        public virtual Users Users { get; set; }

        [JsonIgnore]
        public virtual Users Users1 { get; set; }
    }
}
