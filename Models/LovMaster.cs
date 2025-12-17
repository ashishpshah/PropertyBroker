using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class LovMaster : EntitiesBase
{
	[NotMapped] public override long Id { get; set; }

	public string LovColumn { get; set; } = null!;

    public string LovCode { get; set; } = null!;

    public string LovDesc { get; set; } = null!;

    public int DisplayOrder { get; set; }

}
