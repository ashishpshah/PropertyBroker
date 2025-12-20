using System;
using System.Collections.Generic;

namespace Broker.Models;

public partial class Properties : EntitiesBase
{
	public override long Id { get; set; }

	public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public long? TypeId { get; set; }

    public long? CategoryId { get; set; }

    public decimal? Price { get; set; }

    public decimal? AreaSqft { get; set; }

    public string? OwnerName { get; set; }

    public string? OwnerMobile { get; set; }

    public string? BuilderName { get; set; }

    public string? Status { get; set; }

    public long? CityId { get; set; }

    public long? AreaId { get; set; }

    public string? Landmark { get; set; }

    public string? Remark { get; set; }
}
