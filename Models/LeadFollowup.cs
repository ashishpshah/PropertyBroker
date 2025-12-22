using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class LeadFollowup : EntitiesBase
{
	public override long Id { get; set; }

    public long? LeadId { get; set; }

    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? Remark { get; set; }

    public DateTime? NextFollowupDate { get; set; }

    public DateTime? ReminderDatetime { get; set; }

    public virtual Lead? Lead { get; set; }
}
