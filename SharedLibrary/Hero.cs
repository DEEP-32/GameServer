namespace SharedLibrary {

    [System.Serializable]
    public class Hero {
        public int Id { get; set; }
        public int Name { get; set; }
        public User User { get; set;}
    }
}