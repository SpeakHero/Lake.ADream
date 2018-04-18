using System;

namespace Lake.ADream.Entities.Framework
{
    public interface IEntityBase
    {
        DateTime CreatedTime { get; set; }
        string CretaedUser { get; set; }
        string Description { get; set; }
        DateTime EditedTime { get; set; }
        string EditeUser { get; set; }
        string Id { get; set; }
        bool IsDelete { get; set; }
        DateTime TimeSpan { get; set; }
    }
}