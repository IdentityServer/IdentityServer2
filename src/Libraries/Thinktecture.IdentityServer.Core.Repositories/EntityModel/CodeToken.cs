using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql
{
    public class CodeToken
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public int ClientId { get; set; }

        public string UserName { get; set; }

        public string Scope { get; set; }

        public int Type { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
