using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class Lead : EntitiesBase
{
	public override long Id { get; set; }

	public string? Name { get; set; }

	public string? Mobile { get; set; }

	public string? Email { get; set; }

	public string? Requirement { get; set; }

	public long? PreferredCityId { get; set; }

	public long? PreferredAreaId { get; set; }

	public string? Landmark { get; set; }

	public long? PropertyType { get; set; }

	public string? Status { get; set; }

	public long? AssignedTo { get; set; }

	public string? LeadSource { get; set; }

	public decimal? BudgetMin { get; set; }

	public decimal? BudgetMax { get; set; }

	public string? Remarks { get; set; }

	public DateTime? NextFollowUpDate { get; set; }

	public virtual ICollection<LeadTimeline> LeadTimelines { get; set; } = new List<LeadTimeline>();

	[NotMapped] public long? Preferred_City_Id { get; set; }
	[NotMapped] public long? Preferred_Area_Id { get; set; }


	[NotMapped] public string? LeadSource_Value { get; set; }
	[NotMapped] public string? LeadSource_TEXT { get; set; }
	[NotMapped] public string? Property_Type_TEXT { get; set; }
    [NotMapped] public string? AssignedTo_Text { get; set; }
    [NotMapped] public string? Status_TEXT { get; set; }

	[NotMapped] public virtual User? AssignedToNavigation { get; set; }

	[NotMapped] public virtual ICollection<LeadFollowup> LeadFollowups { get; set; } = new List<LeadFollowup>();

	//public virtual LeadSource? LeadSource { get; set; }

	[NotMapped] public virtual City? PreferredCity { get; set; }

	[NotMapped] public virtual PropertyType? PropertyTypeNavigation { get; set; }

	[NotMapped] public DateTime? NextFollowupDate { get; set; }
}
