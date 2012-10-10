using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class DelegationSettingsForUserInputModel
    {
        private Repositories.IDelegationRepository delegationRepository;

        public DelegationSettingsForUserInputModel(Repositories.IDelegationRepository delegationRepository, string username)
        {
            this.delegationRepository = delegationRepository;
            this.UserName = username;
        }

        [Required]
        public string UserName { get; set; }
        DelegationSettingInputModel[] settings;
        public DelegationSettingInputModel[] Settings
        {
            get
            {
                if (settings == null)
                {
                    if (this.UserName == null)
                    {
                        settings = Enumerable.Empty<DelegationSettingInputModel>().ToArray();
                    }
                    else
                    {
                        settings =
                            this
                                .delegationRepository
                                .GetDelegationSettingsForUser(this.UserName)
                                .Select(x => new DelegationSettingInputModel { Description = x.Description, Realm = x.Realm })
                                .ToArray();
                    }
                }
                return settings;
            }
        }
    }

    public class DelegationSettingInputModel
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public Uri Realm { get; set; }

        public bool Delete { get; set; }
    }
}