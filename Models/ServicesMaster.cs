using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class ServicesMaster : EntitiesBase
{
    [NotMapped] public override long Id { get; set; }
    public long ServiceId { get; set; }

    public string ServiceTitle { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public string? ImageName { get; set; }

    public int DisplayOrder { get; set; }

    public bool? IsFeatured { get; set; }

    public byte[]? ResumeFile { get; set; }

    [NotMapped] public IFormFile? ImageFile { get; set; }
}
