using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.LinkModels
{
    public class LinkResponse
    {
        public bool HasLinks { get; set; }
        public List<Entity> ShapedLinks { get; set; }
        public List<LinkCollectionWrapper<Entity>> LinkedEntities { get; set; }

        public LinkResponse()
        {
            LinkedEntities = new List<LinkCollectionWrapper<Entity>>();
            ShapedLinks = new List<Entity>();
        }
    }
}
