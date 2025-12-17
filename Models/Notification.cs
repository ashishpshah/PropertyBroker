using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class Notification : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public long NotificationId { get; set; }

    public long? UserId { get; set; }

    public string? Title { get; set; }

    public string? Message { get; set; }

    public bool? IsRead { get; set; }

    public virtual User? User { get; set; }
}
