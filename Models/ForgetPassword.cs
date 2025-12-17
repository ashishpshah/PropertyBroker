using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class ForgetPassword : EntitiesBase
{
	public override long Id { get; set; }

	public string Email { get; set; } = null!;

    public string Otp { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsUsed { get; set; }

	[NotMapped] public override long CreatedBy { get; set; }

	[NotMapped] public override DateTime? CreatedDate { get; set; }

	[NotMapped] public override long LastModifiedBy { get; set; }

	[NotMapped] public DateTime? LastModifiedDate { get; set; }

	[NotMapped] public override bool IsActive { get; set; }

	[NotMapped] public override bool IsDeleted { get; set; }

}
