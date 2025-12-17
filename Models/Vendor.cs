using System;
using System.Collections.Generic;

namespace Broker.Models;

public partial class Vendor : EntitiesBase
{
	public override long Id { get; set; }

	public long UserId { get; set; }

    public long RoleId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? MiddleName { get; set; }

    public string? Address { get; set; }

    public long? CityId { get; set; }

    public long? StateId { get; set; }

    public long? CountryId { get; set; }

    public string? Email { get; set; }

    public string? ContactNo { get; set; }

    public string? ContactNoAlternate { get; set; }

    public byte[]? Logo { get; set; }
}
