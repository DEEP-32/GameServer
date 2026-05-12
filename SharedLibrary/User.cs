using System.Collections.Generic;

namespace SharedLibrary {
    public class User {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public List<Hero> Heroes { get; set; }

    }
}