using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Models;

public partial class PropertyCategoryTypeMapping
{

	public long CategoryId { get; set; }

	public long TypeId { get; set; }
	public long ParentId { get; set; }

	public string CategoryName { get; set; }

	public string TypeName { get; set; }

}
