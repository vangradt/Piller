using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Services;
using MvvmCross.Platform;

namespace Piller.Data
{
    [Table("ProduktLeczniczyOpakowanie")]
    public class Medicines
    {
        public string NazwaProduktu { get; set; }

        [PrimaryKey]
        public string KodEAN { get; set; }
    }
}
