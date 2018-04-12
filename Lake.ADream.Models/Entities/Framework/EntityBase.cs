using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lake.ADream.Entities.Framework
{
    public abstract class EntityBase : IEntityBase
    {
        [Key]
        [Display(Name = "主键")]
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();
        [Display(Name = "创建时间")]
        public virtual DateTime CreatedTime { get; set; } = DateTime.Now;
        [Display(Name = "最后修改时间")]
        public virtual DateTime EditedTime { get; set; } = DateTime.Now;
        [StringLength(50)]
        [Display(Name = "创建人")]

        public virtual string CretaedUser { get; set; } 
        [StringLength(50)]
        [Display(Name = "最后编辑人员")]

        public virtual string EditeUser { get; set; } 

        [Timestamp]
        [Display(Name = "时间戳")]

        public virtual byte[] TimeSpan { get; set; }

        [Display(Name = "备注说明")]
        public virtual string Description { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
