using Newtonsoft.Json;

namespace HashiAPI_1.Models {

    public class WrikeUser {

            // in the JSON, email is found inside the "profiles" list, 
            // within the "email" field of a profile entry
            // we don't really need it and it's annoying to access to we will ignore it
            // it might be nice to add later for searching users
        public string? Email {get; set;}
        public string? FirstName {get; set;}
        public string? LastName {get; set;}
        public string? Id {get; set;}
        public string? Type {get; set;}

        public string DisplayName() {
            return FirstName + " " + LastName;
        }

        override public string ToString() {
            return $"{DisplayName()} | {Email}";
        }

    }
}