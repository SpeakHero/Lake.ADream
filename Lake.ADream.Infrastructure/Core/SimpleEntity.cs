using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lake.ADream.Infrastructure
{
    public interface ISimpleEntity
    {
        String Id { get; set; }
        string Name { get; set; }
    }

    public class SimpleEntity : ISimpleEntity
    {
        [Key]
        public String Id { get; set; } = Guid.NewGuid().ToString();


        public string Name
        {
            get; set;
        }

        public SimpleEntity()
        {

        }

        public SimpleEntity(string id, string name)
        {
            this.Id = id;
            Name = name;
        }
    }
}
