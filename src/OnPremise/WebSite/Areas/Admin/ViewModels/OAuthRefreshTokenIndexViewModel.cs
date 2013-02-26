using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class OAuthRefreshTokenIndexViewModel
    {
        public OAuthRefreshTokenIndexViewModel(
            TokenSearchCriteria searchCriteria,
            IClientsRepository clientRepository)
        {
            this.SearchCriteria = searchCriteria;
            var clients = 
                from item in clientRepository.GetAll()
                select new 
                {
                    item.Name, item.ID
                };
            var list =
                (from item in clients.ToArray()
                select new SelectListItem
                {
                    Text = item.Name, Value=item.ID.ToString()
                }).ToList();
            list.Insert(0, new SelectListItem { Text = "-none selected-", Value="" });
            this.Clients = list;
        }
        
        public TokenSearchCriteria SearchCriteria { get; private set; }
        public IEnumerable<SelectListItem> Clients { get; set; }


    }

    public class TokenSearchCriteria
    {
        public string Username { get; set; }
        public string Scope { get; set; }
        [ScaffoldColumn(false)]
        public int? ClientID { get; set; }
    }
}