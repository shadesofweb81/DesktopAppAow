using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Models
{
    public class AttributeModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; }

        // Foreign keys
        public Guid CompanyId { get; set; }

        // Navigation properties
        public Company Company { get; set; } = null!;
        public ICollection<AttributeOption> AttributeOptions { get; set; } = new HashSet<AttributeOption>();
        public ICollection<ProductAttributeModel> ProductAttributes { get; set; } = new HashSet<ProductAttributeModel>();
    }

    public class AttributeOption
    {
        public string Value { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; }

        // Foreign keys
        public Guid AttributeId { get; set; }
        public Guid CompanyId { get; set; }

        // Navigation properties
        public AttributeModel Attribute { get; set; } = null!;      

    }
}
