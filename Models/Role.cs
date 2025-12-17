using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class Role : EntitiesBase
{
	public override long Id { get; set; }

	public string Name { get; set; } = null!;

    public int? DisplayOrder { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsAdmin { get; set; }

    public string? ProjectDetailTypeAccess { get; set; }

	[NotMapped] public long SelectedRoleId { get; set; } = 0;
	[NotMapped] public List<SelectListItem> Menus { get; set; } = null;

}
