using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class User : EntitiesBase
{
	public override long Id { get; set; }

	public string? UserName { get; set; }

    public string? Password { get; set; }

    public int? NoOfWrongPasswordAttempts { get; set; }

    public DateTime? NextChangePasswordDate { get; set; }

    public string? MobileNumber { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

	[NotMapped] public string UserId { get; set; }
	[NotMapped] public string User_Role { get; set; }
	[NotMapped] public long User_Role_Id { get; set; }
	[NotMapped] public long RoleId { get; set; }
	[NotMapped] public bool IsPassword_Reset { get; set; }
	[NotMapped] public DateTime? Date { get; set; }
	[NotMapped] public string Date_Text { get; set; }
	[NotMapped] public string User_Id_Str { get; set; }
	[NotMapped] public string Role_Id_Str { get; set; }
}
