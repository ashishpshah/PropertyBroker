using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class LeadSource : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public long LeadSourceId { get; set; }

    public string LeadSourceName { get; set; } = null!;

	[NotMapped] public override long CreatedBy { get; set; }

	[NotMapped] public override DateTime? CreatedDate { get; set; }

	[NotMapped] public override long? LastModifiedBy { get; set; }

	[NotMapped] public DateTime? LastModifiedDate { get; set; }

	[NotMapped] public override bool IsActive { get; set; }

	[NotMapped] public override bool IsDeleted { get; set; }

}
