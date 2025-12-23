using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class PropertyImage : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }
	public long ImageId { get; set; }

    public long? PropertyId { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsPrimary { get; set; }

    public byte[]? ResumeFile { get; set; }

    public virtual Properties? Property { get; set; }

	[NotMapped] public override long CreatedBy { get; set; }

	[NotMapped] public override DateTime? CreatedDate { get; set; }

	[NotMapped] public override long? LastModifiedBy { get; set; }

	[NotMapped] public DateTime? LastModifiedDate { get; set; }

	[NotMapped] public override bool IsActive { get; set; }

	[NotMapped] public override bool IsDeleted { get; set; }

}
