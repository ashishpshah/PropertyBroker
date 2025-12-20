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

    public long? Preferred_Area_Id { get; set; }

    public long? PropertyType { get; set; }

    public string? Status { get; set; }

    public long? AssignedTo { get; set; }

    public long? Preferred_City_Id { get; set; }

    public string? Landmark { get; set; }

    public string? LeadSource_Value { get; set; }
    public string? LeadSource_TEXT { get; set; }
    public string? Property_Type_TEXT { get; set; }
    public string? AssignedTo_Text { get; set; }

    public decimal? BudgetMin { get; set; }

    public decimal? BudgetMax { get; set; }

    public virtual User? AssignedToNavigation { get; set; }

    public virtual ICollection<LeadFollowup> LeadFollowups { get; set; } = new List<LeadFollowup>();

    public virtual LeadSource? LeadSource { get; set; }

    public virtual ICollection<LeadTimeline> LeadTimelines { get; set; } = new List<LeadTimeline>();

    public virtual City? PreferredCity { get; set; }

    public virtual PropertyType? PropertyTypeNavigation { get; set; }
}
