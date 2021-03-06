﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeUnionCommittee.DAL.Entities
{
    public class HobbyEmployees
    {
        public long Id { get; set; }
        public long IdEmployee { get; set; }
        public long IdHobby { get; set; }
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("xmin", TypeName = "xid")]
        public uint RowVersion { get; set; }

        public Employee IdEmployeeNavigation { get; set; }
        public Hobby IdHobbyNavigation { get; set; }
    }
}
