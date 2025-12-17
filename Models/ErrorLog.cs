using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class ErrorLog : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public long ErrorId { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string? ControllerName { get; set; }

    public string ErrorMessage { get; set; } = null!;

    public string? ErrorType { get; set; }

    public string? StackTrace { get; set; }

    public string? RequestUrl { get; set; }

    public string? RequestPayload { get; set; }

    public string? UserAgent { get; set; }

    public long? UserId { get; set; }

    public string? ClientIp { get; set; }

	public override long CreatedBy { get; set; }

	public override DateTime? CreatedDate { get; set; }

	[NotMapped] public override long LastModifiedBy { get; set; }

	[NotMapped] public DateTime? LastModifiedDate { get; set; }

	[NotMapped] public override bool IsActive { get; set; }

	[NotMapped] public override bool IsDeleted { get; set; }

}
