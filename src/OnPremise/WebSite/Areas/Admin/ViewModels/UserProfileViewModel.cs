using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Profile;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel(string username, ProfilePropertyInputModel[] values)
        {
            this.Username = username;
            this.ProfileValues = new ProfilePropertyViewModel[values.Length];

            for (int i = 0; i < this.ProfileValues.Length; i++)
            {
                var prop = ProfileBase.Properties[values[i].Name];
                this.ProfileValues[i] = new ProfilePropertyViewModel(prop, values[i]);
            }
        }

        public UserProfileViewModel(string username)
        {
            this.Username = username;

            if (ProfileBase.Properties != null && ProfileBase.Properties.Count > 0)
            {
                var propertyList = ProfileBase.Properties.Cast<SettingsProperty>().ToArray();
                var profile = ProfileBase.Create(username);
                var values =
                    from property in propertyList
                    where 
                        (property.PropertyType.IsValueType && !property.PropertyType.IsGenericType) || 
                        property.PropertyType == typeof(string)
                    select new ProfilePropertyViewModel(property, Convert.ToString(profile[property.Name]));
                ProfileValues = values.ToArray();
            }
        }

        public bool UpdateProfileFromValues(ModelStateDictionary errors)
        {
            var profile = ProfileBase.Create(Username);
            for (int i = 0; i < ProfileValues.Length; i++)
            {
                var prop = ProfileBase.Properties[ProfileValues[i].Data.Name];
                try
                {
                    if (String.IsNullOrWhiteSpace(ProfileValues[i].Data.Value) && 
                        prop.PropertyType.IsValueType)
                    {
                        errors.AddModelError("profileValues[" + i + "].value", prop.Name + " is required.");
                    }
                    else
                    {
                        object val = Convert.ChangeType(ProfileValues[i].Data.Value, prop.PropertyType);
                        profile.SetPropertyValue(prop.Name, val);
                    }
                }
                catch (FormatException ex)
                {
                    errors.AddModelError("profileValues[" + i + "].value", "There was an error converting the value for " + prop.Name + ". " + ex.Message);
                }
                catch (Exception)
                {
                    errors.AddModelError("profileValues[" + i + "].value", "There was an error converting the value for " + prop.Name + ". It must be of type " + prop.PropertyType.Name + ".");
                }
            }

            if (errors.IsValid)
            {
                profile.Save();
            }

            return errors.IsValid;
        }

        public string Username { get; set; }
        public ProfilePropertyViewModel[] ProfileValues { get; set; }
    }

    public class ProfilePropertyViewModel
    {
        public ProfilePropertyViewModel(SettingsProperty property, ProfilePropertyInputModel value)
        {
            Type = PropTypeFromPropertyType(property);
            Description = property.Name + " must be of type " + property.PropertyType.Name;
            if (property.PropertyType.IsValueType) Description += " and is required";
            Description += ".";
            Data = value;
        }

        public ProfilePropertyViewModel(SettingsProperty property, string value)
            : this(property, 
                    new ProfilePropertyInputModel
                    {
                        Name = property.Name,
                        Value = value
                    })
        {
        }
        
        ProfilePropertyViewModel.ProfilePropertyType PropTypeFromPropertyType(SettingsProperty prop)
        {
            return prop.PropertyType == typeof(Boolean) ?
                    ProfilePropertyViewModel.ProfilePropertyType.Boolean :
                    ProfilePropertyViewModel.ProfilePropertyType.String;
        }

        public enum ProfilePropertyType
        {
            String,
            Boolean
        }

        public ProfilePropertyType Type { get; set; }
        public string Description { get; set; }
        public ProfilePropertyInputModel Data { get; set; }
    }

    public class ProfilePropertyInputModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}