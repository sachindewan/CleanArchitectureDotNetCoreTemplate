﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.WebApi1.Domain.Common
{
    public abstract class AuditableBaseEntity:BaseEntity
    {
        public virtual int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
