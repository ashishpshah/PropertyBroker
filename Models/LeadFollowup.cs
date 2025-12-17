using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class LeadFollowup : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public long FollowupId { get; set; }

    public long? LeadId { get; set; }

    public string? Remark { get; set; }

    public DateOnly? NextFollowupDate { get; set; }

    public DateTime? ReminderDatetime { get; set; }

    public virtual Lead? Lead { get; set; }
}
