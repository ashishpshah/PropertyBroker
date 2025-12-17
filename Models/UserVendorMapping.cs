using System;
using System.Collections.Generic;

namespace Broker.Models;

public partial class UserVendorMapping : EntitiesBase
{
	public override long Id { get; set; }

	public long VendorId { get; set; }

    public long UserId { get; set; }

}
