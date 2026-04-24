using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ChangedResponse
    {
        public int Id { get; set; }

        public static ChangedResponse FromEntity(IEntity entity)
        {
            return new ChangedResponse { Id = entity.Id };
        }
    }
}
